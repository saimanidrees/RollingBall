using System;
using System.Collections;
using GameAnalyticsSDK;
using GameData.MyScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PopUpUI : MonoBehaviour
{
    [SerializeField] private new string tag;
    [SerializeField] private UnityEvent onEnableEvent;
    private void OnEnable()
    {
        OnEnableEventFunction();
        switch (tag)
        {
            case PlayerPrefsHandler.LevelComplete:
                if(SoundController.Instance)
                    SoundController.Instance.PlayGameCompleteSound();
                break;
            case PlayerPrefsHandler.RevivePopup:
                if(SoundController.Instance)
                    SoundController.Instance.PlayGameOverSound();
                Callbacks.OnRewardRevive += DisableRevivePopup;
                StartCoroutine(CountDown(5));
                break;
            case PlayerPrefsHandler.BreakTheWallPopup:
                Callbacks.OnRewardRewardDownGradeWall += GiveWallDownGradeReward;
                break;
        }
    }
    private void OnDisable()
    {
        switch (tag)
        {
            case PlayerPrefsHandler.RevivePopup:
                Callbacks.OnRewardRevive -= DisableRevivePopup;
                break;
            case PlayerPrefsHandler.BreakTheWallPopup:
                Callbacks.OnRewardRewardDownGradeWall -= GiveWallDownGradeReward;
                break;
        }
    }
    private void OnEnableEventFunction()
    {
        onEnableEvent?.Invoke();
    }
    public void WatchVideoToRevive()
    {
        Callbacks.ADType = "Revive";
        AdsCaller.Instance.ShowRewardedAd();
    }
    public void WatchVideoToRemoveAds()
    {
        Callbacks.ADType = "RemoveAds";
        AdsCaller.Instance.ShowRewardedAd();
    }
    public void WatchVideoToDownGradeWall()
    {
        if (AdsCaller.Instance.IsRewardedAdAvailable())
        {
            Callbacks.ADType = "DownGradeWall";
            AdsCaller.Instance.ShowRewardedAd();
        }
        else
        {
            AdsCaller.Instance.ShowInterstitialAd();
            GamePlayManager.Instance.DownGradeWall();
            GiveWallDownGradeReward();
            GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, "Max", "InterstitialReward_DownGradeWall");
            FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + GAAdType.RewardedVideo + "_MAX_InterstitialReward_DownGradeWall");
        }
    }
    private IEnumerator CountDown(int time)
    {
        var countdownText = transform.Find("Counter/FillerBg/Filler/Text").GetComponent<Text>();
        while (time > 0)
        {
            countdownText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        yield return null;
        GameManager.Instance.StartMode();
        gameObject.SetActive(false);
    }
    public void NoThanks()
    {
        SoundController.Instance.PlayBtnClickSound();
        gameObject.SetActive(false);
        GameManager.Instance.StartMode();
    }
    private void DisableRevivePopup()
    {
        gameObject.SetActive(false);   
    }
    private void GiveWallDownGradeReward()
    {
        GamePlayManager.Instance.GetGamePlayUIManager().HideAllSubMenus();
    }
}