using UnityEngine;

public class BallCamera : MonoBehaviour
{
    public Transform playerTransform;   // Reference to the player's transform
    public Vector3 cameraOffset;        // Offset from the player's position
    public float smoothness = 5f;       // Camera movement smoothing factor
    public float xLimit = 5f;           // Limit for camera movement in the x-axis

    private float currentXOffset;       // Current offset in the x-axis

    private void LateUpdate()
    {
        // Check if the player's x-position exceeds the limit
        if (playerTransform.position.x > xLimit)
        {
            currentXOffset = playerTransform.position.x - xLimit;
        }
        else if (playerTransform.position.x < -xLimit)
        {
            currentXOffset = playerTransform.position.x + xLimit;
        }
        else
        {
            currentXOffset = 0f;
        }

        // Calculate the target x-axis position based on the player's position and the current offset
        float targetXPosition = playerTransform.position.x + currentXOffset;

        // Calculate the desired camera position
        Vector3 desiredPosition = new Vector3(targetXPosition, playerTransform.position.y, playerTransform.position.z) + cameraOffset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothness);
    }
}
