using System.Collections;
using System.Linq;
using GameData.MyScripts;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class AllMenus
{
    public string name;
    public GameObject menu;
}
public class SharedUI : MonoBehaviour
{
    #region Properties
    [ReadOnly] public GamePlayUIManager gamePlayUIManager;
    [Space]
    [SerializeField] private AllMenus[] allMenus;
    private int sceneIndexToOpen = 1;
    [Header("Links")]
    public string privacyPolicyLink = "https://worldofanimal1234.blogspot.com/2022/02/world-of-animals.html";
    public string moreGamesLink = "https://play.google.com/store/apps/dev?id=6407456443209899378";
    #endregion
    public static SharedUI Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SwitchMenu(PlayerPrefsHandler.Loading);
        HideAllSubMenus();
    }
    public void GameplayUIActivated(GamePlayUIManager m)
    {
        gamePlayUIManager = m;
    }
    private void GetReference()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            var g = FindObjectOfType<GamePlayUIManager>();
            GameplayUIActivated(g);
        }
    }
    public void SwitchMenu(string menuToShow)
    {
        if(!gamePlayUIManager)
            GetReference();
        if (GetActiveMenu(menuToShow))
        {
            Debug.Log(menuToShow);
            return;
        }
        HideAll();
        GameObject menu = null;
        if (gamePlayUIManager)
        {
            menu = gamePlayUIManager.GetMenu(menuToShow);
            if (menu)
            {

                menu.SetActive(true);
                return;
            }
        }
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
    private GameObject GetMenu(string menuName)
    {
        foreach (var t in allMenus)
        {
            if (t.name == menuName)
                return t.menu;
        }
        return null;
    }
    public void HideAll()
    {
        if (gamePlayUIManager)
        {
            gamePlayUIManager.HideAll();
        }
        foreach (var t in allMenus)
        {
            t.menu.SetActive(false);
        }
    }
    public bool GetActiveMenu(string menuName)
    {
        if (gamePlayUIManager)
        {
            if (gamePlayUIManager.GetActiveMenu(menuName))
            {
                return true;
            }
        }
        return allMenus.Where(t => t.menu.gameObject.activeSelf).Any(t => menuName == t.name);
    }
    public void SubMenu(string menuToShow)
    {
        HideAllSubMenus();
        GameObject menu = null;
        if (gamePlayUIManager)
        {
            menu = gamePlayUIManager.GetSubMenu(menuToShow);
            if (menu)
            {
                menu.SetActive(true);
                return;
            }
        }
    }
    private void HideAllSubMenus()
    {
        if (gamePlayUIManager)
        {
            gamePlayUIManager.HideAllSubMenus();
        }
    }
    public void CloseSubMenu()
    {
        HideAllSubMenus();
    }
    public void SetNextSceneIndex(int sceneIndex)
    {
        sceneIndexToOpen = sceneIndex;
    }
    public void SetNextSceneIndex(string sceneName)
    {
        switch (sceneName)
        {
            case PlayerPrefsHandler.Splash:
                sceneIndexToOpen = 0;
                break;
            case PlayerPrefsHandler.GamePlay:
                sceneIndexToOpen = 1;
                break;
        }
    }
    private int GetNextSceneIndex()
    {
        return sceneIndexToOpen;
    }
    public void SwitchScene()
    {
        StartCoroutine(DelayToSwitchScene());
    }
    private IEnumerator DelayToSwitchScene()
    {
        //SwitchMenu(PlayerPrefsHandler.Loading);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(GetNextSceneIndex());
    }
}