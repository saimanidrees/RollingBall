using GameData.MyScripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class MiniWheel : MonoBehaviour
{
    [SerializeField]private Text rewardTextMiniGame;
    private int reward;
    private int rewardMultiplier;
    public Button collectButton;
    public UnityEvent EndSpin;
    public UnityEvent CollectButtonOnClick;
    private bool isAlreadyComandGiven;
    private void Start()
    {
        collectButton.onClick.AddListener(CollectButtonClick);
    }
    private void OnEnable()
    {
        Callbacks.OnRewardMiniWheel += MiniGameReward;
        collectButton.interactable = true;
    }
    private void OnDisable()
    {
        Callbacks.OnRewardMiniWheel -= MiniGameReward;
    }
    private void SetAlreadyCommandBool()
    {
        isAlreadyComandGiven = true;
    }
    public void CollectButtonClick()
    {
        PlayButtonClickSound(); 
        CollectButtonOnClick.Invoke();
        OnTap();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.GetComponent<RewardValue>())
            return;
        
        rewardMultiplier = other.GetComponent<RewardValue>().multiplierValue;
        rewardTextMiniGame.text = "+ "+(CurrencyCounter.Instance.GetCashReward() * rewardMultiplier);
    }
    private void OnTap()
    {
        if (GameManager.Instance.isFreeStop)
        {
            OnStopReward();
            GameManager.Instance.isFreeStop = false;
            MiniGameReward();
        }
        else
        {
            Callbacks.ADType = "MiniWheel";
            if (!CheckAdAvailable())
            {
                GamePlayManager.Instance.GetGamePlayUIManager().SubMenu(PlayerPrefsHandler.NoVideo);
                return;
            }
            OnStopReward();
            AdsCaller.Instance.ShowRewardedAd();
        }
    }
    private bool CheckAdAvailable()
    {
        return AdsManager.Instance.IsRewardedAdReady();
    }
    private void PlayButtonClickSound()
    {
        SoundController.Instance.PlayBtnClickSound();
    }
    private void OnStopReward()
    {
        //EventManager.instance.InvokeLevelCompleteCommandGiven();
        EndSpin.Invoke();
        GetComponent<Animator>().enabled = false;
    }
    private void MiniGameReward()
    {
        GamePlayManager.Instance.isLevelCompleteRewardGiven = true;
        collectButton.interactable = false;
        reward = CurrencyCounter.Instance.GetCashReward() * rewardMultiplier;
        CurrencyCounter.Instance.SetCompleteReward(reward);
        CurrencyCounter.Instance.ShowCashEffect(collectButton.transform);
        //Invoke(nameof(LoadNextLevel),2f);
    }
    public void NoThanks()
    {
        collectButton.interactable = false;
        GetComponent<Animator>().enabled = false;
        CurrencyCounter.Instance.ShowCashEffect(collectButton.transform);
    }
}