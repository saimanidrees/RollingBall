using UnityEngine;

namespace _RollingBall.MyScripts
{
    public class CameraController : MonoBehaviour
    {
        public Transform ball; // Reference to the ball
        public Rigidbody ballRigidbody; // Rigidbody attached to the ball

        [Header("Follow Settings")] public float followSpeed = 10f; // Speed at which CameraRoot follows the ball
        public Vector3 offset = new Vector3(0, 5, -10); // Initial offset relative to the ball

        [Header("Orbit Settings")] public float orbitSpeed = 30f; // Speed of Pivot rotation around the ball

        private Transform cameraRoot; // CameraRoot for following the ball
        private Transform pivot; // Pivot for orbiting the ball
        private Transform cameraTransform; // Camera itself, always looks at the ball

        private Vector3 lastBallPosition; // To track the ball's previous position for direction checking
        private Vector3 ballDirection; // To store the direction of the ball's movement

        private void Start()
        {
            // Initialize references
            cameraRoot = transform;
            pivot = transform.GetChild(0); // Pivot should be the first child of CameraRoot
            cameraTransform = pivot.GetChild(0); // Camera should be the child of Pivot
            lastBallPosition = ball.position; // Set initial position of the ball
        }

        private void FixedUpdate()
        {
            if (ball == null || ballRigidbody == null) return;

            // Step 1: Smoothly follow the ball with CameraRoot, adjusting the position based on the offset
            Vector3 targetPosition = ball.position + offset;
            cameraRoot.position = Vector3.Lerp(cameraRoot.position, targetPosition, followSpeed * Time.fixedDeltaTime);

            // Step 2: Check the ball's movement direction
            ballDirection = ball.position - lastBallPosition;
            lastBallPosition = ball.position;

            // Step 3: If the ball is moving in the opposite direction, reverse the orbit direction
            if (ballDirection.sqrMagnitude > 0.01f) // Only check when the ball is moving
            {
                Vector3 flatDirection = new Vector3(ballDirection.x, 0, ballDirection.z).normalized; // Ignore Y-axis
                Vector3 pivotToBall = ball.position - pivot.position; // Direction from pivot to ball

                // Calculate dot product to determine if the ball is moving in the opposite direction
                if (Vector3.Dot(flatDirection, pivotToBall.normalized) < 0)
                {
                    orbitSpeed = -Mathf.Abs(orbitSpeed); // Reverse orbit direction
                }
                else
                {
                    orbitSpeed = Mathf.Abs(orbitSpeed); // Maintain normal orbit direction
                }

                // Step 4: Orbit the Pivot around the ball automatically (constant Y-axis rotation)
                pivot.RotateAround(ball.position, Vector3.up, orbitSpeed * Time.fixedDeltaTime);
            }

            // Step 5: Ensure the Camera always looks at the ball
            cameraTransform.LookAt(ball.position);
        }
    }
}