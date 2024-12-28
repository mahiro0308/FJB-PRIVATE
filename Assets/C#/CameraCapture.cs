using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class CameraCapture : MonoBehaviour
{
    public static string ClassName { get; private set; } // 他スクリプトからアクセス可能なプロパティ

    private class BypassCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    public Camera cam;
    public Text resultText;
    public Button scoreButton;

    private const string serverUrl = "https://172.20.10.4:8888/predict";

    void Start()
    {
        scoreButton.gameObject.SetActive(false);
        scoreButton.onClick.AddListener(OnScoreButtonClick);
        StartCoroutine(CaptureAndSendImage());
    }

    IEnumerator CaptureAndSendImage()
    {
        while (true)
        {
            RenderTexture renderTexture = new RenderTexture(256, 256, 24);
            cam.targetTexture = renderTexture;

            Texture2D screenshot = new Texture2D(256, 256, TextureFormat.RGB24, false);

            RenderTexture.active = renderTexture;
            cam.Render();

            screenshot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
            screenshot.Apply();

            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            byte[] bytes = screenshot.EncodeToPNG();
            Destroy(screenshot);

            yield return StartCoroutine(Upload(bytes));
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator Upload(byte[] bytes)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(serverUrl, bytes))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/octet-stream");
            www.certificateHandler = new BypassCertificateHandler();

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
                resultText.text = "Error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                try
                {
                    var json = JsonUtility.FromJson<ResponseData>(responseText);

                    if (json.confidence >= 90.0f)
                    {
                        ClassName = json.class_name; // 静的プロパティに値を保存
                        scoreButton.gameObject.SetActive(true);
                        SetButtonText(json.class_name);
                        resultText.text = json.confidence.ToString("F2");
                    }
                    else if (json.confidence < 90.0f && json.confidence >= 50.0f)
                    {
                        resultText.text = ("あと少しです...\n" + json.confidence.ToString("F2"));
                    }
                    else if (json.confidence < 50.0f)
                    {
                        resultText.text = ("読み取っています...\n" + json.confidence.ToString("F2"));
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("JSON Parse Error: " + ex.Message);
                    resultText.text = "Error: Could not parse response.";
                }
            }
        }
    }

    private void OnScoreButtonClick()
    {
        SceneManager.LoadScene("apst");
    }

    private void SetButtonText(string text)
    {
        Text buttonText = scoreButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = text;
        }
        else
        {
            Debug.LogError("Button Text component not found!");
        }
    }

    [System.Serializable]
    private class ResponseData
    {
        public string class_name;
        public float confidence;
    }
}
