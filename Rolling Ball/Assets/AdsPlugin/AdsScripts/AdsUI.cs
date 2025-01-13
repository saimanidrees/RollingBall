using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdsUI : MonoBehaviour
{
    [SerializeField] private Text text;
    private void OnEnable()
    {
        StartCoroutine(ShowAdCountDown());
    }
    private IEnumerator ShowAdCountDown()
    {
        for (var i = 3; i > 0; i--)
        {
            text.text = "SHOWING AD IN " + i;
            yield return new WaitForSeconds(0.5f);
        }
        AdsCaller.Instance.ShowInterstitialAd();
        gameObject.SetActive(false);
    }
}
