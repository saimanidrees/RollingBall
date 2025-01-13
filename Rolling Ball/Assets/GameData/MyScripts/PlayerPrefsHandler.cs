using System.Collections.Generic;
using UnityEngine;

namespace GameData.MyScripts
{
    public class PlayerPrefsHandler
    {
        #region Scenes
        public const string Splash = "MySplash";
        public const string GamePlay = "GamePlay";
        #endregion
        #region Menus
        public const string HUD = "HUD";
        public const string Loading = "Loading";
        public const string LevelComplete = "LevelComplete";
        public const string RevivePopup = "RevivePopup";
        public const string Settings = "Settings";
        public const string NoVideo = "NoVideo";
        public const string Reward = "Reward";
        public const string BreakTheWallPopup = "BreakTheWallPopup";
        public const string RemoveAdsPopup = "RemoveAdsPopup";
        public const string BallsSkinsSelection = "BallsSkinsSelection";
        public const string CurrencyCounter = "CurrencyCounter";
    
        #endregion
        #region Tags
        public const string Player = "Player";
        public const string Ball = "Ball";
        public const string Obstacle = "Obstacle";
        public const string Wall = "Wall";
        public const string Ground = "Ground";
        public const string SlopeDownward = "SlopeDownward";
        public const string SlopeUpward = "SlopeUpward";
        public const string LevelEndPoint = "LevelEndPoint";
        public const string LevelFailTrigger = "LevelFailTrigger";
        public const string RevivePointTrigger = "RevivePointTrigger";
        public const string WallTriggerToUpgrade = "WallTriggerToUpgrade";
        public const string Lifter = "Lifter";
        public const string Hole = "Hole";
        public const string EventTrigger = "EventTrigger";
        #endregion
    
        #region Modes
        public const string MergeBallMode = "MergeBall2048";
        public const string InfiniteMode = "InfiniteZone";
        public const string RollingBallMode = "RollingBall";
        #endregion

        public const int MergeBallsTotalLevels = 18;
        private const string CoinsString = "Coins";
        private const string CurrentModeString = "currentMode";
        private const string LevelsCounterString = "levelCounterString";
        private const string HighScoreString = "HighScore";

        #region Firebase

        public const string ControlExperimentString = "Control";
        public static string ControlType = "Old";
        public const string EnableFailOverString = "EnableFailOver";
        public static bool EnableFailOver = false;
        public const string MakeLevelsEasyString = "MakeLevelsEasy";
        public static bool MakeLevelsEasy = false;
        public static readonly string[] LevelsSequenceArray = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17"};

        #endregion

        public static readonly List<int> BallNumbers = new List<int> { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };
    
        private static readonly float[] BallSizes = { 1, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f};
    
        public static int Coins
        {
            get => PlayerPrefs.GetInt(CoinsString, 1000);
            set => PlayerPrefs.SetInt(CoinsString, value);
        }
        public static int CurrentMode
        {
            get => PlayerPrefs.GetInt(CurrentModeString, 0);
            set => PlayerPrefs.SetInt(CurrentModeString, value);
        }
        public static int LevelsCounter
        {
            get => PlayerPrefs.GetInt(LevelsCounterString, 0);
            set => PlayerPrefs.SetInt(LevelsCounterString, value);
        }
        public static int HighScore
        {
            get => PlayerPrefs.GetInt(HighScoreString, 0);
            set => PlayerPrefs.SetInt(HighScoreString, value);
        }
        public static void SetCurrentLevel(string modeName, int levelNo)
        {
            PlayerPrefs.SetInt(modeName, levelNo);
        }
        public static int GetCurrentLevel(string modeName)
        {
            return PlayerPrefs.GetInt(modeName, 0);
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
        public static float GetBallSize(int ballNumber)
        {
            var index = BallNumbers.IndexOf(ballNumber);
            return BallSizes[index];
        }
    }
}