using System.Collections;
using GameAnalyticsSDK;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class GamePlayManager : MonoBehaviour
    {
        [HideInInspector]
        public bool gameStartFlag = false, gamePauseFlag = false, gameOverFlag = false, gameCompleteFlag = false, gameContinueFlag = false;
        public GamePlayUIManager uiManager;
        public BallController ball;
        [ReadOnly] public GameObject currentLevel;
        private const string LevelsPath = "Level";
        public static GamePlayManager Instance;
        [SerializeField] private CameraViewController cameraViewController;
        private const int TotalLives = 5;
        private int _liveCount = 0;
        [SerializeField] private CameraController cameraController;
        public VibrationManager vibrationManager;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            SoundController.Instance.PlayRollingBallBgMusic();
            uiManager.OpenSpecialMenu(PlayerPrefsHandler.CurrencyCounter);
            CurrentLevelSettings();
        }
        public bool IsGameReadyToPlay()
        {
            return gameStartFlag == true && gamePauseFlag == false && gameOverFlag == false && gameCompleteFlag == false;
        }
        public bool IsLevelCompleted()
        {
            return gameCompleteFlag;
        }
        public void SetGameToPlay(int no)
        {
            gameStartFlag = false;
            gamePauseFlag = false;
            gameOverFlag = false;
            gameCompleteFlag = false;
            switch (no)
            {
                // game start
                case 0:
                    gameStartFlag = true;
                    break;
                // game pause
                case 1:
                    gamePauseFlag = true;
                    break;
                // game over
                case 2:
                    gameOverFlag = true;
                    break;
                // game complete or level complete
                default:
                    gameCompleteFlag = true;
                    break;
            }
        }
        private void CurrentLevelSettings()
        {
            var path = LevelsPath + PlayerPrefsHandler.CurrentLevelNo;
            currentLevel = Instantiate((GameObject)Resources.Load(path));
            Invoke(nameof(LevelStart), 0.1f);
        }
        private void LevelStart()
        {
            SetGameToPlay(0);
            SendProgressionEvent(GAProgressionStatus.Start);
            ball.AllowMovement(true);
        }
        public void LevelComplete(float delay)
        {
            if(gameCompleteFlag) return;
            SetGameToPlay(3);
            SendProgressionEvent(GAProgressionStatus.Complete);
            StartCoroutine(DelayForLevelComplete(delay));
        }
        private IEnumerator DelayForLevelComplete(float delay)
        {
            SetNextLevel();
            cameraController.SetViewForLevelEnd();
            GetCameraViewController().SetLevelCompleteView();
            vibrationManager.TapVibrate();
            yield return new WaitForSeconds(delay);
            if (gameOverFlag)
            {
                SetPreviousLevel();
                yield break;
            }
            SoundController.Instance.PlayRollingBallWinSound();
            uiManager.SwitchMenu(PlayerPrefsHandler.LevelComplete);
            const string modeName = GameData.MyScripts.PlayerPrefsHandler.RollingBallMode;
            GameData.MyScripts.PlayerPrefsHandler.CurrentMode = GameData.MyScripts.GameManager.Instance.GetModeNo(modeName);
            GameData.MyScripts.GameManager.Instance.StartMode(modeName);
        }
        public void LevelFail(float delay)
        {
            SetGameToPlay(2);
            SendProgressionEvent(GAProgressionStatus.Fail);
            StartCoroutine(DelayForLevelFail(delay));
        }
        private IEnumerator DelayForLevelFail(float delay)
        {
            yield return new WaitForSeconds(delay);
            SoundController.Instance.PlayGameCompleteSound();
            uiManager.SwitchMenu(PlayerPrefsHandler.LevelFail);
            const string modeName = GameData.MyScripts.PlayerPrefsHandler.RollingBallMode;
            GameData.MyScripts.PlayerPrefsHandler.CurrentMode = GameData.MyScripts.GameManager.Instance.GetModeNo(modeName);
            GameData.MyScripts.GameManager.Instance.StartMode(modeName);
        }
        private void SetNextLevel()
        {
            if (PlayerPrefsHandler.CurrentLevelNo < PlayerPrefsHandler.TotalLevels - 1)
                PlayerPrefsHandler.CurrentLevelNo += 1;
            else
                PlayerPrefsHandler.CurrentLevelNo = 0;
            PlayerPrefsHandler.LevelsCounter++;
            //Debug.Log("CurrentLevelNo: " + PlayerPrefsHandler.CurrentLevelNo);
            //Debug.Log("LevelsCounter: " + PlayerPrefsHandler.LevelsCounter);
        }
        private void SetPreviousLevel()
        {
            if (PlayerPrefsHandler.CurrentLevelNo > 0)
                PlayerPrefsHandler.CurrentLevelNo -= 1;
            else
                PlayerPrefsHandler.CurrentLevelNo = PlayerPrefsHandler.TotalLevels - 1;
        }
        public CameraViewController GetCameraViewController()
        {
            return cameraViewController;
        }
        public void SetReverseViewCamera(int priorityValue)
        {
            cameraViewController.SetReverseViewCamera(priorityValue);
        }
        public void SetTopViewCamera(int priorityValue)
        {
            cameraViewController.SetTopViewCamera(priorityValue);
        }
        public void SetTopViewCamera2(int priorityValue)
        {
            cameraViewController.SetTopViewCamera2(priorityValue);
        }
        public void SetReCentering(bool flag)
        {
            cameraViewController.SetReCentering(flag);
        }
        public void SetXAxisValue(float newValue)
        {
            cameraViewController.SetXAxisValue(newValue);
        }
        public void StartMode(string modeName)
        {
            GameData.MyScripts.PlayerPrefsHandler.CurrentMode = GameData.MyScripts.GameManager.Instance.GetModeNo(modeName);
            GameData.MyScripts.GameManager.Instance.StartMode(modeName);
        }
        public void RewardNextLevel()
        {
            SetNextLevel();
            StartMode(GameData.MyScripts.PlayerPrefsHandler.RollingBallMode);
        }
        public bool IsAlive()
        {
            uiManager.GetBallLivesUI().Die(_liveCount);
            _liveCount++;
            if (_liveCount >= TotalLives)
                return false;
            else
                return true;
        }
        public CameraController GetCameraController()
        {
            return cameraController;
        }
        private void SendProgressionEvent(GAProgressionStatus status)
        {
            var eventString1 = "Mode " +GameData.MyScripts.GameManager.Instance.GetModeName(GameData.MyScripts.PlayerPrefsHandler.CurrentMode) 
                                       + " Level " + (PlayerPrefsHandler.LevelsCounter + 1);
            GameAnalytics.NewProgressionEvent(status,  eventString1);
            var eventString2 = "Mode_" + GameData.MyScripts.GameManager.Instance.GetModeName(GameData.MyScripts.PlayerPrefsHandler.CurrentMode) 
                                       + "_Level_" + (PlayerPrefsHandler.LevelsCounter + 1);
            FirebaseManager.Instance.ReportEvent(status + eventString2);
            AppmetricaAnalytics.ReportCustomEvent(AnalyticsType.GameData, $"Mode_{GameData.MyScripts.PlayerPrefsHandler.CurrentMode + 1}", 
                $"Level_{PlayerPrefsHandler.LevelsCounter + 1}", status.ToString());
        }
    }
}