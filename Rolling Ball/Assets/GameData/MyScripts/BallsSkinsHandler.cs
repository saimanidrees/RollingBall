using GameData.MyScripts;
using UnityEngine;
public class BallsSkinsHandler : MonoBehaviour
{
    [SerializeField] private Transform[] ballsSkins;
    private int _ballSkinIndex = -1;
    public void ApplySkin()
    {
        GamePlayManager.Instance.isBallUnlocked[_ballSkinIndex] = true;
        HideAllSkins();
        foreach (var t in ballsSkins)
        {
            t.transform.GetChild(_ballSkinIndex).gameObject.SetActive(true);
        }
    }
    public void ApplySkin(int skinIndex)
    {
        GamePlayManager.Instance.isBallUnlocked[skinIndex] = true;
        HideAllSkins();
        foreach (var t in ballsSkins)
        {
            t.transform.GetChild(skinIndex).gameObject.SetActive(true);
        }
    }
    private void HideAllSkins()
    {
        foreach (var t in ballsSkins)
        {
            for (var j = 0; j < t.childCount; j++)
            {
                t.GetChild(j).gameObject.SetActive(false);
            }
        }
    }
    public void SetSkinIndex(int index)
    {
        _ballSkinIndex = index;
    }
    public int GetSkinIndex()
    {
        return _ballSkinIndex;
    }
}