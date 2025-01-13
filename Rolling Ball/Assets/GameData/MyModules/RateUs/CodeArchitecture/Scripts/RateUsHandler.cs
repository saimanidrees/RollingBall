using GameData.MyScripts;
using UnityEngine;
public class RateUsHandler : MonoBehaviour
{
    public GameObject Hand;
    public GameObject[] AllStars;
    [SerializeField] private GameObject reviewManager;
    public void FillImage(float fillAmount) 
    {
        Debug.Log("fillAmount: " + fillAmount);
        var limit = fillAmount * 5;
        for (var i = 0; i < limit; i++)
        {
            AllStars[i].SetActive(true);
        }
        FirebaseManager.Instance.ReportEvent("RateUs_Stars_" + fillAmount * 5);
        if (fillAmount >= 0.8f)
        {
            PlayerPrefsHandler.SetBool("RateUs", true);
            FirebaseManager.Instance.ReportEvent("RateUs_Panel_Opened");
            RateUs();
            Invoke(nameof(ClosePanel), 1f);
        }
        else 
        {
            LaterClick();
        }
        Hand.SetActive(false);
    }

    private void RateUs()
    {
        //Debug.Log("RateUs_Panel_Opened");
        SoundController.Instance.PlayBtnClickSound();
        reviewManager.SetActive(true);
        //Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
        //PlayerPrefsHandler.SetBool("RateUs", true);
    }
    private void LaterClick()
    {
        Debug.Log("RateUs_Panel_Closed");
        //PlayerPrefsHandler.SetBool("RateUs", true);
        Invoke(nameof(ClosePanel), 1f);
    }
    public void ClosePanel()
    {
        SoundController.Instance.PlayBtnClickSound();
        gameObject.SetActive(false);
    }
}