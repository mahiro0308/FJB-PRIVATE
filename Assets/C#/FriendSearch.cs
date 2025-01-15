using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FriendSearch : MonoBehaviour
{
    public InputField friendIdInputField; // フレンドID入力用
    public Button addFriendButton;       // フレンド追加ボタン
    private string serverUrl = "https://requin.jp/FJB/PHP/friendSearch.php"; // PHPのURL
    private string friendId;             // フレンドIDを一時保存する変数
    public Text friendIdDisplayText;     // 検索結果のフレンドID表示用

    [System.Serializable]
    public class FriendSearchResponse
    {
        public string status;
        public string message;
        public string UserId;
    }

    private string userid;

    private void Start()
    {
        userid = PlayerPrefs.GetString("username", ""); // デフォルト値を空文字列に設定
        addFriendButton.gameObject.SetActive(false); // フレンド追加ボタンを非表示に設定
        friendIdDisplayText.text = "";              // 初期状態でテキストを空にする
    }

    public void SearchFriend()
    {
        friendId = friendIdInputField.text.Trim();

        if (string.IsNullOrEmpty(friendId))
        {
            Debug.LogWarning("Friend ID is empty.");
            friendIdDisplayText.text = "Friend IDを入力してください。";
            return;
        }

        if (userid == friendId)
        {
            Debug.LogWarning("Cannot search for yourself.");
            friendIdDisplayText.text = "自分自身を追加することはできません。";
            return;
        }

        Debug.Log("Searching friend with ID: " + friendId); // デバッグ用出力
        StartCoroutine(SearchFriendOnServer(friendId));
    }

    private IEnumerator SearchFriendOnServer(string friendId)
    {
        WWWForm form = new WWWForm();
        form.AddField("friend_id", friendId);

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, form))
        {
            // タイムアウト設定（例: 30秒）
            request.timeout = 30;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error searching for friend: " + request.error);
                friendIdDisplayText.text = "エラーが発生しました。";
                addFriendButton.gameObject.SetActive(false);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Search result: " + responseText);

                try
                {
                    var jsonResponse = JsonUtility.FromJson<FriendSearchResponse>(responseText);

                    if (jsonResponse.status == "error")
                    {
                        Debug.Log("Friend not found.");
                        friendIdDisplayText.text = "フレンドが見つかりません。";
                        addFriendButton.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Friend found! You can proceed with the friend request.");
                        friendIdDisplayText.text = jsonResponse.UserId;
                        addFriendButton.gameObject.SetActive(true);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error parsing JSON response: " + ex.Message);
                    friendIdDisplayText.text = "無効な応答形式です。";
                    addFriendButton.gameObject.SetActive(false);
                }
            }
        }
    }

   
}
