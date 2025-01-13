using UnityEngine;
public class BezierCurveCar : MonoBehaviour
{    
    [SerializeField] private LineRenderer lineRenderer;
    [Space]
    public Transform position1, position2, position3, pos2Original, pos3Original;
    [Space]
    public Vector3 newPos;
    [Space]
    public float curveLenght3, curveLenght2;
    [Space]
    public int positionCounts;
    [Space]
    [SerializeField] private int speed;
    [Space]
    [SerializeField] private int smoothTime;
    [Space]
    [SerializeField] private int x = -90;
    private float steeringInput;
    private Vector3 velocity;
    //private RCC_Settings.MobileController _mobileController;
    private void LateUpdate()
    {
        DrawCubicBezierCurve(position1.position, position2.position, position3.position);
        CheckPos();
    }
    private void DrawCubicBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        lineRenderer.positionCount = positionCounts;
        var t = 0f;
        for (var i = 0; i < lineRenderer.positionCount; i++)
        {
            var b = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;

            lineRenderer.SetPosition(i, b);

            t += (1 / (float)lineRenderer.positionCount);
        }
    }
    private void CheckPos()
    {
        /*if (RCC_SceneManager.Instance.activePlayerVehicle != null) steeringInput = RCC_SceneManager.Instance.activePlayerVehicle.steerInput;
        if (GamePlayManager.Instance.currentPlayer.carController.direction == 1)
        {
            SoundController.Instance.StopReverseSound();
            if (steeringInput == 0)
            {                
                newPos = pos3Original.localPosition;
                position2.localPosition = pos2Original.localPosition;
            }
            else
            {
                newPos = new Vector3(pos3Original.localPosition.x, pos3Original.localPosition.y + (steeringInput * curveLenght3), pos3Original.localPosition.z);
                position2.localPosition = new Vector3(pos2Original.localPosition.x + (steeringInput * curveLenght2), pos2Original.localPosition.y, pos2Original.localPosition.z);
            }
            ChangePos();
        }
        else if (GamePlayManager.Instance.currentPlayer.carController.direction != 1)
        {
            SoundController.Instance.PlayReverseSound();
            if (steeringInput == 0)
            {
                newPos = new Vector3(pos3Original.localPosition.x, pos3Original.localPosition.y, -pos3Original.localPosition.z);
                position2.localPosition = new Vector3(pos2Original.localPosition.x, pos2Original.localPosition.y, -pos2Original.localPosition.z);
            }
            else
            {
                newPos = new Vector3(pos3Original.localPosition.x, pos3Original.localPosition.y + (steeringInput * curveLenght3), -pos3Original.localPosition.z);
                position2.localPosition = new Vector3(pos2Original.localPosition.x + (steeringInput * curveLenght2), pos2Original.localPosition.y, pos2Original.localPosition.z * (-1));
            }
            ChangePos();
        }*/
    }
    private void ChangePos()
    {
        position3.localPosition = Vector3.SmoothDamp(position3.localPosition, newPos, ref velocity, Time.deltaTime * smoothTime, speed);
    }
}