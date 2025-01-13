using UnityEngine;

namespace _RollingBall.MyScripts
{
    public class CameraControl : MonoBehaviour
    {
        [Header("Target to Follow")] public Transform ball; // Reference to the ball's transform

        [Header("Camera Settings")] public Vector3 offset = new Vector3(0, 5, -10); // Default offset from the ball
        public float smoothSpeed = 0.125f; // Smoothness factor for camera movement

        void FixedUpdate()
        {
            if (ball != null)
            {
                // Calculate the desired position based on ball position and offset
                Vector3 desiredPosition = ball.position + offset;

                // Smoothly interpolate between the current position and the desired position
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

                // Update the camera's position
                transform.position = smoothedPosition;

                // Optionally, make the camera look at the ball
                transform.LookAt(ball);
            }
        }
    }
}