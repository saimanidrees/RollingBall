using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class BallMovement : MonoBehaviour, IMovable
    {
        private bool _allowMovement = true;
        private Rigidbody _rb;
        [SerializeField] private float maxSpeed = 10f, speedForCameraReCentering = 3f, defaultDrag = 0.3f, defaultAngularDrag = 0.5f;
        [SerializeField] private float minDrag = 0.05f, maxDrag = 2f, minAngularDrag = 0.05f, maxAngularDrag = 1.5f;
        private bool _applyMinDrag = false, _isFlyingUp = false;
        private readonly Vector3 _fakeGravity = new (0, 10, 0);
        private Vector3 _ballLastPosTransform;
        public void Initialize(Rigidbody body)
        {
            _rb = body;
        }
        public void Move(Vector3 direction)
        {
            if(!_allowMovement) return;
            _rb.AddForce(direction);
            if(_isFlyingUp)
            {
                _rb.velocity += _fakeGravity * Time.fixedDeltaTime;
            }
            if (_rb.velocity.magnitude > maxSpeed)
            {
                _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);
            }
        }
        public void Push(Vector3 direction)
        {
            _rb.AddForce(direction, ForceMode.Impulse);
        }
        public void IncreaseDrag()
        {
            if(_applyMinDrag)
            {
                _rb.drag = minDrag;
                _rb.angularDrag = minAngularDrag;
                return;
            }
            _rb.drag = maxDrag;
            _rb.angularDrag = maxAngularDrag;
        }
        public void DecreaseDrag()
        {
            if(_applyMinDrag)
            {
                _rb.drag = minDrag;
                _rb.angularDrag = minAngularDrag;
                return;
            }
            _rb.drag = defaultDrag;
            _rb.angularDrag = defaultAngularDrag;
        }
        public void MinimumDrag(bool flag)
        {
            _applyMinDrag = flag;
        }
        public void FlyUp(bool flag)
        {
            _isFlyingUp = flag;
            _rb.useGravity = !flag;
        }
        public void AllowMovement(bool flag)
        {
            _allowMovement = flag;
        }
        public void SetBallLastPos(Vector3 newPos)
        {
            _ballLastPosTransform = newPos;
        }
        public Vector3 GetBallLastPos()
        {
            return _ballLastPosTransform;
        }
    }
    public interface IMovable
    {
        void Move(Vector3 direction);
        void Push(Vector3 direction);
        void IncreaseDrag();
        void DecreaseDrag();
        void MinimumDrag(bool flag);
        void FlyUp(bool flag);
        void AllowMovement(bool flag);
        void SetBallLastPos(Vector3 newPos);
    }
}