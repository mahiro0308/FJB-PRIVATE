using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CheckLoginStatus : MonoBehaviour
{
    // ログインチェック用のURL
    private string checkLoginUrl = "https://requin.jp/FJB/PHP/registrate.php";

    public InputField emailInputField;
    public InputField useridInputField;
    public InputField passwordInputField;

    // ボタンが押されたときに呼ばれるメソッド
    public void OnLoginButtonClicked()
    {
        string email = emailInputField.text;
        string userid = useridInputField.text;
        string password = passwordInputField.text;

        // 入力された内容が空でないことを確認
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Please fill in all fields.");
            return;
        }

        StartCoroutine(CheckLogin(email, userid, password));
    }

    // サーバーにリクエストを送信し、レスポンスを処理する
    private IEnumerator CheckLogin(string email, string userid, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("userid", userid);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(checkLoginUrl, form);
        Debug.Log("" + www.result);

        yield return www.SendWebRequest();


        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Raw Response: " + www.downloadHandler.text);

            try
            {
                // JSONレスポンスをパース
                Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);

                if (response.status == "success")
                {
                    Debug.Log("Registration successful: " + response.message);
                    PlayerPrefs.SetString("userid", userid);
                    PlayerPrefs.SetString("UserLoggedIn", "login"); // ログインフラグを保存
                    PlayerPrefs.Save();
                    // 必要に応じて画面遷移などを行う
                    SceneManager.LoadScene("Top"); // ホーム画面に遷移
                }
                else
                {
                    Debug.LogError("Registration failed: " + response.message);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to parse JSON: " + e.Message);
            }
        }
        else
        {
            Debug.LogError($"Network Error: {www.error}, Response: {www.downloadHandler.text}");
        }
    }

    [System.Serializable]
    public class Response
    {
        public string status;
        public string message;
    }
}
