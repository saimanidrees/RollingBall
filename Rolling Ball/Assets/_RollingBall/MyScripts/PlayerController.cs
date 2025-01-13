using Lean.Touch;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace HCStore.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] internal float fwdSpeed = 5;
        [SerializeField] private float fingerSensitivity = 5;
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] internal float roadWidth = 5;
        [SerializeField] private float Ypos = 5;
        [SerializeField] Collider ChildCollider;
        public PhysicMaterial NoFriction;
        [Header("Ref")]
        [SerializeField] internal Rigidbody RigidBody;
        //[SerializeField] internal MergeInfinityBall InfinteBall;
        [SerializeField] bool IsGrounded;
        private bool _isGameOn = false;
        private Camera _camera;
        private float _startX;
        private LeanFinger _currentFinger;
        internal bool AllowForwardMoment, useJump,UnderPole;
       
        private Transform StrechingPt, JumpTarget;
        internal bool StartStreching;
        #region UNity

        private void Awake()
        {
            _camera = Camera.main;
            AllowForwardMoment = true;
            ChildCollider.material = null;
           //RigidBody.isKinematic = true;
        }

        private void OnEnable()
        {
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerUp += OnFingerUp;
            timeStep = 1f / 50;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerUp -= OnFingerUp;
        }
        public float upForece = 2;
        private float timeStep;
        Vector3 velocity;
        Vector3 delta;
        float newX;
        Vector3 newPos;
        Vector3 dirToNewPos;
        Vector3 rbPos;
        private void FixedUpdate()
        {
           //if (!GameManager.Instance.PlayGame) return;
            if (AllowForwardMoment)
            {
                velocity = Vector3.forward * fwdSpeed;
                if (_currentFinger != null)
                {
                    delta = _currentFinger.GetWorldPosition(10, _camera) - _currentFinger.GetStartWorldPosition(10, _camera);
                    delta.z = 0;
                    delta.x *= fingerSensitivity;

                    newX = _startX + delta.x+ Ypos;
                    newPos = new Vector3(newX, transform.position.y, transform.position.z);
                    newPos.x = Mathf.Clamp(newPos.x, -roadWidth * .5f, roadWidth * .5f);
                    dirToNewPos = newPos - transform.position;
                    dirToNewPos.z = 0;
                    velocity += dirToNewPos * moveSpeed;
                }
               RigidBody.velocity = new Vector3(velocity.x, RigidBody.velocity.y, velocity.z);
                rbPos = RigidBody.position;
                rbPos.x = Mathf.Clamp(rbPos.x, -roadWidth * .5f, roadWidth * .5f);
                RigidBody.position = rbPos;
                if (!IsGrounded)
                {
                    RigidBody.velocity = Vector3.Lerp(RigidBody.velocity, Vector3.zero,5*Time.deltaTime);
                }

            }

            if (UnderPole&&!AllowForwardMoment&&!useJump&& !StartStreching)
            {
                RigidBody.velocity= Vector3.Lerp(RigidBody.velocity, Vector3.zero, 5 * Time.deltaTime);
                RigidBody.AddForce(-Vector3.up * 10, ForceMode.Impulse);
            }
            if (useJump)
            {
                if (JumpTarget)
                {
                    RigidBody.isKinematic = true;
                    RigidBody.Sleep();
                    RigidBody.velocity = Vector3.zero;
                    MidPt = (transform.position
                            + JumpTarget.position + Offset) / 2;
                    Vector3[] path = new Vector3[50];
                    //path = bazierCurve.DrawQuardicCurvePAth(transform.position, MidPt, JumpTarget.position);
                    useJump = false;
                    ChildCollider.material = NoFriction;
                }

            }
            if (StartStreching)
            {
                if (StrechingPt)
                {
                    transform.position = Vector3.MoveTowards(transform.position, StrechingPt.transform.position+ StrechOffset, StrechValue);
                    RigidBody.freezeRotation = false;
                }
            }

        }
        [SerializeField]internal float StrechValue;
        Vector3 MidPt;
        IEnumerator OffRigidBody()
        {
            RigidBody.isKinematic = false;
            yield return new WaitForEndOfFrame();
            RigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            RigidBody.AddForce(Vector3.up * upForece, ForceMode.Impulse);
        }
        internal void UnFreezeRigidBody()
        {
            RigidBody.Sleep();
            RigidBody.velocity = Vector3.zero;
            RigidBody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
        #endregion
        public Vector3 Offset,StrechOffset;
        #region Inputs

        private void OnFingerDown(LeanFinger obj)
        {
            if (!_isGameOn)
            {
                _isGameOn = true;
            }

            if (_currentFinger != null) return;
            _currentFinger = obj;
            _startX = transform.position.x;
        }

        private void OnFingerUp(LeanFinger obj)
        {
            if (_currentFinger != obj) return;
            _currentFinger = null;
        }

        #endregion
        public LayerMask GoundLayer;
        internal void SetStrechTraget(Transform Pt) => StrechingPt = Pt;
        internal void SetJumpTraget(Transform Pt) => JumpTarget = Pt;


        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                IsGrounded = true;
                OutOfEnv = false;
            }
            if (other.CompareTag("Env") && !IsPlayFall)
            {
                OutOfEnv = false;
                AllowForwardMoment = true;

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                IsGrounded = false;
            }
        }
        public bool OutOfEnv;
        internal bool IsPlayFall;
        IEnumerator PlayerOut(Transform DebegPos)
        {
            yield return new WaitForSeconds(.21f);
            if (!IsGrounded&& OutOfEnv)
            {                
                AllowForwardMoment = false;
                IsPlayFall = true;
                RigidBody.velocity = -Vector3.up;
                RigidBody.AddForce(-Vector3.up * 50, ForceMode.Impulse);
                //Debug.Log("PlayerExit" +" Not Gound"+ IsGrounded+"Out of Env"+ OutOfEnv, DebegPos);
                yield return new WaitForSeconds(0.51f);
                print("In Water");
            }

        }
        private void OnTriggerEnter(Collider other)
        {
           
        }
        internal void ReSizeMainCollider(float value) => GetComponent<SphereCollider>().radius = value + 0.05f;

        internal void SetBounceMat()
        {
        }
    }
}

  
