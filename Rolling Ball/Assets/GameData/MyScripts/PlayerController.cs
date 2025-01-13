using Lean.Touch;
using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerCameraTargetPoint playerCameraTargetPoint;
        [SerializeField] internal float fwdSpeed = 5;
        [SerializeField] private float fingerSensitivity = 5;
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] internal float roadWidth = 5;
        [SerializeField] private float Ypos = 5;
        [SerializeField] Collider ChildCollider;
        [Header("Ref")]
        [SerializeField] internal Rigidbody RigidBody;
        private Camera _camera;
        private float _startX;
        private LeanFinger _currentFinger;
        internal bool startRun;
        
        public bool horizontalMovement = false;
       
        private float timeStep;
        private Vector3 velocity;
        private Vector3 delta, firstDelta;
        private float newX;
        private Vector3 newPos;
        private Vector3 dirToNewPos;
        private Vector3 rbPos;
        public float minLimit = -5f;  
        public float maxLimit = 5f;
        
        [SerializeField] private MagnetPowerUp magnetPowerUp;
        [SerializeField] private Transform trails;
        
        #region UNity

        private void Awake()
        {
            _camera = Camera.main;
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

        public void Show(string msg)
        {
            Debug.Log(msg + delta);
        }
        private void Update()
        {
            if (!startRun) return;
            if (!horizontalMovement) return;
            var targetPos = transform.localPosition;
            if(targetPos.x < minLimit) {
                playerCameraTargetPoint.FollowtoXaxis(false);
            }
            else if (targetPos.x > maxLimit)
            {
                playerCameraTargetPoint.FollowtoXaxis(true);
            }else {
                playerCameraTargetPoint.BackToNormal();
            }
        }
        private void FixedUpdate()
        {
            if (startRun)
            {
                velocity = Vector3.forward * fwdSpeed;
                if (_currentFinger != null)
                {
                    delta = _currentFinger.GetWorldPosition(50, _camera) - _currentFinger.GetStartWorldPosition(50, _camera);
                    delta.z = 0;
                    if (horizontalMovement)
                    {
                        //if (delta.x > 0 || delta.x < 0)
                        {
                            delta.x *= fingerSensitivity;

                            newX = _startX + delta.x + Ypos;
                            newPos = new Vector3(newX, transform.position.y, transform.position.z);
                            newPos.x = Mathf.Clamp(newPos.x, -roadWidth * .5f, roadWidth * .5f);

                            dirToNewPos = newPos - transform.position;
                            dirToNewPos.z = 0;
                            velocity += dirToNewPos * moveSpeed;
                        }
                    }
                }
                RigidBody.velocity = new Vector3(velocity.x, RigidBody.velocity.y, velocity.z);
                rbPos = RigidBody.position;
                rbPos.x = Mathf.Clamp(rbPos.x, -roadWidth * .5f, roadWidth * .5f);
                RigidBody.position = rbPos;
                magnetPowerUp.AttractBallsByMagnet();
            }
            trails.position = transform.position;
        }

        #endregion
        
        #region Inputs

        private void OnFingerDown(LeanFinger finger)
        {
            if (!startRun)
            {
                return;
            }

            if (_currentFinger != null) return;
            _currentFinger = finger;
            _startX = transform.position.x;
            firstDelta = _currentFinger.SwipeScreenDelta;

            //Debug.Log("OnFingerDowndelta: " + _currentFinger.ScaledDelta);
        }
        private void OnFingerUp(LeanFinger finger)
        {
            if (_currentFinger != finger) return;
            //Debug.Log("OnFingerUpdelta: " + _currentFinger.ScaledDelta);
            _currentFinger = null;
        }

        #endregion
        
        public void StartMovement(bool flag)
        {
            startRun = flag;
        }
        public void StartMagnetEffect()
        {
            magnetPowerUp.gameObject.SetActive(true);
            magnetPowerUp.ActivateMagnetEffect(true);
        }
        public Transform GetCameraTarget()
        {
            var transform1 = playerCameraTargetPoint.transform;
            transform1.position = transform.position;
            return transform1;
        }
}