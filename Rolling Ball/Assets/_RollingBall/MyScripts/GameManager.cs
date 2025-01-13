using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _RollingBall.MyScripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool isTesting = true;

        [ShowIf("isTesting")] [SerializeField, Range(0, PlayerPrefsHandler.TotalLevels - 1)]
        private int levelNo;

        public static GameManager Instance;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            if (isTesting)
                PlayerPrefsHandler.CurrentLevelNo = levelNo;
        }

        public void LoadGamePlay()
        {
            SceneManager.LoadScene(PlayerPrefsHandler.GamePlayScene);
        }

        public void Restart()
        {
            SceneManager.LoadScene(PlayerPrefsHandler.SplashScene);
        }

        public bool IsTesting()
        {
            return isTesting;
        }
    }
}