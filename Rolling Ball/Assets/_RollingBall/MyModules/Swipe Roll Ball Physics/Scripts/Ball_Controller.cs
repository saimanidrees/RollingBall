using UnityEngine;

public class Ball_Controller : MonoBehaviour
{
    private Rigidbody Ball_Rb;
    [SerializeField]
    private Local_Data Local_Data;
    [SerializeField]
    private Transform Camera_Transform, Ball_Clone_Transform;

    private Transform Ball_Original_Transform;
    [SerializeField]
    private float Push_Force;

    private float Applied_Push_Force=0;

    private Vector3 Camera_No_Y_Pos = new Vector3(0 ,0 , 0), Ball_No_Y_Pos = new Vector3(0, 0, 0),Normal_Direction = new Vector3(0, 0, 0);
    private Vector3 Force_Direction = new Vector3(0 , 0, 0);
 
    // Start is called before the first frame update
    void Start()
    {
        Ball_Original_Transform = GetComponent<Transform>();
        Ball_Rb = GetComponent<Rigidbody>();
        Ball_Rb.maxAngularVelocity = Mathf.Infinity;
        Application.targetFrameRate = 200;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Set Ball Clone to Ball Position
           Ball_Clone_Transform.position = Ball_Original_Transform.position;

        if (Local_Data.Relative_Angle != 0)
        {
           
            // Find Applied push force
            if (Local_Data.Relative_Angle > -30 && Local_Data.Relative_Angle < 30)
            {
              Applied_Push_Force = Push_Force * 2;       
            }
            else
            {            
               Applied_Push_Force = Push_Force;
            }
            // No Y Positions
            Camera_No_Y_Pos = new Vector3(Camera_Transform.position.x, 0, Camera_Transform.position.z);
            Ball_No_Y_Pos = new Vector3(Ball_Clone_Transform.position.x, 0, Ball_Clone_Transform.position.z);

            //Find Normal Direction
            Normal_Direction = Camera_No_Y_Pos - Ball_No_Y_Pos;
            Normal_Direction = Normal_Direction.normalized;

            //Find Force Direction
            Force_Direction = Quaternion.AngleAxis(Local_Data.Relative_Angle, -Ball_Clone_Transform.up) * Normal_Direction * 2; // - sign is needed         

            //Reset Relative angle
            Local_Data.Relative_Angle = 0;

            //Add Force to ball
            Ball_Rb.AddForce(Force_Direction * Applied_Push_Force, ForceMode.Force);

        }
    }


    public void reset_ball()
    {
        Ball_Original_Transform.position = new Vector3(2,0,3);
        Ball_Rb.velocity = new Vector3(0,0,0);
    }
}
