using GameData.MyScripts;
using UnityEngine;
public class Experimental : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(!PlayerPrefsHandler.MakeLevelsEasy);
    }
}