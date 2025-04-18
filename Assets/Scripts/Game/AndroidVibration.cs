using UnityEngine;

public class AndroidVibration : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject vibrator;
    private static bool initialized = false;

    private static void Initialize()
    {
        if (!initialized)
        {
            try
            {
                unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                initialized = true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error initializing Android Vibration: " + e.Message);
                initialized = false;
            }
        }
    }

    public static void Vibrate(long milliseconds = 250)
    {
        if (IsAndroid())
        {
            Initialize(); // Initialize on first use
            if (vibrator != null)
            {
                vibrator.Call("vibrate", milliseconds);
            }
            else
            {
                Debug.LogWarning("Vibrator service not found.");
            }
        }
        else
        {
            Debug.LogWarning("Vibration not supported on this platform in this implementation.");
        }
    }

    public static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
#else
    public static void Vibrate(long milliseconds = 250)
    {
        Debug.Log("Vibration (simulated in editor)");
    }

    public static bool IsAndroid()
    {
        return false;
    }
#endif
}