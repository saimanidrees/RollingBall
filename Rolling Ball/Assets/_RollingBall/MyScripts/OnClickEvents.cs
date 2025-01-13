using UnityEngine;
using UnityEngine.EventSystems;
namespace _RollingBall.MyScripts
{
	public class OnClickEvents : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
	{
		public string buttonName;
		public void OnPointerUp(PointerEventData eventData)
		{
			if(SoundController.Instance)
				SoundController.Instance.PlayBtnClickSound();
			switch (buttonName)
			{
				case PlayerPrefsHandler.SwipeToPlay:
				
					break;
				case PlayerPrefsHandler.NextComplete:
					GamePlayManager.Instance.StartMode(GameData.MyScripts.PlayerPrefsHandler.RollingBallMode);
					break;
				case PlayerPrefsHandler.Replay:
					GamePlayManager.Instance.StartMode(GameData.MyScripts.PlayerPrefsHandler.RollingBallMode);
					break;
				case PlayerPrefsHandler.Home:
					GamePlayManager.Instance.StartMode(GameData.MyScripts.PlayerPrefsHandler.MergeBallMode);
					break;
				case PlayerPrefsHandler.Settings:
					GamePlayManager.Instance.uiManager.SubMenu(PlayerPrefsHandler.Settings);
					break;
				case PlayerPrefsHandler.SettingsClose:
					GamePlayManager.Instance.uiManager.CloseSubMenu();
					break;
				case PlayerPrefsHandler.BallCustomization:
					GamePlayManager.Instance.GetCameraViewController().SetBallSelectionView(12);
					GamePlayManager.Instance.uiManager.SwitchMenu(PlayerPrefsHandler.BallCustomization);
					break;
				case Callbacks.SkipLevelString:
					Callbacks.ADType = Callbacks.SkipLevelString;
					AdsCaller.Instance.ShowRewardedAd();
					break;
			}
		}
		public void OnPointerDown(PointerEventData eventData)
		{
		
		}
	}
}