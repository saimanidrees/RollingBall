using System.Collections;
using Dreamteck.Splines;
using GameData.MyScripts;
using UnityEngine;
using UnityEngine.Events;
public class LevelBasedParams : MonoBehaviour
{
    #region Properties
    
    private GamePlayManager _gamePlayManager;
    [SerializeField] private Transform endingScenesPoints;
    private Transform _revivePoint;
    [SerializeField] private int ballUpgradeValue = 16;
    [SerializeField] private GameObject magnetPowerUpPoint, shieldPowerUpPoint, ballSkinPoint;
    [SerializeField] private bool _movingUpwards = false;
    public UnityEvent onLevelStart;
    private float _cameraDistance, _cameraHeight, _cameraHeightRatio;
    
    #endregion

    #region Methods
    
    public void SetGamePlayManager()
    {
        _gamePlayManager = GamePlayManager.Instance;
    }
    public void EnableHud(bool flag)
    {
        _gamePlayManager.GetGamePlayUIManager().controls.EnableHud(flag);
    }
    public void LevelComplete(float delay)
    {
        _gamePlayManager.GameComplete(delay);
    }
    public void ShowInterstitialAd()
    {
        AdsCaller.Instance.ShowInterstitialAd();
    }
    public void EnableObject(GameObject ob)
    {
        StartCoroutine(DelayToEnableObject(ob));
    }
    private static IEnumerator DelayToEnableObject(GameObject ob)
    {
        yield return new WaitForSeconds(1f);
        ob.SetActive(true);
    }
    private void EnableObjectAfterOneSec(GameObject ob)
    {
        ob.SetActive(true);
    }
    public Transform GetEndingScenePoint(int index)
    {
        return endingScenesPoints.GetChild(index);
    }
    public void SetRevivePoint(Transform point)
    {
        _revivePoint = point;
    }
    public Transform GetRevivePoint()
    {
        return _revivePoint;
    }
    public int GetBallUpgradeValue()
    {
        return ballUpgradeValue;
    }
    public void DisablePowerUpPoint(string pointName)
    {
        switch (pointName)
        {
            case "Magnet":
                if(magnetPowerUpPoint)
                    magnetPowerUpPoint.SetActive(false);
                break;
            case "Shield":
                if(shieldPowerUpPoint)
                    shieldPowerUpPoint.SetActive(false);
                break;
            case "BallSkin":
                if(ballSkinPoint)
                    ballSkinPoint.SetActive(false);
                break;
        }
    }
    public void SetPlayerForwardTransform(Transform forwardTransform)
    {
        GamePlayManager.Instance.currentPlayer.GetComponent<BallController>().SetForwardTransform(forwardTransform);
    }
    public void SetCameraDistance(float newDistance)
    {
        _cameraDistance = newDistance;
    }
    public void SetCameraHeight(float newHeight)
    {
        _cameraHeight = newHeight;
    }
    public void SetCameraHeightRatio(float newHeightRatio)
    {
        _cameraHeightRatio = newHeightRatio;
        StartCoroutine(DelayForCameraView());
    }
    private IEnumerator DelayForCameraView()
    {
        var cam = GamePlayManager.Instance.playerCamera.GetComponent<PerfectCameraController>();
        var time = 0f;
        const float DURATION = 30f;
        while (time < DURATION)
        {
            time += Time.deltaTime / DURATION;
            cam.smoothFollowSettings.distance = Mathf.Lerp(cam.smoothFollowSettings.distance, _cameraDistance, time);
            cam.smoothFollowSettings.height = Mathf.Lerp(cam.smoothFollowSettings.height, _cameraHeight, time);
            cam.smoothFollowSettings.viewHeightRatio = Mathf.Lerp(cam.smoothFollowSettings.viewHeightRatio, _cameraHeightRatio, time);
            yield return null;
        }
    }
    public bool IsMovingUpwards()
    {
        return _movingUpwards;
    }
    public void ChangeCameraRotation(float yAxis)
    {
        //GamePlayManager.Instance.currentPlayer.GetComponent<PlayerController>().RotateCamera(yAxis);
    }

    #endregion
}