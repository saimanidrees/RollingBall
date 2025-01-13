using Sirenix.OdinInspector;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class MyGizmos : MonoBehaviour
    {
        private enum GizmosShape
        {
            None,
            Cube,
            Sphere
        }
        [SerializeField] private GizmosShape gizmosShape;
        [ShowIf("@gizmosShape == GizmosShape.Cube")]
        [SerializeField] private Vector3 cubeSize = new Vector3(0.05f, 0.05f, 0.05f);
        [ShowIf("@gizmosShape == GizmosShape.Sphere")]
        [SerializeField] private float sphereRadius = 0.05f;
        [Space]
        [HideIf("@gizmosShape == GizmosShape.None")]
        [SerializeField] private Color gizmosColor = Color.gray;
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            if(gizmosShape == GizmosShape.Cube)
                Gizmos.DrawCube(transform.position, cubeSize);
            if(gizmosShape == GizmosShape.Sphere)
                Gizmos.DrawSphere(transform.position, sphereRadius);
        }
    }
}