using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using Cinemachine;
public class BallMovement : MonoBehaviour
{
    public VariableJoystick joystick;
    public float moveSpeed,normalSpeed,increasedSpeed,decreasedSpeed,torque;
    public float controlSpeed;   
    public bool allowToMove;
    public CinemachineVirtualCamera lockCamera;
    [HideInInspector]public Rigidbody ballRB;
    
    // Start is called before the first frame update
    void Start()
    {
        ballRB = GetComponent<Rigidbody>();
        allowToMove = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveBallForward();
        //MoveBallLR();
    }

    // Constantly Roll the Ball forward
    public void MoveBallForward() {
        if (allowToMove) {           

            float moveForward = joystick.Vertical;
            float moveX = joystick.Horizontal;
                //.GetAxis("Horizontal");
           
           if (moveForward > 0)
            {
                moveSpeed = Mathf.Lerp(normalSpeed, increasedSpeed, 0.1f/Time.deltaTime);
               
            }
            else if (moveForward < 0)
            {
                moveSpeed = Mathf.Lerp(normalSpeed, decreasedSpeed, 0.1f / Time.deltaTime);
            }
            else {
                moveSpeed = Mathf.Lerp(moveSpeed, normalSpeed, 0.1f / Time.deltaTime);
            }
             // ballRB.MovePosition(transform.position+(Vector3.forward*moveSpeed*Time.deltaTime));
            ballRB.AddForce(Vector3.forward * moveSpeed, ForceMode.Acceleration);
            ballRB.AddForce(Vector3.right * moveX * torque,ForceMode.Force);
            //ballRB.AddForce(new Vector3(moveX,0,1) *moveSpeed, ForceMode.Acceleration);//Vector3.forward * moveSpeed, ForceMode.Acceleration);
            float singleStep = controlSpeed * Time.deltaTime;
            transform.Translate(Vector3.right * moveX * singleStep);
          
        
            // Draw a ray pointing at our target in
          

            //ballRB.AddTorque(Vector3.forward*moveX*torque,ForceMode.Force);

        }
    }
    Touch touch;
    bool LR;
    
        /* moveSpeed = decreasedSpeed;
            float moveLR = Input.GetAxis("Horizontal");
            if (moveLR > 0)
            {
                moveSpeed = decreasedSpeed;
                ballRB.AddForce(Vector3.right * controlSpeed, ForceMode.Acceleration);
            }
            else if (moveLR < 0)
            {
                moveSpeed = decreasedSpeed;
                ballRB.AddForce(-Vector3.right * controlSpeed, ForceMode.Acceleration); ;
            }*/
        /*
        if (Input.touchCount > 0)
         {
           

             if (touch.phase == TouchPhase.Moved)
             {

                 float turnLR = CnInputManager.GetAxis("Horizontal");
                 if (turnLR > 0)
                 {
                     ballRB.velocity = Vector3.right * controlSpeed;

                 }
                 if (turnLR < 0)
                 {
                     ballRB.velocity = -Vector3.right * controlSpeed;
                 }
                 if (touch.phase == TouchPhase.Ended)
                 {
                     ballRB.velocity = Vector3.forward;
                    
                 }
             }
         }*/
        //   #if UNITY_EDITOR


        /*if (Input.touchCount > 0)
        {
            //LR = true;
            float turnLR = CnInputManager.GetAxis("Horizontal");
            if (turnLR > 0)
            {
                ballRB.velocity = new Vector3(1 * controlSpeed, 0, ballRB.velocity.z);
               
                //ballRB.AddForce(Vector3.right * controlSpeed, ForceMode.Force);
            }
            else if (turnLR < 0)
            {
               // LR = true;
                ballRB.velocity = new Vector3(-1 * controlSpeed, 0, ballRB.velocity.z );
                //ballRB.AddForce(-Vector3.right * controlSpeed, ForceMode.Force);
            }
            else
            {
                // LR = false;
                ballRB.velocity = Vector3.zero;
            }
        }
        else*/

    }
   
