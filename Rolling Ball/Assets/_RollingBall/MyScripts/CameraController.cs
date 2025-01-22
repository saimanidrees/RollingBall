using System.Collections;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform ball; // The ball to follow
        private Rigidbody _ballRigidbody;
        [SerializeField] private Transform cameraTransform; // The camera's transform (child of CameraRoot)
        [Header("Camera Settings")]
        [SerializeField] private float height = 5f; // Fixed height offset of the camera
        [SerializeField] private float distance = 10f; // Fixed distance behind the ball
        [SerializeField] private float minAlignmentSpeed = 1f; // Minimum alignment speed
        [SerializeField] private float maxAlignmentSpeed = 5f; // Maximum alignment speed
        [SerializeField] private float orbitSpeed = 2f; // Speed for orbiting during reverse movement
        [Header("Look At Settings")]
        [SerializeField] private Vector3 lookAtOffset = Vector3.zero; // Offset for the look-at position
        private Vector3 _currentCameraDirection; // Current direction of the camera relative to the ball
        private bool _isBallMoving = false;
        [SerializeField]
        private bool pauseAlignment = false, pauseTheFollowing = false;
        private void Start()
        {
            _ballRigidbody = ball.GetComponent<Rigidbody>();
            // Initialize the camera direction to be directly behind the ball
            _currentCameraDirection = -ball.forward.normalized;
        }
        private void Update()
        {
            if (ball == null || cameraTransform == null)
            {
                Debug.LogWarning("CameraController: Ball or CameraTransform is not assigned!");
                return;
            }
            if (pauseTheFollowing) return;
            // Calculate ball's movement direction
            var ballForward = ball.forward;
            var ballSpeed = 0f;
            ballSpeed = _ballRigidbody.velocity.magnitude;
            if (ballSpeed > 0.2f)
            {
                ballForward = _ballRigidbody.velocity.normalized;
                _isBallMoving = true;
            }
            else
            {
                _isBallMoving = false;
            }
            // Handle camera behavior during reverse movement or alignment
            if (!pauseAlignment)
            {
                SmoothAlignAndOrbit(ballForward, ballSpeed);
            }
            // Update the camera's position and make it look at the ball
            var desiredPosition = ball.position + _currentCameraDirection * distance + Vector3.up * height;
            cameraTransform.position = desiredPosition;
            var lookAtPosition = ball.position + lookAtOffset;
            cameraTransform.LookAt(lookAtPosition);
        }
        private void SmoothAlignAndOrbit(Vector3 ballForward, float ballSpeed)
        {
            // Dynamically calculate alignment speed based on ball's speed
            var dynamicAlignmentSpeed = Mathf.Lerp(minAlignmentSpeed, maxAlignmentSpeed, ballSpeed / 10f);

            if (_isBallMoving)
            {
                // Align smoothly behind the ball's movement direction
                var targetDirection = -ballForward;
                _currentCameraDirection = Vector3.Slerp(
                    _currentCameraDirection,
                    targetDirection,
                    Time.deltaTime * dynamicAlignmentSpeed
                ).normalized;
            }
            else
            {
                // Orbit smoothly around the ball when reversing or stationary
                var orbitAngle = orbitSpeed * Time.deltaTime;
                var orbitRotation = Quaternion.AngleAxis(orbitAngle, Vector3.up);
                _currentCameraDirection = orbitRotation * _currentCameraDirection;
            }
            // Prevent abrupt flipping by constraining camera's vertical axis
            _currentCameraDirection.y = 0;
            _currentCameraDirection.Normalize();
        }
        public void PauseTheAlignment(bool flag)
        {
            pauseAlignment = flag;
            StartCoroutine(AlignCameraBehindBall());
        }
        private IEnumerator AlignCameraBehindBall()
        {
            // Align the camera directly behind the ball
            _currentCameraDirection = -ball.forward.normalized;
            var position = ball.position;
            // Calculate the desired camera position
            var desiredPosition = position + _currentCameraDirection * distance + Vector3.up * height;
            // Set the camera position and look at the ball
            cameraTransform.position = desiredPosition;
            cameraTransform.LookAt(position + lookAtOffset);
            yield return null;
            pauseAlignment = false;
        }
        public void SetViewForLevelEnd()
        {
            pauseAlignment = true;
            StartCoroutine(AlignCameraForLevelEnd());
        }
        private IEnumerator AlignCameraForLevelEnd()
        {
            const float duration = 50f;
            var time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime / duration;
                distance = Mathf.Lerp(distance, 10, time);
                height = Mathf.Lerp(height, 5, time);
                yield return null;
            }
        }
        public void PauseTheAlignmentOnly()
        {
            pauseAlignment = true;
        }
        public void UnPauseTheAlignment()
        {
            pauseAlignment = false;
        }
        public void PauseTheFollowing(bool flag)
        {
            pauseTheFollowing = flag;
        }

        private Coroutine _coroutine;
        public void SetViewForBallSelection()
        {
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(AlignCameraForBallSelection());
        }
        private IEnumerator AlignCameraForBallSelection()
        {
            const float duration = 5f;
            var time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime / duration;
                height = Mathf.Lerp(height, 5, time);
                distance = Mathf.Lerp(distance, 8, time);
                lookAtOffset = new Vector3(lookAtOffset.x, Mathf.Lerp(lookAtOffset.y, 0, time), lookAtOffset.z);
                yield return null;
            }
            height = 5f;
            distance = 8f;
            lookAtOffset = new Vector3(lookAtOffset.x, 0f, lookAtOffset.z);
        }
        public void SetBackNormalView()
        {
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(AlignCameraBackToNormal());
        }
        private IEnumerator AlignCameraBackToNormal()
        {
            const float duration = 5f;
            var time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime / duration;
                height = Mathf.Lerp(height, 3, time);
                distance = Mathf.Lerp(distance, 6, time);
                lookAtOffset = new Vector3(lookAtOffset.x, Mathf.Lerp(lookAtOffset.y, 0.8f, time), lookAtOffset.z);
                yield return null;
            }
            height = 3f;
            distance = 6f;
            lookAtOffset = new Vector3(lookAtOffset.x, 0.8f, lookAtOffset.z);
        }
    }
}