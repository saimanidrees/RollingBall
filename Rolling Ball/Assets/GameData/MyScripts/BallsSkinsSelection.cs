using GameData.MyScripts;
using UnityEngine;
using UnityEngine.UI;
public class BallsSkinsSelection : MonoBehaviour
{
    [SerializeField] private GameObject tapToTryBtn, selectedBtn, rightArrowBtn, leftArrowBtn;
    [SerializeField] private Image mainBallRender;
    [SerializeField] private Transform ballsButtonsContainer0, ballsButtonsContainer1;
    [SerializeField] private Sprite selectedSprite, unselectedSprite;
    [SerializeField] private Sprite[] ballsRenders;
    private int _currentSkinNo = 0;
    private void OnEnable()
    {
        Callbacks.OnRewardBallSkin += GiveSkinAsReward;
        RefreshUI();
    }
    private void OnDisable()
    {
        Callbacks.OnRewardBallSkin -= GiveSkinAsReward;
    }
    public void ShowRewardedAd()
    {
        SoundController.Instance.PlayBtnClickSound();
        GamePlayManager.Instance.currentPlayer.GetComponent<BallsSkinsHandler>().SetSkinIndex(_currentSkinNo);
        Callbacks.ADType = "BallSkin";
        if(AdsCaller.Instance.IsRewardedAdAvailable())
            AdsCaller.Instance.ShowRewardedAd();
        else
            GamePlayManager.Instance.GetGamePlayUIManager().SubMenu(PlayerPrefsHandler.NoVideo);
    }
    private void GiveSkinAsReward()
    {
        SoundController.Instance.PlayBuySound();
        Debug.Log("_currentSkinNo: " + _currentSkinNo);
        GamePlayManager.Instance.isBallUnlocked[_currentSkinNo] = true;
        RefreshUI();
    }
    public void CloseBallsSkinsSelection()
    {
        SoundController.Instance.PlayBtnClickSound();
        GamePlayManager.Instance.GetGamePlayUIManager().SwitchMenu(PlayerPrefsHandler.HUD);
        AdsCaller.Instance.ShowInterstitialAd();
    }
    public void SelectBall(int skinNo)
    {
        SoundController.Instance.PlayBtnClickSound();
        _currentSkinNo = skinNo;
        RefreshUI();
    }
    private void RefreshUI()
    {
        if(_currentSkinNo == -1) return;
        for (var i = 0; i < ballsButtonsContainer0.childCount; i++)
        {
            var ballBtn = ballsButtonsContainer0.GetChild(i);
            ballBtn.Find("LockedText").GetComponent<Text>().text = "Locked";
            ballBtn.GetComponent<Image>().sprite = unselectedSprite;
        }
        for (var i = 0; i < ballsButtonsContainer1.childCount; i++)
        {
            var ballBtn = ballsButtonsContainer1.GetChild(i);
            ballBtn.Find("LockedText").GetComponent<Text>().text = "Locked";
            ballBtn.GetComponent<Image>().sprite = unselectedSprite;
        }
        if(_currentSkinNo <= 5)
            ballsButtonsContainer0.GetChild(_currentSkinNo).GetComponent<Image>().sprite = selectedSprite;
        else if (_currentSkinNo > 5)
        {
            ballsButtonsContainer1.GetChild(_currentSkinNo - 6).GetComponent<Image>().sprite = selectedSprite;
        }
        //Debug.Log("_currentSkinNo: " + _currentSkinNo + " _isBallUnlocked:" + PlayerPrefsHandler._isBallUnlocked[_currentSkinNo]);
        mainBallRender.gameObject.SetActive(false);
        mainBallRender.sprite = ballsRenders[_currentSkinNo];
        mainBallRender.gameObject.SetActive(true);
        tapToTryBtn.SetActive(!GamePlayManager.Instance.isBallUnlocked[_currentSkinNo]);
        selectedBtn.SetActive(GamePlayManager.Instance.isBallUnlocked[_currentSkinNo]);
        if (!GamePlayManager.Instance.isBallUnlocked[_currentSkinNo]) return;
        var skinHandler = GamePlayManager.Instance.currentPlayer.GetComponent<BallsSkinsHandler>();
        skinHandler.SetSkinIndex(_currentSkinNo);
        skinHandler.ApplySkin();
        if(_currentSkinNo <= 5)
            ballsButtonsContainer0.GetChild(_currentSkinNo).Find("LockedText").GetComponent<Text>().text = "Selected";
        else if (_currentSkinNo > 5)
        {
            ballsButtonsContainer1.GetChild(_currentSkinNo - 6).Find("LockedText").GetComponent<Text>().text = "Selected";
        }
    }
    public void RightArrowClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        ballsButtonsContainer0.gameObject.SetActive(false);
        ballsButtonsContainer1.gameObject.SetActive(true);
        leftArrowBtn.SetActive(true);
        rightArrowBtn.SetActive(false);
    }
    public void LeftArrowClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        ballsButtonsContainer0.gameObject.SetActive(true);
        ballsButtonsContainer1.gameObject.SetActive(false);
        leftArrowBtn.SetActive(false);
        rightArrowBtn.SetActive(true);
    }
}