using GameData.MyScripts;
using UnityEngine;
public class AdsCaller : MonoBehaviour
{
    public static AdsCaller Instance;
    private float _time = 0;
    private bool _startTimer = false;
    private bool _adReady = false;
    private const string RemoveAdsString = "RemoveAds";
    public enum InterType
    {
        Simple,
        Timer
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (_startTimer)
        {
            if (_adReady) return;
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _adReady = true;
                _startTimer = false;
            }
        }
    }
    public void StartAdTimer()
    {
        _time = PlayerPrefsHandler.InterTimeInterval;
        _startTimer = true;
        _adReady = false;
    }
    public void EndAdTimer()
    {
        _startTimer = false;
    }
    public void ShowTimerAd()
    {
        _startTimer = false;
        if(!_adReady) return;
        _adReady = false;
        ShowInterstitialAd();
    }
    public void ShowInterstitialAd()
    {
        if(PlayerPrefsHandler.GetBool(RemoveAdsString + PlayerPrefsHandler.CurrentMode)) return;
        AdsManager.Instance.ShowInterstitial();
    }
    public bool IsInterstitialAdAvailable()
    {
        return AdsManager.Instance.IsInterstitialReady();
    }
    public void ShowBanner()
    {
        AdsManager.Instance.ShowBanner();
    }
    public void HideBanner()
    {
        AdsManager.Instance.HideBanner();
    }
    private bool _isRewardedAdCall = false;
    public void ShowRewardedAd()
    {
        if (_isRewardedAdCall) return;
        _isRewardedAdCall = true;
        Invoke(nameof(SetRewardedBool),0.5f);
        AdsManager.Instance.ShowRewardedAd();
    }
    public bool IsRewardedAdAvailable()
    {
        return AdsManager.Instance.IsRewardedAdReady();
    }
    private void SetRewardedBool()
    {
        _isRewardedAdCall = false;
    }
}