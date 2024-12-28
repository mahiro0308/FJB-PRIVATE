using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SessionCheck : MonoBehaviour
{
    public Text sessionStatusText; // UIに表示するText

    private string url = "http://localhost/logincheck.php"; // PHPスクリプトのURL

    void Start()
    {
        StartCoroutine(CheckSession());
    }

    private IEnumerator CheckSession()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                sessionStatusText.text = "Error: " + www.error;
            }
            else
            {
                // JSONデータをデシリアライズ
                string jsonResponse = www.downloadHandler.text;
                SessionResponse response = JsonUtility.FromJson<SessionResponse>(jsonResponse);

                // メッセージをUIに表示
                sessionStatusText.text = response.message;

                // メッセージに応じて画面遷移
                if (response.message == "ログイン") // ここを適切なメッセージに変更
                {
                    ChangeScene("Top"); // "Top"シーンに遷移
                }
                else
                {
                    ChangeScene("Register"); // "Register"シーンに遷移
                }
            }
        }
    }

    // シーン遷移のためのメソッド
    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

// JSONデータをマッピングするためのクラス
[System.Serializable]
public class SessionResponse
{
    public string message;
}
