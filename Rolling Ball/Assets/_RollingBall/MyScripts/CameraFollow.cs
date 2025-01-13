using UnityEngine;

namespace _RollingBall.MyScripts
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform player;
        public Transform pivot;
        public Transform cameraTransform;
        public float followSpeed = 10f;
        public float rotationSpeed = 5f;
        private Vector3 _lastPlayerPosition;

        private void Start()
        {
            _lastPlayerPosition = player.position;
        }

        private void FixedUpdate()
        {
            var targetPosition = player.position;
            var position = transform.position;
            targetPosition.y = position.y;
            position = Vector3.Lerp(position, targetPosition, followSpeed * Time.deltaTime);
            transform.position = position;
            AdjustPivotRotation();
            cameraTransform.LookAt(player);
        }

        private void AdjustPivotRotation()
        {
            var movementDirection = player.position - _lastPlayerPosition;
            if (movementDirection.magnitude > 0.1f)
            {
                var horizontalVelocity = new Vector3(movementDirection.x, 0f, movementDirection.z).normalized;
                if (Physics.Raycast(player.position + Vector3.up * 0.5f, Vector3.down, out var hit, 2f))
                {
                    var platformNormal = hit.normal;
                    var platformForward = Vector3.Cross(platformNormal, Vector3.right).normalized;
                    var targetRotationY = Mathf.Atan2(horizontalVelocity.x, horizontalVelocity.z) * Mathf.Rad2Deg;
                    var smoothRotationY = Mathf.LerpAngle(pivot.eulerAngles.y, targetRotationY,
                        rotationSpeed * Time.deltaTime);
                    pivot.rotation = Quaternion.Euler(0f, smoothRotationY, 0f);
                }
            }

            _lastPlayerPosition = player.position;
        }
    }
}