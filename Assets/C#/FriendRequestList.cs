using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FriendList : MonoBehaviour
{
    public GameObject textPrefab;  // プレハブとして設定されたTextオブジェクト
    public Transform parentTransform;  // Textオブジェクトを配置する親Transform

    private void Start()
    {
        StartCoroutine(GetFriendData());
    }

    IEnumerator GetFriendData()
    {
        string url = "http://localhost/getFriendRequestList.php";  // PHPスクリプトのURLに置き換えてください

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        // リクエストの送信
        yield return request.SendWebRequest();

        // エラーチェック
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            CreateText("Error: " + request.error);
        }
        else
        {
            // レスポンスデータの解析
            string jsonResponse = request.downloadHandler.text;

            // JSONデータをFriendListDataクラスにデシリアライズ
            FriendListData friendListData = JsonUtility.FromJson<FriendListData>(jsonResponse);

            // 結果に基づいて表示を更新
            if (friendListData.status == "success")
            {
                // フレンド情報の数だけTextオブジェクトを作成
                foreach (var friend in friendListData.friends)
                {
                    string displayText = $"User ID: {friend.user_id}\nFriend ID: {friend.friend_id}\nStatus: {friend.friend_status}";
                    CreateText(displayText);
                }
            }
            else
            {
                CreateText("Error: " + friendListData.message);
            }
        }
    }

    // 新しいTextオブジェクトを作成して親Transformに追加する
    void CreateText(string message)
    {
        GameObject newText = Instantiate(textPrefab, parentTransform);
        newText.GetComponent<Text>().text = message;
    }
}

// PHPからのレスポンスを扱うためのクラス
[System.Serializable]
public class Friend
{
    public string user_id;
    public string friend_id;
    public string friend_status;
}

// FriendListDataクラス
[System.Serializable]
public class FriendListDataaa
{
    public string status;
    public string message;
    public Friend[] friends;  // 複数のFriendオブジェクトを格納する配列
}
