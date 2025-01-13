using UnityEngine;
using UnityEngine.EventSystems;

namespace CnControls
{
    public class Touchpad : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        /// <summary>
        /// Current event camera reference. Needed for the sake of Unity Remote input
        /// </summary>
        public Camera CurrentEventCamera { get; set; }

        /// <summary>
        /// The name of the horizontal axis for this touchpad to update
        /// </summary>
        public string horizontalAxisName = "Horizontal";

        /// <summary>
        /// The name of the vertical axis for this touchpad to update
        /// </summary>
        public string verticalAxisName = "Vertical";

        /// <summary>
        /// Whether this touchpad should preserve inertia when the finger is lifted
        /// </summary>
        public bool preserveInertia = true;

        /// <summary>
        /// The speed of decay of inertia
        /// </summary>
        public float friction = 3f, sensitivity = 3f;

        private VirtualAxis _horizontalAxis;
        private VirtualAxis _verticalAxis;
        private int _lastDragFrameNumber;
        private bool _isCurrentlyTweaking;

        /// <summary>
        /// Joystick movement direction
        /// Specifies the axis along which it can move
        /// </summary>
        [Tooltip("Constraints on the joystick movement axis")]
        public ControlMovementDirection controlMoveAxis = ControlMovementDirection.Both;

        private void OnEnable()
        {
            // When we enable, we get our virtual axis

            _horizontalAxis = _horizontalAxis ?? new VirtualAxis(horizontalAxisName);
            _verticalAxis = _verticalAxis ?? new VirtualAxis(verticalAxisName);

            // And register them in our input system
            CnInputManager.RegisterVirtualAxis(_horizontalAxis);
            CnInputManager.RegisterVirtualAxis(_verticalAxis);
        }

        private void OnDisable()
        {
            // When we disable, we just unregister our axis
            // It also happens before the game object is Destroyed
            CnInputManager.UnregisterVirtualAxis(_horizontalAxis);
            CnInputManager.UnregisterVirtualAxis(_verticalAxis);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            // Some bitwise logic for constraining the touchpad along one of the axis
            // If the "Both" option was selected, non of these two checks will yield "true"
            if ((controlMoveAxis & ControlMovementDirection.Horizontal) != 0)
            {
                _horizontalAxis.Value = eventData.delta.x / sensitivity;
            }
            if ((controlMoveAxis & ControlMovementDirection.Vertical) != 0)
            {
                _verticalAxis.Value = eventData.delta.y / sensitivity;
            }

            _lastDragFrameNumber = Time.renderedFrameCount;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isCurrentlyTweaking = false;
            if (!preserveInertia)
            {
                _horizontalAxis.Value = 0f;
                _verticalAxis.Value = 0f;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isCurrentlyTweaking = true;
            OnDrag(eventData);
        }

        private void Update()
        {
            if (_isCurrentlyTweaking && _lastDragFrameNumber < Time.renderedFrameCount - 2)
            {
                _horizontalAxis.Value = 0f;
                _verticalAxis.Value = 0f;
            }

            if (preserveInertia && !_isCurrentlyTweaking)
            {
                _horizontalAxis.Value = Mathf.Lerp(_horizontalAxis.Value, 0f, friction * Time.deltaTime);
                _verticalAxis.Value = Mathf.Lerp(_verticalAxis.Value, 0f, friction * Time.deltaTime);
            }
        }
    }
}