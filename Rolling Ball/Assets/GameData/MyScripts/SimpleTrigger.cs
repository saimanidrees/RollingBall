using System.Collections.Generic;
using GameAnalyticsSDK;
using GameData.MyScripts;
using UnityEngine;
using UnityEngine.Events;
public class SimpleTrigger : MonoBehaviour
{
	public List<string> tagsToDetect = new List<string>() { "Player" };
	public LayerMask layerToDetect = 0 << 1;
	[SerializeField] private UnityEvent onTriggerEnter;
	[SerializeField] private UnityEvent onTriggerExit;
	private void OnTriggerEnter(Collider other)
	{
		if (!tagsToDetect.Contains(other.gameObject.tag) || !IsInLayerMask(other.gameObject, layerToDetect))
			return;
		_otherObject = other.gameObject;
		onTriggerEnter.Invoke();
	}
	private void OnTriggerExit(Collider other)
	{
		if (!tagsToDetect.Contains(other.gameObject.tag) || !IsInLayerMask(other.gameObject, layerToDetect))
			return;
		_otherObject = null;
		onTriggerExit.Invoke();
	}
	private static bool IsInLayerMask(GameObject obj, LayerMask mask)
	{
		return ((mask.value & (1 << obj.layer)) > 0);
	}
	private GameObject _otherObject;
	public void DisableOtherObject()
	{
		if(_otherObject)
			_otherObject.SetActive(false);
	}
	public void WatchVideoToPowerUp(string powerUpName)
	{
		if (AdsCaller.Instance.IsRewardedAdAvailable())
		{
			Callbacks.ADType = powerUpName;
			AdsCaller.Instance.ShowRewardedAd();
		}
		else
		{
			AdsCaller.Instance.ShowInterstitialAd();
			switch (powerUpName)
			{
				case "Magnet":
					GamePlayManager.Instance.RewardMagnet();
					break;
				case "Shield":
					GamePlayManager.Instance.RewardShield();
					break;
			}
			GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.Video, "Max", "InterstitialReward_" + powerUpName + "_" + 
			GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode));
			FirebaseManager.Instance.ReportEvent(GAAdAction.RewardReceived + "_" + GAAdType.RewardedVideo + "_MAX_InterstitialReward_" + powerUpName + "_" + 
			                                     GameManager.Instance.GetModeName(PlayerPrefsHandler.CurrentMode));
		}
		
	}
}