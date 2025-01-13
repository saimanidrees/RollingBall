using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using UnityEngine.Events;

public class AppOpenAdCaller : MonoBehaviour
{
    [SerializeField] private string appOpenID = "";
    [SerializeField] private ScreenOrientation orientation= ScreenOrientation.LandscapeLeft;
    public static bool IsInterstitialAdPresent;
    private AppOpenAd _appOpenAd;
    private readonly TimeSpan _appOpenTimeout = TimeSpan.FromHours(4);
    private DateTime _appOpenExpireTime;
    [HideInInspector]
    public UnityEvent onAdOpeningEvent, onAdClosedEvent;
    
    private void OnDestroy()
    {
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }
    public void InitAppOpen()
    {
        //Debug.Log("Initializing AppOpen...");
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        Invoke(nameof(ShowAppOpenAd), 3f);
    }

    #region APPOPEN ADS

    public bool IsAppOpenAdAvailable
    {
        get
        {
            return (_appOpenAd != null
                    && _appOpenAd.CanShowAd()
                    && DateTime.Now < _appOpenExpireTime);
        }
    }

    private void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        PrintStatus("App State is " + state);

        // OnAppStateChanged is not guaranteed to execute on the Unity UI thread.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (state == AppState.Foreground)
            {
                ShowAppOpenAd();
            }
        });
    }
    //ca-app-pub-3940256099942544/3419835294
    private void RequestAndLoadAppOpenAd()
    {
        PrintStatus("Requesting App Open ad.");
#if UNITY_EDITOR
        var adUnitId = appOpenID;
#elif UNITY_ANDROID
        var adUnitId = appOpenID;
#elif UNITY_IPHONE
        var adUnitId = appOpenID;
#else
        var adUnitId = "unexpected_platform";
#endif

        // destroy old instance.
        if (_appOpenAd != null)
        {
            DestroyAppOpenAd();
        }

        // Create a new app open ad instance.
        AppOpenAd.Load(adUnitId, orientation, CreateAdRequest(),
            (AppOpenAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    PrintStatus("App open ad failed to load with error: " +
                        loadError.GetMessage());
                    return;
                }
                else if (ad == null)
                {
                    PrintStatus("App open ad failed to load.");
                    return;
                }
                PrintStatus("App Open ad loaded. Please background the app and return.");
                this._appOpenAd = ad;
                this._appOpenExpireTime = DateTime.Now + _appOpenTimeout;

                ad.OnAdFullScreenContentClosed += () =>
                {
                    PrintStatus("App open ad closed."); 
                    AdsCaller.Instance.ShowBanner();
                    onAdClosedEvent.Invoke();
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    PrintStatus("App open ad failed to show with error: " +
                                error.GetMessage());
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    var msg = string.Format("{0} (currency: {1}, value: {2}",
                        "App open ad received a paid event.",
                        adValue.CurrencyCode,
                        adValue.Value);
                    AdjustManager.Instance.Admob(adValue);
                    AppmetricaAnalytics.ReportRevenue_Admob(adValue, AppmetricaAnalytics.AdFormat.AppOpen, adUnitId);
                    PrintStatus(msg);
                };
            });
    }

    private void DestroyAppOpenAd()
    {
        if (this._appOpenAd != null)
        {
            this._appOpenAd.Destroy();
            this._appOpenAd = null;
        }
    }

    public void ShowAppOpenAd()
    {
        if (IsInterstitialAdPresent)
        {
            IsInterstitialAdPresent = false;
            return;
        }
        if (!IsAppOpenAdAvailable)
        {
            RequestAndLoadAppOpenAd();
            return;
        }
        _appOpenAd.Show();
        AdsCaller.Instance.HideBanner();
        //AdsCaller.Instance.DestroyRectBanner();
    }

    /*public void OnApplicationPause(bool paused)
    {
        // Display the app open ad when the app is foregrounded
        if (!paused)
        {
            //Debug.Log("OnApplicationPause: " + IsInterstitialAdPresent);
            if (IsInterstitialAdPresent)
            {
                IsInterstitialAdPresent = false;
                return;
            }
            ShowAppOpenAd();
        }
    }*/

    #endregion
    
    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    #endregion
    
    #region Utility

    private void PrintStatus(string message)
    {
        Debug.Log(message);
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            // statusText.text = message;
        });
    }

    #endregion
}
