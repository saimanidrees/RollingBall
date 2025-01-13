using UnityEngine;

public class Local_Data : MonoBehaviour
{
    public bool New_Touch_Cycle = false;
    public Vector2 Touch_GetAxis_Raw = new Vector2(0,0);
    public Vector2 Touch_Swipe_Axis = new Vector2(0,0);
    public bool Touch_isTouched = false;
    public float Touch_Speed = 0;


    public float Relative_Angle = 0;
}
