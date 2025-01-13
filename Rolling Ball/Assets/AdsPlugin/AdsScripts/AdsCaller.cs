using GameAnalyticsSDK;
using GameData.MyScripts;
using UnityEngine;
public class AdsCaller : MonoBehaviour
{
    public static AdsCaller Instance;
    private float _time = 0;
    private bool _startTimer = false;
    private bool _adReady = false;
    
    private float _bannerTime = 0;
    private bool _startBannerTimer = false;
    private void Awake()
    {
        Instance = this;
    }
    /*private void Update()
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
        if(!_startBannerTimer) return;
        _bannerTime -= Time.deltaTime;
        if (!(_bannerTime <= 0))
            return;
        RequestFlooringBanner();
        _startBannerTimer = false;
    }*/
    public void StartAdTimer(float interval)
    {
        _time = interval;
        _startTimer = true;
    }
    public void StartBannerAdTimer(float interval)
    {
        _bannerTime = interval;
        _startBannerTimer = true;
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
        if(PlayerPrefsHandler.GetBool("RemoveAds" + PlayerPrefsHandler.CurrentMode)) return;
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