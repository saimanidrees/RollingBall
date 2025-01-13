using System.Collections;
using GameData.MyScripts;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    public enum CharacterType
    {
        Human, Train, Vehicle
    }
    [SerializeField] private CharacterType characterType = CharacterType.Human;
    [SerializeField] private bool allowMovement = false, loopMovement = false, allowDeath = true;
    [SerializeField] private float moveSpeed = 3.0f, rotationSpeed = 3.0f, closeDistance = 1f, gravityForce = 1f;
    private Transform _target;
    private CharacterController _characterController;
    private Animator _animator;
    private Vector3 _velocity, _startPos;
    [SerializeField] private Transform waypoints;
    private int _waypointIndex;
    private bool _waypointsEndFlag = false;
    public UnityEvent onWaypointsEnds, onEnteringTaxi;
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _target = waypoints.GetChild(_waypointIndex);
        _startPos = transform.localPosition;
    }

    private void Update()
    {
        if(!allowMovement) return;
        if (IsTargetCloseEnough())
        {
            if (_waypointIndex < waypoints.childCount - 1)
            {
                _waypointIndex++;
                _target = waypoints.GetChild(_waypointIndex);
            }
            else if (_waypointIndex == waypoints.childCount - 1)
            {
                if (loopMovement)
                {
                    ResetMovement();
                }
                else
                {
                    PlayAnimation("Idle");
                    if (_waypointsEndFlag)
                        return;
                    _waypointsEndFlag = true;
                    allowMovement = false;
                    onWaypointsEnds.Invoke();
                }
            }
        }
        else
        {
            PlayAnimation("Walk");
            if (_characterController.isGrounded)
            {
                var direction = _target.position - transform.position;
                direction.Normalize();
                direction.y = 0;
                var fastLook = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, fastLook, Time.deltaTime * rotationSpeed);
                _velocity = direction * moveSpeed;
            }
            _velocity.y -= gravityForce;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
    private bool IsTargetCloseEnough()
    {
        var distance = Vector3.Distance(transform.position, _target.position);
        return distance <= closeDistance;
    }
    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }
    public void AllowMovement(bool flag)
    {
        allowMovement = flag;
    }
    private void Die(Transform killer)
    {
        AllowMovement(false);
        GetComponent<CharacterController>().enabled = false;
        if (characterType == CharacterType.Human)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            if (allowDeath)
            {
                _animator.enabled = false;
                _animator.GetBoneTransform(HumanBodyBones.Spine).GetComponent<Rigidbody>().AddForce(killer.forward * 200f, ForceMode.Impulse);
            }
            else
            {
                StartCoroutine(DelayToEnterTaxi());
            }
        }
        else
        {
            
        }
        if (allowDeath)
            GamePlayManager.Instance.GameOver(3);
    }
    private void ResetMovement()
    {
        Debug.Log("ResetMovement");
        transform.localPosition = _startPos;
        _waypointIndex = 0;
        _target = waypoints.GetChild(_waypointIndex);
        _waypointsEndFlag = false;
        allowMovement = true;
    }
    private IEnumerator DelayToEnterTaxi()
    {
        transform.Find("TaxiEntryEffect").gameObject.SetActive(true);
        yield return null;
        _animator.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        onEnteringTaxi.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player") && !other.gameObject.tag.Equals("Trailer"))
            return;
        Die(other.transform);
    }
}