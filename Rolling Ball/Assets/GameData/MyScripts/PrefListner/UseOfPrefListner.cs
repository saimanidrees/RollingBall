using UnityEngine;

namespace GamesAxis
{
    public class UseOfPrefListner : MonoBehaviour
    {

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            PrefListner.StartListening();


#if UNITY_EDITOR
            //
#elif UNITY_ANDROID
            SetHandler();
        Invoke("SetWaitTimer", 1f);
        Invoke("SetHandler", 2f);
        Invoke("SetHandler", 3f);
        //Invoke("SetHandler", 5f);//8
        //Invoke("SetWaitTimer", 8f);

         //Invoke("SetWaitTimer", 0f);
#elif UNITY_IPHONE
		//
#else
		//
#endif


            //sysInfo();
        }

        private void SetHandler()
        {
            PrefListner.SetHandler();
        }
        private void SetWaitTimer()
        {
            PrefListner.UpdateWaitTime(2300L);//2600L);
            SetHandler();
        }

    }
}