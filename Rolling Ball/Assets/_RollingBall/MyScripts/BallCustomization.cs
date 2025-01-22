using _RollingBall.MyScripts;
using UnityEngine;
public class BallCustomization : MonoBehaviour
{
    private BallAppearance _ball;
    private int _ballIndex = 0;
    [SerializeField] private GameObject selectedImage, claimBtn;
    private void Start()
    {
        _ball = GamePlayManager.Instance.ball.GetComponent<BallAppearance>();
    }
    private void OnEnable()
    {
        _ballIndex = PlayerPrefsHandler.BallSkinNo;
        Callbacks.OnRewardRollingBallSkin += SelectBtnClicked;
        if (_ballIndex == PlayerPrefsHandler.BallSkinNo)
        {
            //selectBtn.SetActive(false);
            selectedImage.SetActive(true);
            claimBtn.SetActive(false);
        }
        else
        {
            //selectBtn.SetActive(false);
            selectedImage.SetActive(false);
            claimBtn.SetActive(true);
        }
    }
    private void OnDisable()
    {
        Callbacks.OnRewardRollingBallSkin -= SelectBtnClicked;
    }
    public void BackBtnClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        GamePlayManager.Instance.uiManager.SwitchMenu(PlayerPrefsHandler.HUD);
        _ball.ApplySkin(PlayerPrefsHandler.BallSkinNo);
        GamePlayManager.Instance.GetCameraController().SetBackNormalView();
    }
    public void RightBtnClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (_ballIndex < 5)
            _ballIndex += 1;
        else
            _ballIndex = 0;
        _ball.ApplySkin(_ballIndex);
        SetButtons();
    }
    public void LeftBtnClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (_ballIndex > 0)
            _ballIndex -= 1;
        else
            _ballIndex = 5;
        _ball.ApplySkin(_ballIndex);
        SetButtons();
    }
    public void ClaimBtnClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        Callbacks.ADType = Callbacks.RollingBallSkinRewardString;
        AdsCaller.Instance.ShowRewardedAd();
    }
    public void SelectBtnClicked()
    {
        SoundController.Instance.PlayBtnClickSound();
        PlayerPrefsHandler.BallSkinNo = _ballIndex;
        _ball.ApplySkin(_ballIndex);
        //selectBtn.SetActive(false);
        selectedImage.SetActive(true);
        claimBtn.SetActive(false);
    }
    private void SetButtons()
    {
        if (_ballIndex == PlayerPrefsHandler.BallSkinNo)
        {
            //selectBtn.SetActive(false);
            selectedImage.SetActive(true);
            claimBtn.SetActive(false);
        }
        else
        {
            //selectBtn.SetActive(false);
            selectedImage.SetActive(false);
            claimBtn.SetActive(true);
        }
    }
}