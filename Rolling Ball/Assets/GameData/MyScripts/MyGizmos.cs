using System;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    [SerializeField] private bool drawCube = false;
    [SerializeField] private Vector3 cubeSize = new Vector3(0.05f, 0.05f, 0.05f);
    [SerializeField] private bool drawSphere = false;
    [SerializeField] private float sphereRadius = 0.05f;
    [Space]
    [SerializeField] private Color gizmosColor = Color.gray;
    /*private void OnDrawGizmosSelected() {
        Gizmos.color = gizmosColor;
        if(drawCube)
            Gizmos.DrawCube(transform.position, cubeSize);
        if(drawSphere)
            Gizmos.DrawSphere(transform.position, sphereRadius);
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        if(drawCube)
            Gizmos.DrawCube(transform.position, cubeSize);
        if(drawSphere)
            Gizmos.DrawSphere(transform.position, sphereRadius);
    }
}