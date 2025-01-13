using Firebase.Analytics;
using GameAnalyticsSDK;
using GameData.MyScripts;
using UnityEngine;
public class NeutralAgeScreenPanel : MonoBehaviour
{
    [SerializeField] private GameObject agePanel;
    [SerializeField] private Animator animatorOfSplash;

    private const string PRIVACY_POLICY_LINK = "https://thegoodtoseeyou.com/privacy-policy/";
    private const string TERMS_LINK = "https://thegoodtoseeyou.com/terms-and-conditions/";
    private const string HasAskedForAgeString = "HasAskedForAge", PrivacyString = "Privacy", BeforePrivacyString = "Before_Privacy", AfterPrivacyString = "After_Privacy";
    private void Start()
    {
        Invoke(nameof(ShowThePanelOnStart), 1f);
    }

    private void ShowThePanelOnStart()
    {
        if(PlayerPrefsHandler.GetBool(HasAskedForAgeString))
        {
            gameObject.SetActive(false);
            return;
        };
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, PrivacyString);
        FirebaseAnalytics.LogEvent(BeforePrivacyString);
        animatorOfSplash.enabled = false;
        ShowPanel();
    }

    private void ShowPanel()
    {
        agePanel.SetActive(true);
    }

    public void HidePanel()
    {
        agePanel.SetActive(false);
        PlayerPrefsHandler.SetBool(HasAskedForAgeString, true);
        animatorOfSplash.enabled = true;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, PrivacyString);
        FirebaseAnalytics.LogEvent(AfterPrivacyString);
        gameObject.SetActive(false);
    }

    public void GotoPolicySection()
    {
        Application.OpenURL(PRIVACY_POLICY_LINK);
    }

    public void GotoTermsSection()
    {
        Application.OpenURL(TERMS_LINK);
    }
}