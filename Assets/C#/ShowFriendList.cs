using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ShowFriendList : MonoBehaviour
{
    public Text friendInfoText;  // PHPからのデータを表示するTextコンポーネント

    private void Start()
    {
        StartCoroutine(FriendInfo());
    }

    IEnumerator FriendInfo()
    {
        string url = "http://localhost/getFriendData.php";  // PHPスクリプトのURLに置き換えてください

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        // リクエストの送信
        yield return request.SendWebRequest();

        // エラーチェック
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            friendInfoText.text = "Error: " + request.error;
        }
        else
        {
            // レスポンスデータの解析
            string jsonResponse = request.downloadHandler.text;

            // JSONデータが空でないことを確認
            if (!string.IsNullOrEmpty(jsonResponse))
            {
                // JSONデータをFriendListDataクラスにデシリアライズ
                FriendListData friendList = JsonUtility.FromJson<FriendListData>(jsonResponse);

                // 結果に基づいて表示を更新
                if (friendList.status == "success")
                {
                    // フレンド情報を表示
                    string friendDetails = "";
                    foreach (var friend in friendList.friends)
                    {
                        friendDetails += $"User ID: {friend.user_id}, Friend ID: {friend.friend_id}, Status: {friend.friend_status}\n";
                    }
                    friendInfoText.text = friendDetails;
                }
                else
                {
                    friendInfoText.text = "Error: " + friendList.message;
                }
            }
            else
            {
                friendInfoText.text = "Error: Received empty response.";
            }
        }
    }
}

// PHPからのレスポンスを扱うためのクラス
[System.Serializable]
public class FriendInfo
{
    public string user_id;
    public string friend_id;
    public string friend_status;
}

[System.Serializable]
public class FriendListData  // クラス名を変更
{
    public string status;
    public string message;
    public FriendInfo[] friends;  // 配列でフレンド情報を保持
}
