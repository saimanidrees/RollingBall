using GameData.MyScripts;
using UnityEngine;
public class SplashScript : MonoBehaviour
{
    [SerializeField] private GameObject modesPanel, loadingPanel;
    public void StartMode(int modeNo)
    {
        SoundController.Instance.PlayBtnClickSound();
        PlayerPrefsHandler.CurrentMode = modeNo;
        loadingPanel.SetActive(true);
        modesPanel.SetActive(false);
    }
}