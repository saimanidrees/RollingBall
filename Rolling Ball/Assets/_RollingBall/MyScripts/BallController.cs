using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class BallController : MonoBehaviour
    {
        public float force = 10f;
        private const float PushForce = 10f;
        private IInputProvider _inputProvider;
        private IMovable _movable;
        [SerializeField] private Transform cam;
        [SerializeField] private Rigidbody body;
        private void Start()
        {
            _inputProvider = GetComponent<IInputProvider>();
            _movable = GetComponent<IMovable>();
            if (_movable is BallMovement ballMovement)
            {
                ballMovement.Initialize(body);
            }
        }
        private void Update()
        {
            var moveHorizontal = _inputProvider.GetHorizontalInput();
            var moveVertical = _inputProvider.GetVerticalInput();
            if (moveHorizontal == 0f && moveVertical == 0)
            {
                _movable.DecreaseDrag();
            }
            else
            {
                _movable.IncreaseDrag();
            }
            /*if (moveVertical != 0)
            cineMachineFreeLook.m_RecenterToTargetHeading.m_enabled = true;
        else
            cineMachineFreeLook.m_RecenterToTargetHeading.m_enabled = false;*/
            //cineMachineFreeLook.m_XAxis.Value = moveVertical * Time.deltaTime;
            //var movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * speed;
            //var direction = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
            //var direction = (transform.localPosition - cam.localPosition).normalized;
            //if (direction.magnitude >= 0.1f)
            /*{
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            var movement = new Vector3(0f, targetAngle, 0f); //* Vector3.forward;
            _movable.Move(movement * speed * Time.deltaTime);
        }*/
            //var movement = new Vector3(moveHorizontal, 0.0f, direction.z * moveVertical) * speed;
            //var movement = new Vector3(cam.right.x * moveHorizontal, 0.0f, cam.forward.z * moveVertical) * speed;
            var forward = cam.forward;
            var right = cam.right;
            var forwardMovement = moveVertical * forward;
            var sideMovement = moveHorizontal * right;
            var movement = forwardMovement + sideMovement;
            movement = new Vector3(movement.x, -8f, movement.z);
            //movement = new Vector3(Mathf.Clamp(movement.x, -10f, 10f), -10f, Mathf.Clamp(movement.z, -10f, 10f));
            //Debug.Log(movement);
            _movable.Move(movement * force);
        }
        public void PushForward()
        {
            _movable.Push(Vector3.forward * (force * PushForce));
        }
        public void AllowMovement(bool flag)
        {
            body.isKinematic = !flag;
        }
    }
}