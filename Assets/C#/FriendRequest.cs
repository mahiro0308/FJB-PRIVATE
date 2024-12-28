using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FriendListManager : MonoBehaviour
{
    public Text friendListText; // UIにフレンドリストを表示するためのText
    private string serverUrl = "http://localhost/getFriendList.php"; // フレンドリストを取得するPHPのURL
    private string userId = "1"; // 仮のユーザーID（実際にはログインしたユーザーIDを使う）

    private void Start()
    {
        // フレンドリストを取得
        StartCoroutine(GetFriendList(userId));
    }

    private IEnumerator GetFriendList(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", userId);

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Friend list received: " + responseText);

                // 受け取ったフレンドリストの処理（カンマ区切りでIDが返されていると仮定）
                string[] friendIds = responseText.Split(',');

                // フレンドリストをUIに表示
                friendListText.text = "Friend List:\n";
                foreach (var friendId in friendIds)
                {
                    friendListText.text += "ID: " + friendId + "\n";
                }
            }
            else
            {
                Debug.LogError("Error fetching friend list: " + request.error);
            }
        }
    }
}
