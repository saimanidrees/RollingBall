using GameAnalyticsSDK;
using GameData.MyScripts;
using UnityEngine;
public class BallSkinPoint : MonoBehaviour
{
    private BallsSkinsHandler _ballsSkinsHandler;
    private int _skinNo = 0;
    [SerializeField] private Transform ballSkins;
    private void OnEnable()
    {
        _ballsSkinsHandler = GamePlayManager.Instance.currentPlayer.GetComponent<BallsSkinsHandler>();
        SelectSkinNo();
    }
    private void SelectSkinNo()
    {
        while (true)
        {
            var no = _ballsSkinsHandler.GetSkinIndex();
            if (no == -1)
            {
                _skinNo = Random.Range(0, 9);
            }
            else
            {
                var r = Random.Range(0, 9);
                if (r == no) continue;
                _skinNo = r;
            }
            ApplySkin();
            break;
        }
    }
    public void WatchVideoForBallSkin()
    {
        GamePlayManager.Instance.currentPlayer.GetComponent<BallsSkinsHandler>().SetSkinIndex(_skinNo);
        if (AdsCaller.Instance.IsRewardedAdAvailable())
        {
            Callbacks.ADType = "BallSkin";
            AdsCaller.Instance.ShowRewardedAd();
        }
        else
        {
            AdsCaller.Instance.ShowInterstitialAd();
            GamePlayManager.Instance.RewardSkin();
            GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, "Max", "InterstitialReward_BallSkin_" + 
                GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode));
            FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + GAAdType.RewardedVideo + "_MAX_InterstitialReward_BallSkin_" + 
                                                 GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode));
        }
    }
    private void ApplySkin()
    {
        Debug.Log("ApplySkin: " + _skinNo);
        for (var i = 0; i < ballSkins.childCount; i++)
        {
            ballSkins.GetChild(i).gameObject.SetActive(false);
        }
        ballSkins.GetChild(_skinNo).gameObject.SetActive(true);
    }
}