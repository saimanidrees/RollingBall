using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace GameData.MyScripts
{
    public class GameManager : MonoBehaviour
    {
        private int _endingSceneIndex = 0;
        [HideInInspector] public bool isRevived = false, isFreeStop = false;
        [SerializeField] private bool isTesting = true;
        [SerializeField, Range(0, 1)] private int modeNo;
        [SerializeField, Range(0, PlayerPrefsHandler.MergeBallsTotalLevels - 1)] private int levelNo;
        public static GameManager Instance;
        private void Awake()
        {
            if (Instance) return;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;
            GameAnalytics.Initialize();
            if (isTesting)
            {
                PlayerPrefsHandler.CurrentMode = modeNo;
                PlayerPrefsHandler.SetCurrentLevel(GetModeName(modeNo), levelNo);
                _RollingBall.MyScripts.PlayerPrefsHandler.CurrentLevelNo = levelNo;
            }
            else
            {
                PlayerPrefsHandler.CurrentMode = 0;
            }
            PlayerPrefsHandler.SetBool("Revive", false);
        }
        public bool IsBallMergeMode()
        {
            return PlayerPrefsHandler.CurrentMode == 0;
        }
        public bool IsInfiniteMode()
        {
            return PlayerPrefsHandler.CurrentMode == 1;
        }
        public bool IsRollingBallMode()
        {
            return PlayerPrefsHandler.CurrentMode == 2;
        }
        public int GetModeNo(string modeName)
        {
            return modeName switch
            {
                PlayerPrefsHandler.MergeBallMode => 0,
                PlayerPrefsHandler.InfiniteMode => 1,
                PlayerPrefsHandler.RollingBallMode => 2,
                _ => 0
            };
        }
        public string GetModeName(int modeNo)
        {
            return modeNo switch
            {
                0 => PlayerPrefsHandler.MergeBallMode,
                1 => PlayerPrefsHandler.InfiniteMode,
                2 => PlayerPrefsHandler.RollingBallMode,
                _ => PlayerPrefsHandler.MergeBallMode
            };
        }
        public int GetModeSceneIndex()
        {
            if (IsBallMergeMode())
                return 1;
            else if (IsInfiniteMode())
                return 2;
            else if (IsRollingBallMode())
                return 3;
            return 1;
        }
        public int GetModeSceneIndex(string modeName)
        {
            if (modeName == PlayerPrefsHandler.MergeBallMode)
                return 1;
            else if (modeName == PlayerPrefsHandler.InfiniteMode)
                return 2;
            else if (modeName == PlayerPrefsHandler.RollingBallMode)
                return 3;
            return 1;
        }
        public void StartMode()
        {
            SceneManager.LoadScene(GetModeSceneIndex());
        }
        public void StartMode(string modeName)
        {
            SceneManager.LoadScene(GetModeSceneIndex(modeName));
        }
        public int GetEndingSceneIndex()
        {
            return _endingSceneIndex;
        }
        public void SetEndingSceneIndex()
        {
            const int endingScenesLimit = 1;
            _endingSceneIndex++;
            if (_endingSceneIndex > endingScenesLimit)
                _endingSceneIndex = 0;
        }
    }
}