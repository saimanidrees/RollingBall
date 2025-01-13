using GameAnalyticsSDK;
using GameData.MyScripts;
using UnityEngine;
public class Callbacks : MonoBehaviour {
    public delegate void RewardRevive();
    public static event RewardRevive OnRewardRevive;
    public delegate void RewardUpgradeBall();
    public static event RewardUpgradeBall OnRewardUpgradeBall;
    public delegate void RewardShield();
    public static event RewardShield OnRewardShield;
    public delegate void RewardMagnet();
    public static event RewardMagnet OnRewardMagnet;
    public delegate void RewardBallSkin();
    public static event RewardBallSkin OnRewardBallSkin;
    public delegate void RewardDownGradeWall();
    public static event RewardDownGradeWall OnRewardRewardDownGradeWall;
    public delegate void RewardMiniWheel();
    public static event RewardMiniWheel OnRewardMiniWheel;
    public delegate void RewardRollingBallSkin();
    public static event RewardRollingBallSkin OnRewardRollingBallSkin;
    public delegate void RewardSkipLevel();
    public static event RewardSkipLevel OnRewardSkipLevel;
    public static string ADType;
    private const string MaxString = "Max", VideoRewardString = "VideoReward_", CardRewardString = "CardReward", ReviveString = "Revive", 
        UpgradeBallString = "UpgradeBall", ShieldString = "Shield", MagnetString = "Magnet", BallSkinString = "BallSkin", 
        DownGradeWallString = "DownGradeWall", MiniWheelString = "MiniWheel", UnderscoreString = "_";
    public const string RollingBallSkinRewardString = "RollingBallSkinReward", SkipLevelString = "SkipLevel";
    private void Start () 
    {
		DontDestroyOnLoad (gameObject);
	}
    public static void RewardedAdWatched ()
	{
        switch (ADType)
        {
            case CardRewardString:
                FindObjectOfType<CardReward>().Reward();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + CardRewardString);
                break;
            case ReviveString:
                OnRewardRevive?.Invoke();
                GamePlayManager.Instance.RevivePlayer();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + ReviveString);
                break;
            case UpgradeBallString:
                OnRewardUpgradeBall?.Invoke();
                GamePlayManager.Instance.UpgradeBall();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + UpgradeBallString);
                break;
            case ShieldString:
                OnRewardShield?.Invoke();
                GamePlayManager.Instance.RewardShield();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + ShieldString);
                break;
            case MagnetString:
                OnRewardMagnet?.Invoke();
                GamePlayManager.Instance.RewardMagnet();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + MagnetString + 
                    UnderscoreString + GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode));
                break;
            case BallSkinString:
                OnRewardBallSkin?.Invoke();
                GamePlayManager.Instance.RewardSkin();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + BallSkinString + 
                    UnderscoreString + GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode));
                break;
            case DownGradeWallString:
                OnRewardRewardDownGradeWall?.Invoke();
                GamePlayManager.Instance.DownGradeWall();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + DownGradeWallString);
                break;
            case MiniWheelString:
                OnRewardMiniWheel?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + MiniWheelString);
                break;
            case RollingBallSkinRewardString:
                OnRewardRollingBallSkin?.Invoke();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + RollingBallSkinRewardString);
                break;
            case SkipLevelString:
                OnRewardSkipLevel?.Invoke();
                _RollingBall.MyScripts.GamePlayManager.Instance.RewardNextLevel();
                GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, MaxString, VideoRewardString + SkipLevelString);
                break;
        }
    }
}