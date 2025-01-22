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
        [ReadOnly] public LevelProgressTracker currentLevel;
        private const string LevelsPath = "Level";
        public static GamePlayManager Instance;
        //[SerializeField] private CameraViewController cameraViewController;
        private const int TotalLives = 5;
        private int _liveCount = 0;
        [SerializeField] private CameraController cameraController;
        public VibrationManager vibrationManager;
        private Vector3 _ballPositionForRevive;
        private Quaternion _ballRotationForRevive;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            SoundController.Instance.PlayRollingBallBgMusic();
            AdsCaller.Instance.ShowBanner();
            if(GameData.MyScripts.PlayerPrefsHandler.InterType == AdsCaller.InterType.Timer.ToString())
                AdsCaller.Instance.StartAdTimer();
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
            var level = Instantiate((GameObject)Resources.Load(path));
            currentLevel = level.GetComponent<LevelProgressTracker>();
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
            vibrationManager.TapVibrate();
            SoundController.Instance.PlayRollingBallWinSound();
            uiManager.SwitchMenu(PlayerPrefsHandler.LevelComplete);
            yield return new WaitForSeconds(delay);
            if (gameOverFlag)
            {
                SetPreviousLevel();
                yield break;
            }
            ShowLevelCompleteAd();
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
            cameraController.PauseTheFollowing(true);
            yield return new WaitForSeconds(delay);
            SoundController.Instance.PlayGameCompleteSound();
            uiManager.SwitchMenu(PlayerPrefsHandler.LevelFail);
        }
        private void SetNextLevel()
        {
            if (PlayerPrefsHandler.CurrentLevelNo < PlayerPrefsHandler.TotalLevels - 1)
                PlayerPrefsHandler.CurrentLevelNo += 1;
            else
                PlayerPrefsHandler.CurrentLevelNo = 0;
            PlayerPrefsHandler.LevelsCounter++;
        }
        private void SetPreviousLevel()
        {
            if (PlayerPrefsHandler.CurrentLevelNo > 0)
                PlayerPrefsHandler.CurrentLevelNo -= 1;
            else
                PlayerPrefsHandler.CurrentLevelNo = PlayerPrefsHandler.TotalLevels - 1;
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
            return _liveCount < TotalLives;
        }
        public CameraController GetCameraController()
        {
            return cameraController;
        }
        public void RewardRefillBalls()
        {
            _liveCount = 0;
            uiManager.GetBallLivesUI().Refill();
            uiManager.SwitchMenu(PlayerPrefsHandler.HUD);
            ball.GetComponent<Rigidbody>().isKinematic = true;
            var transform1 = ball.transform;
            transform1.position = _ballPositionForRevive;
            transform1.rotation = _ballRotationForRevive;
            ball.GetComponent<Rigidbody>().isKinematic = false;
            cameraController.PauseTheFollowing(false);
            cameraController.PauseTheAlignment(true);
        }
        public void SetPositionRotationForRevive(Vector3 newPos, Quaternion newRot)
        {
            _ballPositionForRevive = newPos;
            _ballRotationForRevive = newRot;
        }
        public void ShowLevelCompleteAd()
        {
            if(GameData.MyScripts.PlayerPrefsHandler.InterType == AdsCaller.InterType.Timer.ToString())
                AdsCaller.Instance.ShowTimerAd();
            else
                AdsCaller.Instance.ShowInterstitialAd();
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