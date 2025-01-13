using GameData.MyScripts;
using UnityEngine;
public class MarblesScene : MonoBehaviour
{
    [SerializeField] private PerfectCameraController cameraController;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Animator vehicleAnimator;
    [SerializeField] private GameObject confetti;
    [SerializeField] private GameObject light;
    private int _ballsCounter = 0;
    private const int TotalBalls = 20;
    private bool _isSceneEnded = false;
    public void SetBallsCounter()
    {
        if(_isSceneEnded) return;
        _ballsCounter++;
        if (_ballsCounter < TotalBalls) return;
        _isSceneEnded = true;
        confetti.SetActive(true);
        Invoke(nameof(InvokeVehicleAnimator), 2f);
    }
    public void SetPlayerBallOnTrack()
    {
        var ballComponent = GamePlayManager.Instance.currentPlayer.GetComponent<BallController>();
        ballComponent.startRun = true;
        ballComponent.horizontalMovement = false;
        //var ballTransform = ballComponent.GetBallAnimator().transform;
        var ballTransform = ballComponent.transform;
        var position = ballTransform.position;
        position = new Vector3(0, position.y, position.z);
        ballTransform.position = position;
        ballComponent.ResetCameraTarget();
    }
    public void StartScene()
    {
        SetCameraView();
        transform.Find("Balls&FireWorks").gameObject.SetActive(true);
        GamePlayManager.Instance.currentPlayer.SetActive(false);
        cameraController.target = cameraTarget;
        light.SetActive(true);
    }
    public void EndScene()
    {
        GamePlayManager.Instance.GameComplete(1f);
    }
    private static void SetCameraView()
    {
        var cam = GamePlayManager.Instance.playerCamera.GetComponent<PerfectCameraController>();
        //cam.smoothFollowSettings.distance = 13.5f;
        //cam.smoothFollowSettings.height = 8.7f;
        cam.smoothFollowSettings.viewHeightRatio = -0.5f;
    }
    private void InvokeVehicleAnimator()
    {
        vehicleAnimator.enabled = true;  
    }
}