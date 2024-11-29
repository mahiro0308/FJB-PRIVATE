using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FriendAccept : MonoBehaviour
{
    // ボタンがクリックされたときに呼ばれるメソッド
    public void OnAcceptButtonClick()
    {
        // PHPにリクエストを送信するコルーチンを開始
        StartCoroutine(SendAcceptRequest());
    }

    // PHPにacceptデータを送信するコルーチン
    IEnumerator SendAcceptRequest()
    {
        // PHPスクリプトのURL
        string url = "http://localhost/friendAccept-program.php";  // 適切なURLに置き換えてください

        // フォームデータを作成
        WWWForm form = new WWWForm();
        form.AddField("status", "accept"); // "status"フィールドに"accept"を追加

        // POSTリクエストを作成
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // リクエストの送信
            yield return www.SendWebRequest();

            // エラーチェック
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Request sent successfully: " + www.downloadHandler.text);
            }
        }
    }
}
