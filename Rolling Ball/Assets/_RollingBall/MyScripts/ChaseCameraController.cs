using CnControls;
using UnityEngine;

public class ChaseCameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 0.125f;
    public float dampingFactor = 0.5f;
    public float lookAtOffsetY = 0, lookAtSpeed = 200f;

    private float offset = 0f, currentOffset = 0f, incValue = 0.02f;

    // Waypoints
    public int waypointCount = 10; // Number of waypoints between the current and desired position.
    private Vector3[] waypoints;

    private Vector3 previousBallVelocity; // Track previous velocity of the ball for comparison.

    private void Start()
    {
        // Initialize waypoints array
        waypoints = new Vector3[waypointCount];
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        var rb = target.GetComponent<Rigidbody>();
        if (rb == null) return;

        // Get the current velocity of the ball
        var currentBallVelocity = rb.velocity;

        // Check if the ball's direction has reversed relative to the camera's facing direction
        if (previousBallVelocity != Vector3.zero && Vector3.Dot(previousBallVelocity, currentBallVelocity) < 0)
        {
            // Apply curve when the ball reverses direction
            Debug.Log("Ball changed direction, applying curve.");
            //distance *= 1.1f; // Optionally increase the distance to simulate "orbit" effect.
        }

        // Update the ball's previous velocity for next comparison
        previousBallVelocity = currentBallVelocity;

        // Get the ball's forward direction based on its velocity
        var forwardDirection = currentBallVelocity.normalized;
        if (forwardDirection.magnitude < 0.1f) forwardDirection = transform.forward;

        // Desired camera position (based on ball's movement)
        var desiredPosition = target.position - forwardDirection * distance + Vector3.up * height;

        // Calculate direction to target (from camera to ball)
        Vector3 directionToTarget = target.position - transform.position;

        // Apply curve offset if needed when ball moves in the opposite direction
        if (Vector3.Dot(forwardDirection, transform.forward) < 0)
        {
            // Ball is moving in the opposite direction to the camera's facing direction
            desiredPosition = ApplyCurveOffset(desiredPosition, directionToTarget, currentBallVelocity);
        }

        // Create waypoints between the current position and desired position
        CreateWaypoints(transform.position, desiredPosition);

        // Move camera through waypoints
        FollowWaypoints();

        // Adjust camera rotation to always look at the target
        var position1 = target.position;
        var _targetPos = new Vector3(position1.x, position1.y + lookAtOffsetY, position1.z);
        var targetRotation = Quaternion.LookRotation(_targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
    }

    private void CreateWaypoints(Vector3 startPos, Vector3 endPos)
    {
        // Interpolate between the start and end position to create waypoints
        for (int i = 0; i < waypointCount; i++)
        {
            float t = (i + 1) / (float)(waypointCount + 1); // Normalize between 0 and 1
            waypoints[i] = Vector3.Lerp(startPos, endPos, t);
        }
    }

    private void FollowWaypoints()
    {
        // Move through the waypoints
        for (int i = 0; i < waypointCount; i++)
        {
            transform.position = Vector3.Lerp(transform.position, waypoints[i], smoothSpeed * dampingFactor);
        }
    }

    private Vector3 ApplyCurveOffset(Vector3 desiredPosition, Vector3 directionToTarget, Vector3 ballVelocity)
    {
        // Calculate the curve based on the ball's velocity (the strength of the curve)
        float curveStrength = Mathf.Sign(ballVelocity.x) * 0.5f; // You can adjust this value for stronger/weaker curves.

        // Apply a smooth curve using sine and cosine to simulate orbital behavior
        Vector3 curveOffset = new Vector3(0, Mathf.Sin(Time.time * curveStrength) * 0.5f, Mathf.Cos(Time.time * curveStrength) * 0.5f);

        // Modify the desired position with the curve offset
        return desiredPosition + curveOffset;
    }
}
