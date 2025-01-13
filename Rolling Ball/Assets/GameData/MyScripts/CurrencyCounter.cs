using System.Collections;
using DG.Tweening;
using GameData.MyScripts;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private int cashReward = 500;
    private int _coinsReward = 0;
    [SerializeField] private Text cashText;
    [SerializeField] private GameObject coinsEffect;
    [SerializeField] private Transform coinsEffectTargetPos;
    [SerializeField] private AnimationCurve coinsEffectCurve;
    public static CurrencyCounter Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        UpdateCoinsText();
    }
    private void UpdateCoinsText()
    {
        cashText.text = PlayerPrefsHandler.Coins.ToString();
    }
    public void UpdateCurrency(int amount)
    {
        PlayerPrefsHandler.Coins += amount;
        if (PlayerPrefsHandler.Coins < 0)
        {
            PlayerPrefsHandler.Coins = 0;
        }
        UpdateCoinsText();
    }
    public void ShowCashEffect(Transform startingPos)
    {
        coinsEffect.transform.position = startingPos.transform.position;
        coinsEffect.SetActive(true);
    }
    public void SetCompleteReward(int miniWheelReward)
    {
        cashReward = miniWheelReward;
    }
    public void SetCurrency()
    {
        var previousValue = PlayerPrefsHandler.Coins;
        PlayerPrefsHandler.Coins += cashReward;
        StartCoroutine(CountUpToTarget(previousValue, PlayerPrefsHandler.Coins, 5f));
    }
    private IEnumerator CountUpToTarget(int previousVal, int targetVal, float duration)
    {
        var current = previousVal;
        while (current < targetVal)
        {
            current += (int)(targetVal / (duration/Time.deltaTime));
            current = Mathf.Clamp(current, 0, targetVal);
            cashText.text = current.ToString();
            yield return null;
        }
        UpdateCoinsText();
        coinsEffect.SetActive(false);
        SoundController.Instance.PlayBuySound();
        Invoke($"Continue", 0.5f);
    }
    public void UpdateCurrency(int amount , float delay)
    {
        PlayerPrefsHandler.Coins += amount;
        if (PlayerPrefsHandler.Coins < 0)
        {
            PlayerPrefsHandler.Coins = 0;
        }
    }
    public void CurrencyDeduction(int coins)
    {
        CurrencyRegisterSound();
        PlayerPrefsHandler.Coins -= coins;
        UpdateCoinsText();
    }
    private static void CurrencyRegisterSound()
    {
        if(SoundController.Instance)
            SoundController.Instance.PlayBuySound();
    }
    public int GetCoinsReward()
    {
        return _coinsReward;
    }
    public void SetCoinsReward(int coinsValue)
    {
        _coinsReward += coinsValue;
    }
    public int GetCashReward()
    {
        return cashReward;
    }
    public void SetCashReward(int cashValue)
    {
        cashReward += cashValue;
    }
    private void Continue()
    {
        GameManager.Instance.StartMode();
    }
    public void MoveTowardsTarget()
    {
        coinsEffect.transform.DOMove(coinsEffectTargetPos.position, 1f).SetEase(coinsEffectCurve).OnComplete(() =>
        {
            coinsEffect.transform.Find("CoinsAnimator").GetComponent<Animator>().Play("CoinsDisappear");
        });
    }
    public void SetCurrencyCounterPosition(Vector2 newPos)
    {
        GetComponent<RectTransform>().anchoredPosition = newPos;
    }
}