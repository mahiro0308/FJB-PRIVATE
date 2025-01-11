using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FriendRequest : MonoBehaviour
{
    public Text receiverUserId;
    [SerializeField] private string serverUrl = "https://requin.jp/FJB/PHP/friendRequest.php"; // PHPのURL
    [SerializeField] private Text friendIdDisplayText; // フレンドリクエスト状態を表示するUI
    [SerializeField] private Button addFriendButton;   // フレンド追加ボタン

    // フレンド追加メソッド（コルーチンとして実行）
    public void AddFriend()
    {
        StartCoroutine(SendFriendRequestCoroutine());
    }

    private IEnumerator SendFriendRequestCoroutine()
    {
        string receiverId = receiverUserId.text;
        string requestUserid = PlayerPrefs.GetString("UserId", "");
        if (requestUserid == receiverId)
        {
            Debug.LogWarning("Cannot add yourself as a friend.");
            friendIdDisplayText.text = "自分自身を追加することはできません。";
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("receiverId", receiverId);
        form.AddField("requestUserId", requestUserid);

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, form))
        {
            // タイムアウト設定（例: 30秒）

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending friend request: " + request.error);
                friendIdDisplayText.text = "フレンドリクエスト送信中にエラーが発生しました。";
            }
            else
            {
                Debug.Log("Friend request sent successfully: " + request.downloadHandler.text);
                friendIdDisplayText.text = "フレンドリクエストが送信されました。";
                Debug.Log(receiverId);

                // ボタンを非表示にする
                addFriendButton.gameObject.SetActive(false);
            }
        }
    }
}
