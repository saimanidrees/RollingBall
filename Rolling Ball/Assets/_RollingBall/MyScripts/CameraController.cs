using System.Collections;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")] public Transform ball; // The ball to follow
        public Transform cameraTransform; // The camera's transform (child of CameraRoot)

        [Header("Camera Settings")] public float height = 5f; // Height offset of the camera
        public float distance = 10f; // Distance behind the ball

        [Header("Alignment Settings")] public float baseAlignmentSpeed = 1f; // Minimum alignment speed
        public float maxAlignmentSpeed = 5f; // Maximum alignment speed
        public float speedThreshold = 10f; // Maximum ball speed for scaling alignment speed

        [Header("Look At Settings")] public Vector3 lookAtOffset = Vector3.zero; // Offset for the look-at position

        private Vector3 currentCameraDirection; // Current direction of the camera relative to the ball
        private bool isBallMoving = false;
        [SerializeField] private bool pauseAlignment = false;
        void Start()
        {
            // Initialize the camera direction to be directly behind the ball
            currentCameraDirection = -ball.forward.normalized;
        }
        private void Update()
        {
            if (ball == null || cameraTransform == null)
            {
                Debug.LogWarning("CameraController: Ball or CameraTransform is not assigned!");
                return;
            }

            // Calculate the direction the ball is moving and its speed
            Vector3 ballForward = ball.forward; // Default to the ball's forward direction
            float ballSpeed = 0f; // Speed of the ball
            if (ball.GetComponent<Rigidbody>() != null)
            {
                Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
                ballSpeed = ballRigidbody.velocity.magnitude;

                if (ballSpeed > 0.2f) // Ball is moving
                {
                    ballForward = ballRigidbody.velocity.normalized;
                    isBallMoving = true;
                }
                else
                {
                    isBallMoving = false;
                }
            }

            // Dynamically adjust alignment speed based on ball's speed
            var alignmentSpeed = Mathf.Lerp(baseAlignmentSpeed, maxAlignmentSpeed, ballSpeed / speedThreshold);
            var desiredCameraPosition = Vector3.zero;

            // If the ball alignment is paused then 
            if (pauseAlignment)
            {
                //return;
            }
            else
            {
                // If the ball is moving, smoothly align the camera to the ball's movement direction
                if (isBallMoving)
                {
                    currentCameraDirection = Vector3
                        .Slerp(currentCameraDirection, -ballForward, Time.deltaTime * alignmentSpeed).normalized;
                }
            }

            // Calculate the desired position for the camera (orbiting around the ball)
            desiredCameraPosition = ball.position + currentCameraDirection * distance + Vector3.up * height;

            // Smoothly move the camera to the desired position
            //cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredCameraPosition, Time.deltaTime * smoothSpeed);
            cameraTransform.position = desiredCameraPosition;

            // Make the camera look at the ball with the offset
            Vector3 lookAtPosition = ball.position + lookAtOffset;
            cameraTransform.LookAt(lookAtPosition);
        }
        public void PauseTheAlignment(bool flag)
        {
            pauseAlignment = flag;
            StartCoroutine(AlignCameraBehindBall());
        }
        private IEnumerator AlignCameraBehindBall()
        {
            // Align the camera directly behind the ball
            currentCameraDirection = -ball.forward.normalized;
            var position = ball.position;
            // Calculate the desired camera position
            var desiredPosition = position + currentCameraDirection * distance + Vector3.up * height;
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
            //StartCoroutine(AlignCameraBackToNormal());
        }
        private IEnumerator AlignCameraBackToNormal()
        {
            const float duration = 50f;
            var time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime / duration;
                distance = Mathf.Lerp(distance, 8, -time);
                yield return null;
            }
        }
    }
}