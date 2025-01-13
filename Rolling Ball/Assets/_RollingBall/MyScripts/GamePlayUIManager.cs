using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class GamePlayUIManager : MonoBehaviour
    {
        [System.Serializable]
        public class Menu
        {
            [Required]
            public string name;
            [Space]
            [SceneObjectsOnly, Required]
            public GameObject menu;
        }
    
        #region Properties
    
        [SerializeField] private Menu[] allMenus;
        [SerializeField] private Menu[] subMenus;
        [SerializeField] private Menu[] specialMenus;
        [SerializeField] private BallLivesUI ballLivesUI;

        #endregion
    
        public void SwitchMenu(string menuToShow)
        {
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
            return allMenus.Where(t => t.menu.gameObject.activeSelf).Any(t => menuName == t.name);
        }
        private void HideAll()
        {
            foreach (var t in allMenus)
            {
                t.menu.SetActive(false);
            }
        }
        private GameObject GetMenu(string menuName)
        {
            return (from t in allMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
        }
        public void CloseMenu(string menuToClose)
        {
            foreach (var t in allMenus)
            {
                if (!t.name.Equals(menuToClose)) continue;
                t.menu.SetActive(false);
            }
        }
        public void SubMenu(string menuToShow)
        {
            HideAllSubMenus();
            foreach (var t in subMenus)
            {
                if (!t.name.Equals(menuToShow)) continue;
                t.menu.SetActive(true);
                break;
            }
        }
        public GameObject GetSubMenu(string menuName)
        {
            return (from t in subMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
        }
        private void HideAllSubMenus()
        {
            foreach (var t in subMenus)
            {
                t.menu.SetActive(false);
            }
        }
        public void CloseSubMenu()
        {
            HideAllSubMenus();
        }
        private bool GetActiveSubMenu(string subMenuName)
        {
            return subMenus.Where(tempSubMenu => tempSubMenu.name == subMenuName).Any(tempSubMenu => tempSubMenu.menu.gameObject.activeSelf);
        }
        public void OpenSpecialMenu(string menuToShow)
        {
            GameObject menu = null;
            menu = GetSpecialMenu(menuToShow);
            if (menu)
            {
                menu.SetActive(true);
                return;
            }
            else
            {
                Debug.Log("No Special Menu Found! " + menuToShow);
            }
        }
        private GameObject GetSpecialMenu(string menuName)
        {
            return (from t in specialMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
        }
        public void CloseSpecialMenu(string menuToClose)
        {
            foreach (var t in specialMenus)
            {
                if (!t.name.Equals(menuToClose)) continue;
                t.menu.SetActive(false);
            }
        }
        public BallLivesUI GetBallLivesUI()
        {
            return ballLivesUI;
        }
    }
}