using UnityEngine;

namespace _RollingBall.MyScripts
{
    public class VibrationManager : MonoBehaviour
    {
        private void Start()
        {
            Vibration.Init();
        }

        public void TapVibrate()
        {
            if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Vibration)) return;
            Vibration.Vibrate();
        }

        public void TapPopVibrate()
        {
            if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Vibration)) return;
            Vibration.VibratePop();
        }

        public void TapPeekVibrate()
        {
            if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Vibration)) return;
            Vibration.VibratePeek();
        }

        public void TapNopeVibrate()
        {
            if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Vibration)) return;
            Vibration.VibrateNope();
        }

        public void TapVibrateCustom(int duration)
        {
#if UNITY_ANDROID
            Vibration.VibrateAndroid(duration);
#endif
        }
    }
}