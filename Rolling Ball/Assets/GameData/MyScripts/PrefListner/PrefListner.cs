using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//PrefListner.StartListening();
namespace GamesAxis
{
    public class PrefListner
    {

        private static string appId = "1";
        private static string getLoadedLevel()
        {
            return "-2";
        }
        private static string getUnlockedLevels()
        {
            return "-2";
        }

        private static AndroidJavaObject unityInitializer;
        private static AndroidJavaObject mActivity;
        static PrefListner()
        {
            unityInitializer = null;
#if UNITY_EDITOR

#elif UNITY_ANDROID
		AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mActivity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

		//unityInitializer = new AndroidJavaObject("com.gamesaxis.UnityInitializer", mActivity);
		AndroidJavaClass pluginClass = new AndroidJavaClass("com.gamesaxis.UnityInitializer");		
		unityInitializer = pluginClass.CallStatic<AndroidJavaObject>("getInstance", mActivity);

		if (AppPreferences.GetInt("isSavedStatistis", 0) == 0)
		{
		SaveStatistics();
		AppPreferences.SetInt("isSavedStatistis", 1);
		AppPreferences.Save();
		}		
#elif UNITY_IPHONE

#else

#endif

        }

        public static void StartListening()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
		if(unityInitializer != null){
		//unityInitializer.Call("StartListening", SceneManager.GetActiveScene().name, getLoadedLevel(), getUnlockedLevels());

		unityInitializer.Call("CurrentLoadedScene",mActivity, SceneManager.GetActiveScene().name, getLoadedLevel(), getUnlockedLevels());
		}
#elif UNITY_IPHONE

#else

#endif

            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            //Application.logMessageReceived += HandleLogEntry;
        }

        public static void SetHandler()
        {

#if UNITY_EDITOR
            //
#elif UNITY_ANDROID
			unityInitializer.Call("SetHandler", mActivity);
#elif UNITY_IPHONE
		//
#else
		//
#endif
        }

        public static void UpdateWaitTime(long waitTime)
        {

#if UNITY_EDITOR
            //
#elif UNITY_ANDROID
        unityInitializer.Call("updateWaitTime", waitTime);
#elif UNITY_IPHONE
		//
#else
		//
#endif

        }

        private static void SaveStatistics()
        {

#if UNITY_EDITOR
            //
#elif UNITY_ANDROID
        /*
      unityInitializer.Call("SaveStatistics", SystemInfo.deviceUniqueIdentifier, appId, 
				SystemInfo.systemMemorySize, SystemInfo.processorCount, SystemInfo.processorType,
				Screen.currentResolution.width, Screen.currentResolution.height, Screen.dpi,
				SystemInfo.graphicsDeviceName, SystemInfo.graphicsDeviceVendor, SystemInfo.graphicsMemorySize, SystemInfo.maxTextureSize);
        */
#elif UNITY_IPHONE
		//
#else
		//
#endif

        }

        //public static void QuitAfterTime(int timeToQuit, bool isFocus)
        //    {
        //#if UNITY_EDITOR

        //#elif UNITY_ANDROID
        //		if(unityInitializer != null){
        //		 unityInitializer.Call("QuitAfterTime", timeToQuit, isFocus);
        //		}
        //#elif UNITY_IPHONE

        //#else

        //#endif

        //    }

        static void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log(scene.name);
            //Debug.Log(mode);
#if UNITY_EDITOR

#elif UNITY_ANDROID
		unityInitializer.Call("CurrentLoadedScene", mActivity, SceneManager.GetActiveScene().name, getLoadedLevel(), getUnlockedLevels());		
#elif UNITY_IPHONE

#else

#endif
        }

        public static void ReadyToClose(long delay)
        {

#if UNITY_EDITOR
            //
#elif UNITY_ANDROID
            unityInitializer.Call("ReadyToClose", mActivity, delay);
#elif UNITY_IPHONE
		//
#else
		//
#endif

        }
        public static void CancelToClose()
        {

#if UNITY_EDITOR
            //
#elif UNITY_ANDROID
            unityInitializer.Call("CancelToClose", mActivity);
#elif UNITY_IPHONE
		//
#else
		//
#endif

        }

    }
}