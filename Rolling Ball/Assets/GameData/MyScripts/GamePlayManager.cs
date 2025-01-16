using System.Collections;
using GameAnalyticsSDK;
using UnityEngine;
namespace GameData.MyScripts
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] private GamePlayUIManager uiManager;
        [HideInInspector]
        public bool gameStartFlag = false, gamePauseFlag = false, gameOverFlag = false, gameCompleteFlag = false, gameContinueFlag = false;
        [ReadOnly] public LevelBasedParams currentLevel;
        public GameObject currentPlayer;
        public GameObject playerCamera;
        [SerializeField] private BallController ballController;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject oldCamera, newCamera;
        [SerializeField] private Transform playerPositions;
        [SerializeField] private GameObject levelFailTrigger;
        [SerializeField] private GameObject[] endingScenes;
        [HideInInspector] public bool[] isBallUnlocked = {false, false, false, false, false, false, false, false, false};
        [HideInInspector] public bool isLevelCompleteRewardGiven = false;
        [System.Serializable]
        public class LevelList
        {
            public string[] levels;
        }
        private LevelList _levelsList;
        public static GamePlayManager Instance;
        private void Awake()
        {
            Application.targetFrameRate = 120;
            Instance = this;
            SoundController.Instance.PlayBackgroundMusic();;
            FirstPlay();
        }
        private void FirstPlay()
        {
            if (PlayerPrefsHandler.GetBool("FirstPlay"))
                return;
            PlayerPrefsHandler.SetBool("FirstPlay", true);
        }
        private void Start()
        {
            AdsCaller.Instance.ShowBanner();
            //uiManager.HideAll();
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
        private bool IsLevelFailed()
        {
            return gameOverFlag;
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
        public GamePlayUIManager GetGamePlayUIManager()
        {
            return uiManager;
        }
        private void CurrentLevelSettings()
        {
            if (GameManager.Instance.IsInfiniteMode())
            {
                uiManager.SwitchMenu(PlayerPrefsHandler.HUD);
                SendProgressionEvent(GAProgressionStatus.Start);
                return;
            }
            if (PlayerPrefsHandler.ControlType == "Old")
            {
                currentPlayer = ballController.gameObject;
                playerCamera = oldCamera;
            }
            else
            {
                currentPlayer = playerController.gameObject;
                playerCamera = newCamera;
            }
            var modeName = GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode);
            var currentLevelIndex = PlayerPrefsHandler.GetCurrentLevel(modeName);
            var index = int.Parse(PlayerPrefsHandler.LevelsSequenceArray[currentLevelIndex]);
            var levels = Resources.Load("Levels") as Levels;
            if (levels != null)
            {
                var level = Instantiate(levels.levels[index]);
                //var level = Instantiate((GameObject)Resources.Load("Levels/"+ modeName + "/Level" + PlayerPrefs.GetInt(modeName)));
                currentLevel = level.GetComponent<LevelBasedParams>();
            }
            currentLevel.SetGamePlayManager();
            var posTransform = playerPositions.GetChild(index);
            var playerTempTransform = currentPlayer.transform;
            playerTempTransform.position = posTransform.position;
            playerTempTransform.rotation = posTransform.rotation;
            currentPlayer.SetActive(true);
            playerCamera.SetActive(true);
            if(GameManager.Instance.IsBallMergeMode())
                EnableEndingScene();
            Invoke(nameof(LevelStart), 0.25f);
        }
        private void LevelStart()
        {
            //SetGameToPlay(0);
            uiManager.SwitchMenu(PlayerPrefsHandler.HUD);
            currentLevel.onLevelStart.Invoke();
            SendProgressionEvent(GAProgressionStatus.Start);
        }
        public void GameComplete(float delay)
        {
            //Debug.Log("GameComplete");
            if(IsLevelFailed()) return;
            gameCompleteFlag = true;
            SendProgressionEvent(GAProgressionStatus.Complete);
            uiManager.HideAll();
            SetNextLevel();
            StartCoroutine(WaitForGameComplete(delay));
        }
        private IEnumerator WaitForGameComplete(float delay)
        {
            uiManager.controls.EnableHud(false);
            yield return new WaitForSeconds(delay);
            ShowLevelCompleteAd();
            uiManager.SwitchMenu(PlayerPrefsHandler.LevelComplete);
            //playerCamera.transform.Find("CompleteEffect").gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            GameManager.Instance.StartMode();
        }
        public void GameOver(float delay)
        {
            if(IsLevelCompleted()) return;
            gameOverFlag = true;
            RemovePlayerFromCamera();
            AdsCaller.Instance.ShowInterstitialAd();
            StartCoroutine(WaitForGameOver(delay));
        }
        private IEnumerator WaitForGameOver(float delay)
        {
            //print("GameOver");
            uiManager.CloseMenu(PlayerPrefsHandler.HUD);
            yield return new WaitForSeconds(delay);
            if (GameManager.Instance.IsBallMergeMode())
            {
                if (AdsManager.Instance.IsRewardedAdReady())
                    uiManager.SwitchMenu(PlayerPrefsHandler.RevivePopup);
                else
                    GameManager.Instance.StartMode();
            }
            else
            {
                SetHighScore(currentPlayer.GetComponent<BallController>().GetScore());
                //GameManager.Instance.GetComponent<Ghost>().StopRecordingGhost();
                GameManager.Instance.StartMode();
            }
            SendProgressionEvent(GAProgressionStatus.Fail);
        }
        private void ShowLevelCompleteAd()
        {
            var currentLevelNo = PlayerPrefsHandler.GetCurrentLevel(GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode));
            if (currentLevelNo != 0)
                AdsCaller.Instance.ShowInterstitialAd();
        }
        public bool IsOldControl()
        {
            return PlayerPrefsHandler.ControlType == "Old";
        }
        public void EnablePlayer(bool flag)
        {
            currentPlayer.SetActive(flag);
            if(IsOldControl())
                currentPlayer.GetComponent<BallController>().startRun = flag;
            else
                currentPlayer.GetComponent<PlayerController>().startRun = flag;
        }
        private void SetPlayerToCamera()
        {
            if(IsOldControl())
                playerCamera.GetComponent<PerfectCameraController>().target = currentPlayer.GetComponent<BallController>().GetCameraTarget();
            else
                playerCamera.GetComponent<CameraController>().target = currentPlayer.GetComponent<PlayerController>().GetCameraTarget();
        }

        private void RemovePlayerFromCamera()
        {
            if (GameManager.Instance.IsInfiniteMode())
            {
                playerCamera.GetComponent<PerfectCameraController>().target = null;
                return;
            }
            if (IsOldControl())
                playerCamera.GetComponent<PerfectCameraController>().target = null;
            else
                playerCamera.GetComponent<CameraController>().target = null;
        }
        public void StartPlaying()
        {
            if (GameManager.Instance.IsBallMergeMode())
            {
                if (IsOldControl())
                    currentPlayer.GetComponent<BallController>().StartMovement(true);
                else
                    currentPlayer.GetComponent<PlayerController>().StartMovement(true);
            }
            else if (GameManager.Instance.IsInfiniteMode())
            {
                currentPlayer.GetComponent<BallController>().StartMovement(true);
            }
        }
        private static void SetNextLevel()
        {
            var currentModeNo = GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode);
            var totalLevels = PlayerPrefsHandler.MergeBallsTotalLevels;
            var currentLevelNo = PlayerPrefsHandler.GetCurrentLevel(currentModeNo);
            if (GameManager.Instance.IsBallMergeMode())
            {
                totalLevels = PlayerPrefsHandler.MergeBallsTotalLevels;
            }
            else if (GameManager.Instance.IsInfiniteMode())
            {
                PlayerPrefsHandler.SetCurrentLevel(PlayerPrefsHandler.InfiniteMode, 0);
                return;
            }
            /*else if (GameManager.Instance.IsBallCrushMode())
        {
            totalLevels = PlayerPrefsHandler.CrushBallsTotalLevels;
        }*/
            if (currentLevelNo < totalLevels - 1)
                currentLevelNo++;
            else
                currentLevelNo = 0;
            PlayerPrefsHandler.SetCurrentLevel(currentModeNo, currentLevelNo);
            PlayerPrefsHandler.LevelsCounter++;
        }
        public void RevivePlayer()
        {
            GameManager.Instance.isRevived = true;
            var revivePoint = currentLevel.GetRevivePoint();
            if(!revivePoint)
                revivePoint = playerPositions.GetChild(PlayerPrefsHandler.GetCurrentLevel(GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode)));
            var rb = currentPlayer.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            var playerTempTransform = currentPlayer.transform;
            playerTempTransform.position = revivePoint.position;
            playerTempTransform.rotation = revivePoint.rotation;
            uiManager.SwitchMenu(PlayerPrefsHandler.HUD);
            uiManager.controls.EnableMenuItems(true);
            uiManager.controls.DisableControls();
            SetPlayerToCamera();
            //SetGameToPlay(0);
            if(IsOldControl())
                currentPlayer.GetComponent<BallController>().StartMovement(false);
            else
                currentPlayer.GetComponent<PlayerController>().StartMovement(false);
            rb.isKinematic = false;
            EnableLevelFailTrigger();
            gameOverFlag = false;
        }
        public void UpgradeBall()
        {
            Invoke(nameof(InvokeBallUpgrade), 0.2f);
        }
        private void InvokeBallUpgrade()
        {
            currentPlayer.GetComponent<PlayerBallMerge>().SpecificUpgrade(currentLevel.GetBallUpgradeValue());
        }
        public void RewardShield()
        {
            Invoke(nameof(InvokeBallShield), 0.2f);
        }
        private void InvokeBallShield()
        {
            currentLevel.DisablePowerUpPoint("Shield");
            currentPlayer.GetComponent<PlayerBallMerge>().ActivateShield();
        }
        public void RewardMagnet()
        {
            Invoke(nameof(InvokeBallMagnet), 0.2f);
        }
        private void InvokeBallMagnet()
        {
            if(GameManager.Instance.IsBallMergeMode())
                currentLevel.DisablePowerUpPoint("Magnet");
            if (GameManager.Instance.IsInfiniteMode())
            {
                currentPlayer.GetComponent<BallController>().StartMagnetEffect();
                return;
            }
            if(IsOldControl())
                currentPlayer.GetComponent<BallController>().StartMagnetEffect();
            else
                currentPlayer.GetComponent<PlayerController>().StartMagnetEffect();
        }
        public void RewardSkin()
        {
            Invoke(nameof(InvokeBallSkin), 0.2f);
        }
        private void InvokeBallSkin()
        {
            if(GameManager.Instance.IsBallMergeMode())
                currentLevel.DisablePowerUpPoint("BallSkin");
            currentPlayer.GetComponent<BallsSkinsHandler>().ApplySkin();
        }
        public void DownGradeWall()
        {
            Invoke(nameof(InvokeDownGradeWall), 0.2f);
        }
        private void InvokeDownGradeWall()
        {
            currentPlayer.GetComponent<MergeInfinityBall>().WallUpgrade();
        }
        private void EnableEndingScene()
        {
            var index = GameManager.Instance.GetEndingSceneIndex();
            if (currentLevel.IsMovingUpwards() && index == 2)
                index = 0;
            endingScenes[index].transform.position = currentLevel.GetEndingScenePoint(index).position;
            endingScenes[index].SetActive(true);
            GameManager.Instance.SetEndingSceneIndex();
        }
        private void EnableLevelFailTrigger()
        {
            levelFailTrigger.SetActive(true);   
        }
        private void SetHighScore(int newScore)
        {
            if (PlayerPrefsHandler.HighScore >= newScore) return;
            PlayerPrefsHandler.HighScore = newScore;
            uiManager.controls.SetHighScoreText(PlayerPrefsHandler.HighScore);
        }
        private void SendProgressionEvent(GAProgressionStatus status)
        {
            var eventString1 = "Mode " + GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode) + " Level " +
                               (PlayerPrefsHandler.LevelsCounter + 1);
            GameAnalytics.NewProgressionEvent(status,  eventString1);
            var eventString2 = "Mode_" + GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode) + "_Level_" +
                               (PlayerPrefsHandler.LevelsCounter + 1);
            FirebaseManager.Instance.ReportEvent(status + eventString2);
            AppmetricaAnalytics.ReportCustomEvent(AnalyticsType.GameData, $"Mode_{PlayerPrefsHandler.CurrentMode + 1}", $"Level_{PlayerPrefsHandler.LevelsCounter + 1}", 
                status.ToString());
        }
        public void SendLevelMiddleProgressionEvent()
        {
            var eventString1 = "Mode " + GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode) + " Level " +
                               (PlayerPrefsHandler.LevelsCounter + 1);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined,  eventString1);
            var eventString2 = "Mode_" + GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode) + "_Level_" +
                               (PlayerPrefsHandler.LevelsCounter + 1);
            FirebaseManager.Instance.ReportEvent("Mid" + eventString2);
        }
        public void SendInfiniteProgressionEvent(int patchesCount)
        {
            var eventString1 = "Mode " + GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode) + " PatchesCount " +
                               patchesCount;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,  eventString1);
            var eventString2 = "Mode_" + GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode) + "_PatchesCount_" +
                               patchesCount;
            FirebaseManager.Instance.ReportEvent(GAProgressionStatus.Complete + eventString2);
            AppmetricaAnalytics.ReportCustomEvent(AnalyticsType.GameData, $"Mode_{PlayerPrefsHandler.CurrentMode + 1}", $"Level_{PlayerPrefsHandler.LevelsCounter + 1}", 
                GAProgressionStatus.Complete.ToString());
        }
    }
}