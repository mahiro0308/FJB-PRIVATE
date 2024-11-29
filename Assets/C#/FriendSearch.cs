using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class FriendSearch : MonoBehaviour
{
    public InputField friendIdInputField; // フレンドID入力用
    public Button addFriendButton; // フレンド追加ボタン
    private string serverUrl = "http://localhost/friendSearch.php"; // PHPのURL
    private string friendId; // フレンドIDを一時保存する変数
    public Text friendIdDisplayText; // 検索結果のフレンドID表示用（Text Legacy）

    [System.Serializable]
    public class FriendSearchResponse
    {
        public string status;
        public string message;
        public string UserId; // 必要なデータを追加
    }

    private void Start()
    {
        // フレンド追加ボタンを最初は非表示にしておく
        addFriendButton.gameObject.SetActive(false);
        friendIdDisplayText.text = ""; // 初期状態では空に設定
    }

    public void SearchFriend()
    {
        friendId = friendIdInputField.text;
        Debug.Log("Searching friend with ID: " + friendId); // デバッグ用出力
        StartCoroutine(SearchFriendOnServer(friendId));
    }

    private IEnumerator SearchFriendOnServer(string friendId)
    {
        WWWForm form = new WWWForm();
        form.AddField("friend_id", friendId);

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Search result: " + responseText);

                // レスポンスをJSONとして解析
                var jsonResponse = JsonUtility.FromJson<FriendSearchResponse>(responseText);

                if (jsonResponse.status == "error")
                {
                    Debug.Log("Friend not found.");
                    addFriendButton.gameObject.SetActive(false); // フレンドが見つからなかった場合、ボタンは非表示
                    friendIdDisplayText.text = ""; // 見つからなかった場合は空白
                }
                else
                {
                    Debug.Log("Friend found! You can proceed with the friend request.");
                    addFriendButton.gameObject.SetActive(true);
                    friendIdDisplayText.text = friendId;
                }
            }
            else
            {
                Debug.LogError("Error searching for friend: " + request.error);
            }
        }
    }

    // フレンド追加ボタンを押した時の処理
    public void OnAddFriend()
    {
        if (!string.IsNullOrEmpty(friendId))
        {
            StartCoroutine(SendFriendRequest(friendId));
        }
        else
        {
            Debug.LogError("Friend ID is empty.");
        }
    }

    private IEnumerator SendFriendRequest(string friendId)
    {
        WWWForm form = new WWWForm();
        form.AddField("friend_id", friendId);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/FriendRequest.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Friend request sent successfully: " + request.downloadHandler.text);
                addFriendButton.gameObject.SetActive(false); // 追加後にボタンを非表示
            }
            else
            {
                Debug.LogError("Error sending friend request: " + request.error);
            }
        }
    }
}
