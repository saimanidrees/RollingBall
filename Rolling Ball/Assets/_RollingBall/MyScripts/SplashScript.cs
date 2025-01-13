using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class SplashScript : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.LoadGamePlay();
        }
    }
}