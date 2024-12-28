using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{
    public InputField userIdInput; // UserId用のInputField
    public InputField passwordInput; // Password用のInputField
    public Button loginButton;      // ログインボタン
    public Text resultText;         // 結果を表示するText

    private string url = "http://localhost/login.php"; // ログイン用のPHPスクリプトのURL

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        StartCoroutine(LoginUser());
    }

    private IEnumerator LoginUser()
    {
        // 入力値があるか確認
        if (string.IsNullOrEmpty(userIdInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            resultText.text = "User IDとPasswordを入力してください";
            yield break;
        }

        // フォームデータの作成
        WWWForm form = new WWWForm();
        form.AddField("UserId", userIdInput.text); // UserIdを追加
        form.AddField("password", passwordInput.text); // Passwordを追加

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                resultText.text = "Error: " + www.error;
            }
            else
            {
                // サーバーからの応答を解析
                string responseText = www.downloadHandler.text;

                // JSONのパースと応答チェック
                try
                {
                    var response = JsonUtility.FromJson<LoginResponse>(responseText);

                    if (response.status == "ログイン")
                    {
                        resultText.text = "Login successful!";
                        // ログイン成功時の処理（必要に応じて画面遷移など）
                    }
                    else
                    {
                        resultText.text = "Login failed: " + response.message;
                    }
                }
                catch
                {
                    resultText.text = "Unexpected response format";
                }
            }
        }
    }
}

// サーバーからの応答用のクラス
[System.Serializable]
public class LoginResponse
{
    public string status;
    public string message;
}
