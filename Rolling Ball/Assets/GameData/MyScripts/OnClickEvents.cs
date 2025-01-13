using GameData.MyScripts;
using UnityEngine;
using UnityEngine.EventSystems;
public class OnClickEvents : MonoBehaviour ,IPointerUpHandler , IPointerDownHandler
 {
	public string buttonName;
	public void OnPointerUp(PointerEventData eventData)
	{
		if(SoundController.Instance)
			SoundController.Instance.PlayBtnClickSound();
		switch (buttonName)
		{
			case "Play":
				break;
			case "MoreGames":
				//Application.OpenURL(GamePlayManager.Instance.moreGamesLink);
				break;
			case "RateUs":
				Application.OpenURL("market://details?id=" + Application.identifier);
				break;
			case "PrivacyPolicy":
				//Application.OpenURL(GamePlayManager.Instance.privacyPolicyLink);
				break;
			case "SceneReset":
				GamePlayManager.Instance.GetGamePlayUIManager().SwitchMenu(PlayerPrefsHandler.Loading);
				break;
			case "HideSubMenu":
				GamePlayManager.Instance.GetGamePlayUIManager().HideAllSubMenus();
				break;
			case PlayerPrefsHandler.RemoveAdsPopup:
				GamePlayManager.Instance.GetGamePlayUIManager().SubMenu(PlayerPrefsHandler.RemoveAdsPopup);
				break;
			case PlayerPrefsHandler.Settings:
				GamePlayManager.Instance.GetGamePlayUIManager().SubMenu(PlayerPrefsHandler.Settings);
				break;
			case "HideBreakTheWallPopup":
				GamePlayManager.Instance.GetGamePlayUIManager().HideAllSubMenus();
				Invoke(nameof(InvokePlayerMovement), 1f);
				break;
			case PlayerPrefsHandler.BallsSkinsSelection:
				GamePlayManager.Instance.GetGamePlayUIManager().SwitchMenu(PlayerPrefsHandler.BallsSkinsSelection);
				break;
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		
	}
	private void InvokePlayerMovement()
	{
		GamePlayManager.Instance.currentPlayer.GetComponent<Rigidbody>().isKinematic = false;
		GamePlayManager.Instance.currentPlayer.GetComponent<BallController>().StartMovement(true);
	}
 }