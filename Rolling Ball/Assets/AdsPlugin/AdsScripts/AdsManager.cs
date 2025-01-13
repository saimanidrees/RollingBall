using System;
using GameAnalyticsSDK;
using GameData.MyScripts;
using UnityEngine;
public class AdsManager : MonoBehaviour
{
    public string maxSdkKey = "6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR";
    public string interstitialAdUnitId = "0bf5dd259a7babe3";
    public string rewardedAdUnitId = "5d75002bbc4126b9";
    public string bannerAdUnitId = "ENTER_BANNER_AD_UNIT_ID_HERE";
    private int interstitialRetryAttempt;
    private int rewardedRetryAttempt;
    public static AdsManager Instance;
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this.gameObject);
        InitializeMax();
    }
    private void InitializeMax() {
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            // AppLovin SDK is initialized, configure and start loading ads.
            Debug.Log("MAX SDK Initialized");
            RegisterPaidAdEvent();
            InitializeInterstitialAds();
            InitializeRewardedAds();
            InitializeBannerAds();
            //InitializeMRecAds();
        };
        MaxSdk.SetSdkKey(maxSdkKey);
        MaxSdk.InitializeSdk();
    }
    #region Interstitial Ad Methods

    private void InitializeInterstitialAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.OnInterstitialDisplayedEvent += OnInterstitialDisplayedEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    

    public void LoadInterstitial()
    {
        //interstitialStatusText.text = "Loading...";
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
        // Debug.Log(InterstitialAdUnitId+ "InterstitialAdUnitId");
    }

    public bool IsInterstitialReady()
    {
        return MaxSdk.IsInterstitialReady(interstitialAdUnitId);
    }


    public void ShowInterstitial()
    {
        if (MaxSdk.IsInterstitialReady(interstitialAdUnitId))
        {
            // interstitialStatusText.text = "Showing";
            AppOpenAdCaller.IsInterstitialAdPresent = true;
            MaxSdk.ShowInterstitial(interstitialAdUnitId);
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, "MAX", "Interstitial_MAX");
            FirebaseManager.Instance.ReportEvent("ad_inter");
        }
        else
        {
            Debug.Log("Max Interstitial Failed");
            if (PlayerPrefsHandler.EnableFailOver)
            {
                AdmobManager.Instance.ShowInterstitial();
            }
            else
            {
                GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial, "MAX", "Interstitial_Fail");
                FirebaseManager.Instance.ReportEvent(GAAdAction.FailedShow + "_" + GAAdType.Interstitial + "_MAX_" + "Interstitial_Fail");
            }
        }
    }

    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
        // interstitialStatusText.text = "Loaded";
        Debug.Log("Interstitial loaded");

        // Reset retry attempt
        interstitialRetryAttempt = 0;
    }

    private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
    {
        Debug.Log(adUnitId + "InterstitialAdUnitId");
        // interstitialStatusText.text = "Failed load: " + errorCode + "\nRetrying in 3s...";
        Debug.Log("Interstitial failed to load with error code: " + errorCode);

        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays.

        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, interstitialRetryAttempt);

        Invoke("LoadInterstitial", (float) retryDelay);
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        Debug.Log("Interstitial failed to display with error code: " + errorCode);
        LoadInterstitial();
    }

    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        Debug.Log("Interstitial dismissed");
        LoadInterstitial();
    }
    private void OnInterstitialDisplayedEvent(string obj)
    {
        AppOpenAdCaller.IsInterstitialAdPresent = true;
        Debug.Log("Interstitial ad displayed");
    }

    #endregion

    #region Rewarded Ad Methods

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;



        // Load the first RewardedAd
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        //  rewardedStatusText.text = "Loading...";
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }

    public bool IsRewardedAdReady()
    {
        return MaxSdk.IsRewardedAdReady(rewardedAdUnitId);
    }

    public void ShowRewardedAd()
    {
        if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
        {
            // rewardedStatusText.text = "Showing";
            AppOpenAdCaller.IsInterstitialAdPresent = true;
            MaxSdk.ShowRewardedAd(rewardedAdUnitId);
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, "MAX", "Rewarded_MAX");
            FirebaseManager.Instance.ReportEvent(GAAdAction.Show + "_" + GAAdType.RewardedVideo + "_MAX_" +
                                                 "Rewarded_MAX");
        }
        else
        {
            if(PlayerPrefsHandler.EnableFailOver)
                AdmobManager.Instance.ShowRewardedAd();
            else
            {
                //GamePlayManager.Instance.GetGamePlayUIManager().SubMenu(PlayerPrefsHandler.NoVideo);
                GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo, "MAX", "Rewarded_Fail");
                FirebaseManager.Instance.ReportEvent(GAAdAction.FailedShow + "_" + GAAdType.RewardedVideo + "_MAX_" +
                                                     "Rewarded_Fail");
            }
        }
    }

    private void OnRewardedAdLoadedEvent(string adUnitId)
    {

        Debug.Log("Rewarded ad loaded");
        // Reset retry attempt
        rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
        // rewardedStatusText.text = "Failed load: " + errorCode + "\nRetrying in 3s...";
        Debug.Log("Rewarded ad failed to load with error code: " + errorCode);

        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays.

        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, rewardedRetryAttempt);
        Invoke("LoadRewardedAd", (float) retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {

        // Rewarded ad failed to display. We recommend loading the next ad
        Debug.Log("Rewarded ad failed to display with error code: " + errorCode);
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId)
    {
        Debug.Log("Rewarded ad displayed");
        AppOpenAdCaller.IsInterstitialAdPresent = true;
    }

    private void OnRewardedAdClickedEvent(string adUnitId)
    {
        Debug.Log("Rewarded ad clicked");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId)
    {

        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("Rewarded ad received reward");
        Callbacks.RewardedAdWatched();
    }

    #endregion

    #region Banner Ad Methods

    private void InitializeBannerAds()
    {
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);
    }
    public void ShowBanner()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }
    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerAdUnitId);
    }

    #endregion

    private void RegisterPaidAdEvent()
    {
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += MaxHandleInterstitialPaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += MaxHandleRewardedPaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += MaxHandleBannerPaidEvent;
    }
    private void MaxHandleInterstitialPaidEvent(string sender, MaxSdkBase.AdInfo adInfo)
    {
        AdjustManager.Instance.Max(adInfo);
        AppmetricaAnalytics.ReportRevenue_Applovin(adInfo, AppmetricaAnalytics.AdFormat.Interstitial, interstitialAdUnitId);
    }
    private void MaxHandleRewardedPaidEvent(string sender, MaxSdkBase.AdInfo adInfo)
    {
        AdjustManager.Instance.Max(adInfo);
        AppmetricaAnalytics.ReportRevenue_Applovin(adInfo, AppmetricaAnalytics.AdFormat.Rewarded, rewardedAdUnitId);
    }
    private void MaxHandleBannerPaidEvent(string sender, MaxSdkBase.AdInfo adInfo)
    {
        AdjustManager.Instance.Max(adInfo);
        FirebaseManager.Instance.ReportEvent("ad_banner");
        AppmetricaAnalytics.ReportRevenue_Applovin(adInfo, AppmetricaAnalytics.AdFormat.Banner, bannerAdUnitId);
    }
}