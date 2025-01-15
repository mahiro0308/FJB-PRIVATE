using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class FriendData
{
    public int id;
    public string name;
}

[System.Serializable]
public class FriendList
{
    public List<FriendData> friends;
}

public class FriendListManager : MonoBehaviour
{
    public GameObject buttonPrefab;  // ボタンのプレハブ
    public Transform contentParent; // ScrollViewのContent

    private const string URL = "https://requin.jp/FJB/PHP/friendList.php";
    public string userid = CheckLoginOnStart.userid;  // ここでuseridが正しく取得できていることを確認してください。

    void Start()
    {
        if (!string.IsNullOrEmpty(userid))
        {
            StartCoroutine(GetFriendList(userid)); // ユーザーIDを指定
        }
        else
        {
            Debug.LogError("User ID is null or empty.");
        }
    }

    IEnumerator GetFriendList(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", userId);

        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                FriendList friendList = JsonUtility.FromJson<FriendList>(jsonResponse);

                if (friendList != null && friendList.friends != null)
                {
                    PopulateFriendList(friendList.friends);
                }
                else
                {
                    Debug.LogWarning("No friends found or invalid response.");
                }
            }
            else
            {
                Debug.LogError("Failed to fetch friend list: " + request.error);
            }
        }
    }

    void PopulateFriendList(List<FriendData> friends)
    {
        // 既存のボタンを消去
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 友達のリストを表示
        foreach (var friend in friends)
        {
            GameObject button = Instantiate(buttonPrefab, contentParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = friend.name; // TextMeshProUGUIのテキストを設定

            // ボタンにクリックイベントを設定
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnFriendSelected(friend));
        }
    }

    void OnFriendSelected(FriendData friend)
    {
        Debug.Log($"Selected Friend: {friend.name}");
    }
}
