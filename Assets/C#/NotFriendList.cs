using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json; // NuGetからインポートする必要があります

public class NotFriendList : MonoBehaviour
{
    public GameObject TextPrefab; // フレンドリスト用のTextプレハブ
    public GameObject AcceptBtnPrefab; // 承認ボタン用のプレハブ
    public Transform ContentParent; // UI要素を配置する親オブジェクト

    private string serverUrl = "https://requin.jp/FJB/PHP/StillOthersList.php"; // PHPのURL

    void Start()
    {
        string username = PlayerPrefs.GetString("username", ""); // デフォルト値を空文字列に設定

        if (!string.IsNullOrEmpty(username))
        {
            StartCoroutine(GetNotFriendList(username));
            Debug.Log(username + "：ここまではきてる");
        }
        else
        {
            Debug.LogError("Username is empty!");
        }
    }

    private IEnumerator GetNotFriendList(string username)
    {
        // フォームデータを作成
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        // UnityWebRequestをPOSTで送信
        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // サーバーからのレスポンスを取得
                string jsonResponse = request.downloadHandler.text;
                Debug.Log(username + "：レスポンス受信 - " + jsonResponse);

                // JSONデータを解析
                HandleResponse(jsonResponse);
            }
            else
            {
                Debug.LogError("Error: " + request.error + "\nResponse Code: " + request.responseCode);
            }
        }
    }

    private void HandleResponse(string jsonResponse)
    {
        var response = JsonConvert.DeserializeObject<FriendListResponse>(jsonResponse);

        if (response == null || response.friends == null || response.friends.Count == 0)
        {
            Debug.Log("No friend requests.");
            return;
        }

        foreach (var friend in response.friends)
        {
            Debug.Log("フレンド情報取得: " + friend.RequestUserId);

            // Textプレハブが設定されているか確認
            if (TextPrefab == null || AcceptBtnPrefab == null || ContentParent == null)
            {
                Debug.LogError("Prefab or ContentParent is not set.");
                return;
            }

            // フレンド名を表示するTextを生成
            GameObject textObj = Instantiate(TextPrefab, ContentParent);
            Text textComponent = textObj.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = friend.RequestUserId;
            }
            else
            {
                Debug.LogError("TextPrefab is missing a Text component.");
            }

            // 承認ボタンを生成
            GameObject btnObj = Instantiate(AcceptBtnPrefab, ContentParent);
            Button buttonComponent = btnObj.GetComponent<Button>();
            if (buttonComponent != null)
            {
                string targetFriendId = friend.RequestUserId;

                buttonComponent.onClick.AddListener(() =>
                {
                    AcceptFriendRequest(targetFriendId);
                });
            }
            else
            {
                Debug.LogError("AcceptBtnPrefab is missing a Button component.");
            }
        }
    }

    private void AcceptFriendRequest(string friendId)
    {
        // フレンド承認処理の実装（サーバーにリクエストを送るなど）
        Debug.Log("Accepted friend request from: " + friendId);
    }

    // JSONレスポンス用クラス
    [System.Serializable]
    private class FriendListResponse
    {
        public string status;
        public List<FriendRequest> friends;
    }

    [System.Serializable]
    private class FriendRequest
    {
        [JsonProperty("RequestUserId")] // サーバーのキー名に合わせる
        public string RequestUserId;
    }
}
