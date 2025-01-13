using GameData.MyScripts;
using UnityEngine;
public class RateUsInvoker : MonoBehaviour
{
	[SerializeField] private GameObject rateUsPanel;
	private void OnEnable()
    {
	    if (PlayerPrefsHandler.GetBool("RateUs")) return;
	    if(!GameManager.Instance.IsBallMergeMode()) return;
	    Debug.Log("PlayerPrefsHandler.LevelsCounter: " + PlayerPrefsHandler.LevelsCounter);
	    if(PlayerPrefsHandler.LevelsCounter == 0) return;
	    if (PlayerPrefsHandler.LevelsCounter % 5 == 0)
	    {
			rateUsPanel.SetActive(true);
	    }
    }
}