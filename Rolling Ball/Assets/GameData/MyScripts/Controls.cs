using GameData.MyScripts;
using UnityEngine;
using UnityEngine.UI;
public class Controls : MonoBehaviour
{
    #region Properties
    
    [SerializeField] private GameObject touchPad;
    [SerializeField] private GameObject menuItems;
    [SerializeField] private GameObject ballUpgradeBtn, shieldBtn, magnetBtn;
    [SerializeField] private GameObject mergeModeBtn, infiniteModeBtn;
    [SerializeField] private GameObject magnetEffectTimer;
    [SerializeField] private Text levelNoText;
    [SerializeField] private Text highScoreText, currentScoreText;
    [SerializeField] private GameObject highScoreNotification;

    #endregion
    
    #region Methods

    private void OnEnable()
    {
        Callbacks.OnRewardUpgradeBall += DisableUpgradeBtn;
        Callbacks.OnRewardShield += DisableShieldBtn;
        Callbacks.OnRewardMagnet += DisableMagnetBtn;
        SetPowerUpButtons();
    }
    private void OnDisable()
    {
        Callbacks.OnRewardUpgradeBall -= DisableUpgradeBtn;
        Callbacks.OnRewardShield -= DisableShieldBtn;
        Callbacks.OnRewardMagnet -= DisableMagnetBtn;
    }
    private void Start()
    {
        mergeModeBtn.SetActive(!GameManager.Instance.IsBallMergeMode());
        infiniteModeBtn.SetActive(!GameManager.Instance.IsInfiniteMode());
        if (GameManager.Instance.IsInfiniteMode())
        {
            levelNoText.text = "Infinite Mode";
            ballUpgradeBtn.SetActive(false);
            shieldBtn.SetActive(false);
            magnetBtn.SetActive(true);
            SetHighScoreText(PlayerPrefsHandler.HighScore);
            return;
        }
        levelNoText.text = "Level " + (PlayerPrefsHandler.LevelsCounter + 1);
        ballUpgradeBtn.transform.Find("Text").GetComponent<Text>().text =
            "Upgrade Ball To " + GamePlayManager.Instance.currentLevel.GetBallUpgradeValue();
    }
    public void EnableHud(bool flag)
    {
        gameObject.SetActive(flag);
    }
    public void TapToPlay()
    {
        SoundController.Instance.PlayBtnClickSound();
        EnableMenuItems(false);
        EnableControls();
    }
    public void EnableControls()
    {
        menuItems.SetActive(false);
        if(GamePlayManager.Instance.IsOldControl())
            touchPad.SetActive(true);
        EnableScorerUI(GameManager.Instance.IsInfiniteMode());
        GamePlayManager.Instance.StartPlaying();
    }
    public void DisableControls()
    {
        touchPad.SetActive(false);
    }
    public void EnableMenuItems(bool flag)
    {
        menuItems.SetActive(flag);
        if (GameManager.Instance.IsBallMergeMode() && GameManager.Instance.isRevived)
        {
            GameManager.Instance.isRevived = false;
            ballUpgradeBtn.SetActive(false);
        }
    }
    private void DisableUpgradeBtn()
    {
        ballUpgradeBtn.SetActive(false);
    }
    private void DisableShieldBtn()
    {
        shieldBtn.SetActive(false);
    }
    private void DisableMagnetBtn()
    {
        magnetBtn.SetActive(false);
    }
    public void WatchVideoToPowerUp(string powerUpName)
    {
        Callbacks.ADType = powerUpName;
        if(AdsCaller.Instance.IsRewardedAdAvailable())
            AdsCaller.Instance.ShowRewardedAd();
        else
            GamePlayManager.Instance.GetGamePlayUIManager().SubMenu(PlayerPrefsHandler.NoVideo);
    }
    public void ShowMagnetEffectTimer(bool flag)
    {
        magnetEffectTimer.SetActive(flag);
    }
    public void SetMagnetEffectTimerValue(int timerValue)
    {
        magnetEffectTimer.transform.Find("TimerText").GetComponent<Text>().text = timerValue.ToString();
    }
    private void EnableScorerUI(bool flag)
    {
        highScoreText.transform.parent.gameObject.SetActive(flag);
        currentScoreText.transform.parent.gameObject.SetActive(flag);
    }
    public void SetScoreText(int newScore)
    {
        currentScoreText.text = newScore.ToString();
    }
    public void SetHighScoreText(int highScore)
    {
        highScoreText.text = highScore.ToString();
        if(highScore == 0)
            highScoreText.text = "---";
    }
    public void ShowHighScoreNotification()
    {
        highScoreNotification.SetActive(true);
    }
    private void SetPowerUpButtons()
    {
            var btn = ballUpgradeBtn.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => WatchVideoToPowerUp("UpgradeBall"));
            btn.transform.Find("FreeText").gameObject.SetActive(true);
            btn.transform.Find("PriceText").gameObject.SetActive(false);
            var btn1 = shieldBtn.GetComponent<Button>();
            btn1.onClick.RemoveAllListeners();
            btn1.onClick.AddListener(() => WatchVideoToPowerUp("Shield"));
            btn1.transform.Find("FreeText").gameObject.SetActive(true);
            btn1.transform.Find("PriceText").gameObject.SetActive(false);
            var btn2 = magnetBtn.GetComponent<Button>();
            btn2.onClick.RemoveAllListeners();
            btn2.onClick.AddListener(() => WatchVideoToPowerUp("Magnet"));
            btn2.transform.Find("FreeText").gameObject.SetActive(true);
            btn2.transform.Find("PriceText").gameObject.SetActive(false);
    }
    #endregion
}