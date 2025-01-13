using UnityEngine;


public class Touch_Input : MonoBehaviour
{
    [SerializeField]
    private Local_Data Local_Data;
    [SerializeField]
    private Transform Ball_Trans;

    private Touch touch;

    private Vector2 touch_Start_Position = new Vector2(0,0);
    private Vector2 touch_End_Position = new Vector2(0,0);
    private Vector2 touch_Normal_Position = new Vector2(0,0);

    private Vector2 Touch_Start_To_Normal_Direction= new Vector2(0, 0);
    private Vector2 Touch_Start_To_End_Direction = new Vector2(0, 0);

    private bool bool_isTouched = false;
    private bool Touchphase_hasBegan = false;
    private bool Touchphase_hasEnded = false;

    private float Angle_Between_Normal_And_Touch_Direction = 0;


    private void Start()
    {
        Ball_Trans = transform;
    }



    private void Update()
    {
        //// swipe calculation///////
        F_Detect_Touch();
        F_Detect_Touch_Phase();
        F_Save_Touch_Begin_Position();
        F_Save_Touch_End_Position();
        F_Find_Angle();  
    }

    private void F_Detect_Touch()
    {
        if (Input.touchCount>0)
        {
            bool_isTouched = true;
            touch = Input.GetTouch(0);         
        }
        else
        {
            bool_isTouched = false;
        }       
    }

    private void F_Detect_Touch_Phase()
    {
        if (bool_isTouched)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Touchphase_hasBegan = true;
                Touchphase_hasEnded = false;           
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                Touchphase_hasBegan = false;
                Touchphase_hasEnded = true;     
            }
            else
            {
                Touchphase_hasBegan = false;
                Touchphase_hasEnded = false;              
            }
        }
        else
        {
            Touchphase_hasBegan = false;
            Touchphase_hasEnded = false;
        }
    }

    private void F_Save_Touch_Begin_Position()
    {
        if (Touchphase_hasBegan)
        {
            touch_Start_Position = touch.position;             
        }
    }
    private void F_Save_Touch_End_Position()
    {
        if (Touchphase_hasEnded)
        {
            touch_End_Position = touch.position;      
        }
    }

    private void F_Find_Angle()
    {       
        if (Touchphase_hasEnded)
        {
            // Find normal position
            touch_Normal_Position = new Vector2(touch_Start_Position.x,0);
            // Find Start to normal direction
            Touch_Start_To_Normal_Direction = touch_Normal_Position - touch_Start_Position;

            // Find touch Direction
            Touch_Start_To_End_Direction = touch_End_Position - touch_Start_Position;

            // Find Angle
            Angle_Between_Normal_And_Touch_Direction = Vector2.SignedAngle(Touch_Start_To_Normal_Direction, Touch_Start_To_End_Direction);
            Local_Data.Relative_Angle = Angle_Between_Normal_And_Touch_Direction;         
        }        
    }

 
   


    

}
