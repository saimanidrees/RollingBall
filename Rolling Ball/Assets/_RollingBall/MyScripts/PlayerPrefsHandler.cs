using UnityEngine;
namespace _RollingBall.MyScripts
{
    public static class PlayerPrefsHandler
    {
        #region Scenes

        public const string SplashScene = "MySplash", GamePlayScene = "GamePlay";

        #endregion
    
        #region Menus
    
        public const string HUD = "HUD";
        public const string MainMenu = "MainMenu";
        public const string Loading = "Loading";
        public const string LevelComplete = "LevelComplete";
        public const string LevelFail = "LevelFail";
        public const string Settings = "Settings";
        public const string NoVideo = "NoVideo";
        public const string Unlock = "Unlock";
        public const string Reward = "Reward";
        public const string CurrencyCounter = "CurrencyCounter";
        public const string BallCustomization = "BallCustomization";
    
        #endregion
    
        #region Tags

        public const string BallTag = "Ball", PendulumTag = "Pendulum", CirclePlatformTag = "CirclePlatform", MovingPlatformTag = "MovingPlatform";
        public const string ReverseViewTriggerTag = "ReverseViewTrigger", NonReverseViewTriggerTag = "NonReverseViewTrigger";
        public const string TopViewTriggerTag = "TopViewTrigger", NonTopViewTriggerTag = "NonTopViewTrigger", PendulumSingleSideTag = "PendulumSingleSide";
        public const string CollectableTag = "Collectable", NoneTag = "None", NormalDragTag = "NormalDrag", TopViewTrigger2Tag = "TopViewTrigger2";
        public const string CameraViewTriggerTag = "CameraViewTrigger", OppositeGravityTriggerTag = "OppositeGravity";
        public const string LevelCompleteTriggerTag = "LevelCompleteTrigger", ActivateReCenteringTag = "ActivateCameraReCentering";
        public const string PropTag = "Prop";
        
        #endregion
    
        #region Buttons
    
        public const string SwipeToPlay = "SwipeToPlay";
        public const string NextComplete = "NextComplete";
        public const string Replay = "Replay", Home = "Home";
        public const string SettingsClose = "SettingsClose";
    
        #endregion
    
        #region Settings Constants
    
        public const string Sound = "Sound";
        public const string Music = "Music";
        public const string Vibration = "Vibration";
    
        #endregion
    
        public const int TotalLevels = 20;
        private const string CurrentLevelString = "rollingBallCurrentLevel";
        private const string LevelsCounterString = "rollingBallLevelCounter";
        private const string BallSkinNoString = "rollingBallSkinNo";
        public static int CurrentLevelNo
        {
            get => PlayerPrefs.GetInt(CurrentLevelString, 0);
            set => PlayerPrefs.SetInt(CurrentLevelString, value);
        }
        public static int LevelsCounter
        {
            get => PlayerPrefs.GetInt(LevelsCounterString, 1);
            set => PlayerPrefs.SetInt(LevelsCounterString, value);
        }
        public static int BallSkinNo
        {
            get => PlayerPrefs.GetInt(BallSkinNoString, 0);
            set => PlayerPrefs.SetInt(BallSkinNoString, value);
        }
        public static void SetSoundControllerBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value == false ? 0 : 1);
        }
        public static bool GetSoundControllerBool(string key)
        {
            var value = PlayerPrefs.GetInt(key, 1);
            return value != 0;
        }
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value == false ? 0 : 1);
        }

        public static bool GetBool(string key)
        {
            var value = PlayerPrefs.GetInt(key, 0);
            return value != 0;
        }
    }
}