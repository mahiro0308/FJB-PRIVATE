using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    public Button logoutButton; // ログアウトボタン
    private string logoutUrl = "http://localhost/logout.php"; // ログアウト用のPHPスクリプトのURL

    void Start()
    {
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
    }

    private void OnLogoutButtonClicked()
    {
        StartCoroutine(LogoutUser());
    }

    private IEnumerator LogoutUser()
    {
        // ログアウト処理中のメッセージ

        using (UnityWebRequest www = UnityWebRequest.Get(logoutUrl))
        {
            yield return www.SendWebRequest();

            // エラーチェック
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError ||
                www.result == UnityWebRequest.Result.DataProcessingError)
            {
            }
            else
            {
                Debug.Log("ログアウトエラー");
            }
        }
    }
}
