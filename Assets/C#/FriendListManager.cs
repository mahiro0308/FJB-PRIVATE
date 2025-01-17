using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // Legacy Text 用
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class FriendData
{
    public int id;
    public string name;
}

[System.Serializable]
public class FriendListResponse
{
    public List<FriendData> friends;
}

public class FriendListManager : MonoBehaviour
{
    public GameObject FriendText;  // フレンドリスト用ボタンのプレハブ

    public Transform contentParent; // ScrollViewのContent

    private const string URL = "https://requin.jp/FJB/PHP/friendList.php";
    private GameObject buttonPrefab;
    private string username;

    void Start()
    {
        // プレハブを設定
        buttonPrefab = FriendText;

        // UserIDを取得
        username = PlayerPrefs.GetString("username", "");
        if (!string.IsNullOrEmpty(username))
        {
            StartCoroutine(GetFriendList(username)); // フレンドリストを取得
        }
        else
        {
            Debug.LogError("ユーザーIDが空です。PlayerPrefs で 'username' を確認してください。");
        }
    }

    IEnumerator GetFriendList(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", userId);

        Debug.Log("サーバーへのリクエスト開始: " + userId);

        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("サーバーからのレスポンス: " + jsonResponse);

                try
                {
                    FriendListResponse friendList = JsonUtility.FromJson<FriendListResponse>(jsonResponse);

                    if (friendList != null && friendList.friends != null && friendList.friends.Count > 0)
                    {
                        PopulateFriendList(friendList.friends);
                    }
                    else
                    {
                        Debug.LogWarning("フレンドが見つからないか、レスポンスが無効です。");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON レスポンスの解析に失敗しました: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("フレンドリストの取得に失敗しました: " + request.error);
            }
        }
    }

    void PopulateFriendList(List<FriendData> friends)
    {
        // Contentの子オブジェクトをすべて削除
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // フレンドリストを表示
        foreach (var friend in friends)
        {
            // ボタンプレハブを生成
            GameObject buttonObj = Instantiate(FriendText, contentParent);

            // ボタン内のテキストコンポーネントを取得
            Text textComponent = buttonObj.GetComponentInChildren<Text>();

            if (textComponent != null)
            {
                textComponent.text = "ユーザーID:"+friend.name; // テキストを設定
                Debug.Log($"フレンド名: {friend.name} を設定しました。");
            }
            else
            {
                Debug.LogError("プレハブ内に Text コンポーネントが見つかりません。");
            }
        }
  

}
}
