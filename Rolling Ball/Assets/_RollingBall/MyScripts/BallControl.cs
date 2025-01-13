using System;
using Lean.Touch;
using UnityEngine;
public class BallControl : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    //[SerializeField] private float fingerSensitivity = 5;
    [SerializeField] private Rigidbody body;
    private LeanFinger _currentFinger;
    private bool _isGameOn = false;
    private Vector3 _delta;
    [SerializeField] private Camera cam;
    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
    }
    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }
    private void OnFingerDown(LeanFinger obj)
    {
        if (!_isGameOn)
        {
            _isGameOn = true;
        }

        if (_currentFinger != null) return;
        _currentFinger = obj;
    }

    private void OnFingerUp(LeanFinger obj)
    {
        if (_currentFinger != obj) return;
        _currentFinger = null;
    }

    private void FixedUpdate()
    {
        if (_currentFinger != null)
        {
            _delta = _currentFinger.GetWorldPosition(10, cam) - _currentFinger.GetStartWorldPosition(10, cam);
            _delta.y = 0;
            //_delta *= fingerSensitivity;
            var moveHorizontal = _delta.x;
            var moveVertical = _delta.z;
            var cameraTransform = cam.transform;
            var forward = cameraTransform.forward;
            var right = cameraTransform.right;
            var forwardMovement = moveVertical * forward;
            var sideMovement = moveHorizontal * right;
            var movement = forwardMovement + sideMovement;
            movement = new Vector3(movement.x, 0f, movement.z);
            //Debug.Log(movement);
            body.AddForce(movement * speed);
        }
    }
}