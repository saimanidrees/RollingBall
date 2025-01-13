using UnityEngine;

public class RotateRigidbodies : MonoBehaviour
{
    [System.Serializable]
    public struct RigidbodyRotation
    {
        public Rigidbody rigidbody; // The Rigidbody to rotate
        public bool clockwise;      // Rotation direction (true for clockwise)
    }

    public RigidbodyRotation[] rigidbodiesToRotate; // Array of Rigidbodies and their directions
    public Vector3 rotationAxis = Vector3.up;       // Axis to rotate around (default is Y-axis)
    public float rotationSpeed = 10f;               // Speed of rotation

    void Start()
    {
        // Normalize the rotation axis to ensure consistent behavior
        rotationAxis = rotationAxis.normalized;
    }

    void FixedUpdate()
    {
        foreach (var item in rigidbodiesToRotate)
        {
            if (item.rigidbody != null)
            {
                // Determine rotation direction based on the clockwise flag
                float direction = item.clockwise ? 1f : -1f;

                // Apply angular velocity to rotate the Rigidbody
                item.rigidbody.angularVelocity = rotationAxis * rotationSpeed * direction;
            }
        }
    }
}