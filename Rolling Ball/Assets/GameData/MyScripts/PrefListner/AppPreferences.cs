using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppPreferences  {

	private static AndroidJavaObject appPreferences;
	static AppPreferences()
	{
		appPreferences = null;
#if UNITY_EDITOR

#elif UNITY_ANDROID
		AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

		//appPreferences = new AndroidJavaObject("com.gamesaxis.pref.AppPreferences", activity);
		AndroidJavaClass pluginClass = new AndroidJavaClass("com.gamesaxis.pref.AppPreferences");		
		appPreferences = pluginClass.CallStatic<AndroidJavaObject>("getInstance", activity);

#elif UNITY_IPHONE

#else

#endif
    }

    public static string GetString(string key)
	{
		return GetString(key, "");
	}

	public static string GetString(string key, string value)
	{
#if UNITY_EDITOR
		return PlayerPrefs.GetString(key, value);
#elif UNITY_ANDROID
		return appPreferences.Call<string>("getString", key, value);
#elif UNITY_IPHONE
return PlayerPrefs.GetString(key, value);
#else
return PlayerPrefs.GetString(key, value);
#endif
	}
	public static int GetInt(string key)
	{
		return GetInt(key, 0);
	}
	public static int GetInt(string key, int value)
	{
#if UNITY_EDITOR
		return PlayerPrefs.GetInt(key, value);
#elif UNITY_ANDROID
		return appPreferences.Call<int>("getInt", key, value);
#elif UNITY_IPHONE
return PlayerPrefs.GetInt(key, value);
#else
return PlayerPrefs.GetInt(key, value);
#endif
	}

	public static float GetFloat(string key)
	{
		return GetFloat(key, 0f);
	}
	public static float GetFloat(string key, float value)
	{
#if UNITY_EDITOR
		return PlayerPrefs.GetFloat(key, value);
#elif UNITY_ANDROID
		return appPreferences.Call<float>("getFloat", key, value);
#elif UNITY_IPHONE
return PlayerPrefs.GetFloat(key, value);
#else
return PlayerPrefs.GetFloat(key, value);
#endif
	}
	public static void GetFloat(string key, string value)
	{
#if UNITY_EDITOR
		PlayerPrefs.SetString(key, value);
#elif UNITY_ANDROID
		appPreferences.Call("putString", key, value);
#elif UNITY_IPHONE
PlayerPrefs.SetString(key, value);
#else
PlayerPrefs.SetString(key, value);
#endif
	}
	
	public static bool HasKey(string key)
	{
#if UNITY_EDITOR
		return PlayerPrefs.HasKey(key);
#elif UNITY_ANDROID
		return appPreferences.Call<bool>("contains", key);
#elif UNITY_IPHONE
return PlayerPrefs.HasKey(key);
#else
return PlayerPrefs.HasKey(key);
#endif
	}
	public static void DeleteKey(string key)
	{
#if UNITY_EDITOR
		PlayerPrefs.DeleteKey(key);
#elif UNITY_ANDROID
		appPreferences.Call("remove", key);
#elif UNITY_IPHONE
PlayerPrefs.DeleteKey(key);
#else
PlayerPrefs.DeleteKey(key);
#endif
	}
	public static void DeleteAll()
	{
#if UNITY_EDITOR
		PlayerPrefs.DeleteAll();
#elif UNITY_ANDROID
		appPreferences.Call("clear");
#elif UNITY_IPHONE
PlayerPrefs.DeleteAll();
#else
PlayerPrefs.DeleteAll();
#endif
	}
	
	public static void SetString(string key, string value)
	{
#if UNITY_EDITOR
		PlayerPrefs.SetString(key, value);
#elif UNITY_ANDROID
		appPreferences.Call("putString", key, value);
#elif UNITY_IPHONE
PlayerPrefs.SetString(key, value);
#else
PlayerPrefs.SetString(key, value);
#endif
	}
	public static void SetInt(string key, int value)
	{
#if UNITY_EDITOR
		PlayerPrefs.SetInt(key, value);
#elif UNITY_ANDROID
		appPreferences.Call("putInt", key, value);
#elif UNITY_IPHONE
PlayerPrefs.SetInt(key, value);
#else
PlayerPrefs.SetInt(key, value);
#endif
	}
	public static void SetFloat(string key, float value)
	{
#if UNITY_EDITOR
		PlayerPrefs.SetFloat(key, value);
#elif UNITY_ANDROID
		appPreferences.Call("putFloat", key, value);
#elif UNITY_IPHONE
PlayerPrefs.SetFloat(key, value);
#else
PlayerPrefs.SetFloat(key, value);
#endif
	}
	
	public static void Save()
	{
#if UNITY_EDITOR
		PlayerPrefs.Save();
#elif UNITY_ANDROID
		appPreferences.Call("apply");
#elif UNITY_IPHONE
PlayerPrefs.Save();
#else
PlayerPrefs.Save();
#endif
	}
	
}
