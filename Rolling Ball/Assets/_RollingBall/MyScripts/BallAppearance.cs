using System;
using System.Linq;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class BallAppearance : MonoBehaviour
    {
        [SerializeField] private BallSkin[] ballSkins;
        private BallSkinName _currentSkinName = BallSkinName.AncientBall;
        private void Start()
        {
            ApplySkin(PlayerPrefsHandler.BallSkinNo);
        }
        public void SetNextSkin()
        {
            if (PlayerPrefsHandler.BallSkinNo < 5)
                PlayerPrefsHandler.BallSkinNo += 1;
            else
                PlayerPrefsHandler.BallSkinNo = 0;
        }
        public void ApplySkin(int ballSkinNo)
        {
            var skinToApply = GetBallSkinName(ballSkinNo);
            var skin = GetBallSkinMesh(_currentSkinName);
            if(skin != null)
                skin.SetActive(false);
            foreach (var ballSkin in ballSkins)
            {
                if (!ballSkin.skinName.Equals(skinToApply)) continue;
                ballSkin.skinMesh.SetActive(true);
                _currentSkinName = skinToApply;
            }
        }
        private BallSkinName GetBallSkinName(int index)
        {
            return (BallSkinName) index;
        }
        private GameObject GetBallSkinMesh(BallSkinName skinName)
        {
            return (from ballSkin in ballSkins where skinName.Equals(ballSkin.skinName) select ballSkin.skinMesh).FirstOrDefault();
        }
    }
    public enum BallSkinName
    {
        AncientBall = 0,
        FrameBall = 1,
        ArmourBall = 2,
        WoodenBall = 3,
        SpikeBall = 4,
        MarbleBall = 5
    }
    [Serializable]
    public class BallSkin
    {
        public BallSkinName skinName;
        public GameObject skinMesh;
    }
}