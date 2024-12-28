using UnityEngine;

public class AndroidTTS : MonoBehaviour
{
    AndroidJavaObject tts;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // AndroidÇÃTextToSpeechÇèâä˙âª
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                tts = new AndroidJavaObject("android.speech.tts.TextToSpeech", activity, null);
                tts.Call("setLanguage", new AndroidJavaObject("java.util.Locale", "en", "US"));
            }
        }
    }

    public void Speak(string text)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            tts.Call("speak", text, 0, null, null);
        }
        else
        {
            Debug.LogWarning("TTS is not supported on this platform.");
        }
    }

    private void OnDestroy()
    {
        if (tts != null)
        {
            tts.Call("shutdown");
            tts = null;
        }
    }
}
