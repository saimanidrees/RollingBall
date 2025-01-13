using UnityEngine;
using CnControls;
using GameData.MyScripts;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed, actualSpeed = 500f;
    public float axisSpeed;
    [SerializeField] private float minHorizontalMovement = -9f, maxHorizontalMovement = 9f;
    public bool startRun, horizontalMovement;
    public bool isGrounded;
    public bool levelComplete;
    [SerializeField] private Animator ball;
    [SerializeField] private LayerMask layersToDetect = 0;
    [SerializeField] private Transform cameraTarget;
    private bool _invokedFlag = false;
    private static readonly int State = Animator.StringToHash("state");
    [SerializeField] private MagnetPowerUp magnetPowerUp;
    [SerializeField] private Transform forwardTransform;
    
    public float minLimit = -5f;  
    public float maxLimit = 5f;
    public float lerpSpeed = 5f;
    private Vector3 _startPos;
    private Vector3 _lastTargetPos;

    private float _score = 0f;
    private int _scoreI = 0;
    private bool _isHighScoreNotificationShowed = false;

    [SerializeField] private MergeInfinityBall mergeInfinityBall;

    private void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
        _startPos = cameraTarget.localPosition;  
        _lastTargetPos = transform.localPosition;
    }
    public void StartMovement(bool flag)
    {
        startRun = flag;
        ball.enabled = flag;
    }
    private void Update()
    {
        if (!startRun) return;
        if (!horizontalMovement) return;
        var targetPos = transform.localPosition;
        if(CheckLimits(targetPos)) {
            var offsetPos = GetOffsetPosition(targetPos);
            FollowWithLerp(offsetPos);
        } else {
            LerpBackToStart();
        }
        _lastTargetPos = targetPos;
    }
    private void FixedUpdate()
    {
        if(!startRun) return;
        if (startRun && isGrounded)
        {
            if(forwardTransform)
                rb.velocity = forwardTransform.forward * speed * Time.fixedDeltaTime;
            else
                rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
            magnetPowerUp.AttractBallsByMagnet();
        }
        if (!levelComplete)
        {
            if (horizontalMovement)
            {
                {
                    //var move = joystick.Horizontal * axisSpeed * Time.deltaTime;
                    var move = CnInputManager.GetAxis("Horizontal") * axisSpeed * Time.deltaTime;
                    this.transform.localPosition += new Vector3(move, 0, 0);
                    this.transform.localPosition = new Vector3(Mathf.Clamp(this.transform.localPosition.x, minHorizontalMovement, maxHorizontalMovement), this.transform.localPosition.y, this.transform.localPosition.z);
                }
            }
        }
        if(!GameManager.Instance.IsInfiniteMode())
            return;
        _score += Time.fixedDeltaTime;
        _scoreI = (int)_score;
        GamePlayManager.Instance.GetGamePlayUIManager().controls.SetScoreText(_scoreI);
        if (_scoreI > PlayerPrefsHandler.HighScore)
        {
            if (!_isHighScoreNotificationShowed)
            {
                _isHighScoreNotificationShowed = true;
                GamePlayManager.Instance.GetGamePlayUIManager().controls.ShowHighScoreNotification();
            }
            PlayerPrefsHandler.HighScore = _scoreI;
            GamePlayManager.Instance.GetGamePlayUIManager().controls.SetHighScoreText(_scoreI);
        }
        if(mergeInfinityBall)
            mergeInfinityBall.SetStackedBallsPosition();
    }
    public void OnCollisionStay(Collision collision)
    {
        if (!IsInLayerMask(collision.gameObject, layersToDetect))
            return;
        if (collision.gameObject.tag.Equals(PlayerPrefsHandler.Ground))
        {
            isGrounded = true;
            speed = actualSpeed;
        }
        if (collision.gameObject.tag.Equals(PlayerPrefsHandler.SlopeDownward))
        {
            isGrounded = true;
            speed = 20f;
        }
        if (collision.gameObject.tag.Equals(PlayerPrefsHandler.SlopeUpward))
        {
            isGrounded = true;
            speed = actualSpeed + (actualSpeed / 2f);
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (!IsInLayerMask(collision.gameObject, layersToDetect))
            return;
        if (collision.gameObject.tag.Equals(PlayerPrefsHandler.Ground))
        {
            isGrounded = false;
            if (!_invokedFlag)
            {
                if (!isGrounded)
                {
                    _invokedFlag = true;
                    Invoke(nameof(SpeedZero), 0.25f);
                }
            }
        }
        if (collision.gameObject.tag.Equals(PlayerPrefsHandler.SlopeDownward))
        {
            isGrounded = false;
            speed = 0f;
            /*if (!_invokedFlag)
            {
                if (!isGrounded)
                {
                    _invokedFlag = true;
                    Invoke(nameof(SpeedZero), 0.25f);
                }
            }*/
        }
        if (collision.gameObject.tag.Equals(PlayerPrefsHandler.SlopeUpward))
        {
            isGrounded = false;
            if (!_invokedFlag)
            {
                if (!isGrounded)
                {
                    _invokedFlag = true;
                    Invoke(nameof(SpeedZero), 0.25f);
                }
            }
        }
    }
    private void SpeedZero()
    {
        speed = 0f;
        _invokedFlag = false;
    }
    private static bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
    public void SetBallToRotate(Animator ballAnimator)
    {
        ball = ballAnimator;
        if (ball.enabled == false)
            ball.enabled = true;
    }
    public void ResetCameraTarget()
    {
        var localPosition = cameraTarget.localPosition;
        localPosition = new Vector3(transform.localPosition.x, localPosition.y, localPosition.z);
        cameraTarget.localPosition = localPosition;
    }
    public Transform GetCameraTarget()
    {
        cameraTarget.position = transform.position;
        return cameraTarget;
    }
    public void IncreaseActualSpeed(int incValue)
    {
        actualSpeed += incValue;
        actualSpeed = Mathf.Clamp(actualSpeed, 300, 600);
    }
    public void StartMagnetEffect()
    {
        magnetPowerUp.gameObject.SetActive(true);
        magnetPowerUp.ActivateMagnetEffect(true);
    }
    public Animator GetBallAnimator()
    {
        return ball;
    }
    public void SetForwardTransform(Transform forwardT)
    {
        forwardTransform = forwardT;
    }
    public int GetScore()
    {
        return _scoreI;
    }

    #region Camera Target Methods

    private bool CheckLimits(Vector3 pos) {
        return pos.x < minLimit || pos.x > maxLimit;
    }
    private Vector3 GetOffsetPosition(Vector3 pos) {

        var offset = 1f * ball.transform.localScale.x;
        if(pos.x < 0 && _lastTargetPos.x >= pos.x)  
            pos.x += offset;
        else if(pos.x > 0 && _lastTargetPos.x <= pos.x)
            pos.x -= offset;
        _lastTargetPos = pos;
        return pos;
    }
    private void FollowWithLerp(Vector3 targetPos) {

        cameraTarget.localPosition = Vector3.Lerp(
            cameraTarget.localPosition, 
            targetPos, 
            Time.deltaTime / lerpSpeed);
    }
    private void LerpBackToStart() {

        cameraTarget.localPosition = Vector3.Lerp(
            cameraTarget.localPosition,
            _startPos,
            Time.deltaTime / lerpSpeed);
    }

    #endregion
}