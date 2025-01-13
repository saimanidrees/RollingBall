using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.25f;
    public Vector3 velocity = Vector3.zero;
    public bool isFollowing = false;
    public Transform target;
    private Vector3 targetPosition;
    // private void Update()
    public void LateUpdate()
    {


        if (!isFollowing)
        {
            return;
        }
        targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

}