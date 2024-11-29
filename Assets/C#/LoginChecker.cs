using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;  // JSON解析用に追加（Newtonsoft.JsonをUnityにインポートが必要）

public class LoginChecker : MonoBehaviour
{
    private string url = "http://localhost/logincheck.php";

    void Start()
    {
        StartCoroutine(CheckLoginStatus());
    }

    private IEnumerator CheckLoginStatus()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string response = request.downloadHandler.text;

                // JSON解析
                var json = JObject.Parse(response);
                string message = (string)json["message"];

                // "ログイン" と返ってきた場合にシーンを遷移
                if (message == "ログイン")
                {
                   Debug.Log("ログイン");

                }
                else
                {
                    SceneManager.LoadScene("login-page");  // 次のシーンに遷移 
                }
            }
        }
    }
}
