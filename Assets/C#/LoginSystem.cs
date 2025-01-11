using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{
    public InputField userIdInput; // UserId用のInputField
    public InputField passwordInput; // Password用のInputField
    public Button loginButton;      // ログインボタン

    private string url = "https://requin.jp/FJB/PHP/login.php"; // ログイン用のPHPスクリプトのURL

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
            Debug.Log("UserId または Password が空です。");
            yield break;
        }

        // フォームデータの作成
        WWWForm form = new WWWForm();
        form.AddField("userid", userIdInput.text.Trim()); // ユーザー名のトリム
        form.AddField("password", passwordInput.text);   // パスワード

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Network Error: {www.error}");
            }
            else if (www.responseCode != 200)
            {
                Debug.LogError($"HTTP Error: {www.responseCode}");
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log($"Response from server: {responseText}");

                try
                {
                    var response = JsonUtility.FromJson<LoginResponse>(responseText);

                    if (response.status == "success")
                    {
                        PlayerPrefs.SetString("userid", userIdInput.text.Trim());
                        PlayerPrefs.SetString("UserLoggedIn", "login");
                        PlayerPrefs.Save();
                        SceneManager.LoadScene("Top");
                    }
                    else
                    {
                        Debug.Log($"Login failed: {response.message}");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Unexpected response format: {ex.Message}");
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
