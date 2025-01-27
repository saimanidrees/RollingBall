using System.Collections;
using DanielLochner.Assets.SimpleScrollSnap;
using GameData.MyScripts;
using UnityEngine;
public class SplashScript : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private SimpleScrollSnap simpleScrollSnap;
    private readonly WaitForSeconds _delay = new (0.3f);
    public void StartMode(int modeNo)
    {
        SoundController.Instance.PlayBtnClickSound();
        PlayerPrefsHandler.CurrentMode = modeNo;
        loadingPanel.SetActive(true);
        simpleScrollSnap.gameObject.SetActive(false);
    }
    private IEnumerator Start()
    {
        yield return null;
        simpleScrollSnap.GoToNextPanel();
        yield return _delay;
        simpleScrollSnap.GoToNextPanel();
        yield return _delay;
        simpleScrollSnap.GoToNextPanel();
    }
}