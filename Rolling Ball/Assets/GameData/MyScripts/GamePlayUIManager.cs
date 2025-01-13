using System;
using System.Linq;
using GameData.MyScripts;
using UnityEngine;
public class GamePlayUIManager : MonoBehaviour
{
    #region Properties
    
    [SerializeField] private Menu[] allMenus;
    [SerializeField] private Menu[] subMenus;
    [HideInInspector] public Controls controls;
    
    #endregion
    private void Awake()
    {
        HideAll();
        controls = GetMenu(PlayerPrefsHandler.HUD).GetComponent<Controls>();
    }
    public void SwitchMenu(string menuToShow)
    {
        if (GetActiveMenu(menuToShow))
        {
            Debug.Log(menuToShow);
            return;
        }
        HideAll();
        GameObject menu = null;
        menu = GetMenu(menuToShow);
        if (menu)
        {
            menu.SetActive(true);
            return;
        }
        else
        {
            Debug.Log("No Menu Found! " + menuToShow);
        }
    }
    public bool GetActiveMenu(string menuName)
    {
        return allMenus.Where(t => t.menuObject.gameObject.activeSelf).Any(t => menuName == t.menuName);
    }
    public void HideAll()
    {
        foreach (var t in allMenus)
        {
            t.menuObject.SetActive(false);
        }
    }
    public GameObject GetMenu(string menuName)
    {
        return (from t in allMenus where t.menuName.Equals(menuName) select t.menuObject).FirstOrDefault();
    }
    public void CloseMenu(string menuToClose)
    {
        foreach (var t in allMenus)
        {
            if (!t.menuName.Equals(menuToClose)) continue;
            t.menuObject.SetActive(false);
        }
    }
    public void SubMenu(string menuToShow)
    {
        HideAllSubMenus();
        GameObject menu = null;
        menu = GetSubMenu(menuToShow);
        if (!menu) return;
        menu.SetActive(true);
        SoundController.Instance.PlayPopupSound();
    }
    public GameObject GetSubMenu(string menuName)
    {
        return (from t in subMenus where t.menuName.Equals(menuName) select t.menuObject).FirstOrDefault();
    }
    public void HideAllSubMenus()
    {
        foreach (var t in subMenus)
        {
            t.menuObject.SetActive(false);
        }
    }
    public void StartMode(string modeName)
    {
        AdsCaller.Instance.ShowInterstitialAd();
        SoundController.Instance.PlayBtnClickSound();
        PlayerPrefsHandler.CurrentMode = GameManager.Instance.GetModeNo(modeName);
        GameManager.Instance.StartMode(modeName);
    }
    public void PlayRollingBall()
    {
        SoundController.Instance.PlayBtnClickSound();
        PlayerPrefsHandler.CurrentMode = GameManager.Instance.GetModeNo(PlayerPrefsHandler.RollingBallMode);
        GameManager.Instance.StartMode(PlayerPrefsHandler.RollingBallMode);
    }
    [Serializable]
    private class Menu
    {
        public string menuName;
        public GameObject menuObject;
    }
}