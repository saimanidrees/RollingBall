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
            var forward = cam.forward;
            var right = cam.right;
            var forwardMovement = moveVertical * forward;
            var sideMovement = moveHorizontal * right;
            var movement = forwardMovement + sideMovement;
            movement = new Vector3(movement.x, -8f, movement.z);
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