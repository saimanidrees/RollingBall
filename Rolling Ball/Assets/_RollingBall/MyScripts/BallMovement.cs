using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class BallMovement : MonoBehaviour, IMovable
    {
        private bool _allowMovement = true;
        private Rigidbody _rb;
        [SerializeField] private float maxSpeed = 10f, speedForCameraReCentering = 3f;
        private bool _applyMinDrag = false, _isFlyingUp = false;
        private readonly Vector3 _fakeGravity = new (0, 10, 0);
        public void Initialize(Rigidbody body)
        {
            _rb = body;
        }
        public void Move(Vector3 direction)
        {
            if(!_allowMovement) return;
            _rb.AddForce(direction);
            //Debug.Log(direction);
            if(_isFlyingUp)
            {
                _rb.velocity += _fakeGravity * Time.fixedDeltaTime;
            }
            if (_rb.velocity.magnitude > speedForCameraReCentering)
            {
                GamePlayManager.Instance.SetReCentering(true);
            }
            else
            {
                GamePlayManager.Instance.SetReCentering(false);
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
                _rb.drag = 0.05f;
                _rb.angularDrag = 0.05f;
                return;
            }
            _rb.drag = 2f;
            _rb.angularDrag = 1.5f;
        }
        public void DecreaseDrag()
        {
            if(_applyMinDrag)
            {
                _rb.drag = 0.05f;
                _rb.angularDrag = 0.05f;
                return;
            }
            _rb.drag = 0.3f;
            _rb.angularDrag = 0.5f;
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
    }
}