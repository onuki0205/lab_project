using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WiFiTester : MonoBehaviour
{
    public GameObject obj = null;
#if !UNITY_EDITOR && UNITY_ANDROID
    private static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
#endif


    private void Awake()
    {
        Debug.Log("Awake");
    }

    private void Start()
    {
        Debug.Log("Start");
    }

    public void OnClick(){
#if !UNITY_EDITOR && UNITY_ANDROID
    private static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
#endif
        Text text = obj.GetComponent<Text> ();
        var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
        text.text = wifiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getSSID");
        // string str = "Wifi Get SSID is" + ssid;
        // Debug.Log(str);
        // string str = "Clicked";
        // text.text = str;
    }
}