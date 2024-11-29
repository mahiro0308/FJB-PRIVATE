using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class CameraCapture : MonoBehaviour
{
    public Camera cam;   // Unityのカメラオブジェクト
    public Text resultText;  // 識別結果を表示するTextコンポーネント

    void Start()
    {
        // 一定間隔で画像をキャプチャしてサーバーに送信するコルーチンを開始
        StartCoroutine(CaptureAndSendImage());
    }

    IEnumerator CaptureAndSendImage()
    {
        while (true)
        {
            // カメラの映像をテクスチャにキャプチャ
            RenderTexture renderTexture = new RenderTexture(256, 256, 24);
            cam.targetTexture = renderTexture;
            Texture2D screenshot = new Texture2D(256, 256, TextureFormat.RGB24, false);
            cam.Render();
            RenderTexture.active = renderTexture;
            screenshot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            // 画像をPNGにエンコード
            byte[] bytes = screenshot.EncodeToPNG();
            Destroy(screenshot);

            // サーバーに画像を送信して結果を取得
            yield return StartCoroutine(Upload(bytes));

            // 次の画像を送信するまで0.5秒待機
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Upload(byte[] bytes)
    {
        // FlaskサーバーにHTTP POSTリクエストで画像データを送信
        UnityWebRequest www = new UnityWebRequest("http://localhost:5000/predict", "POST");
        www.uploadHandler = new UploadHandlerRaw(bytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/octet-stream");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
            resultText.text = "Error: " + www.error;  // エラー内容を表示
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);  // サーバーからの応答を表示
            resultText.text = www.downloadHandler.text;  // 識別結果をTextフィールドに表示
        }
    }
}
