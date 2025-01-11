using UnityEngine;
using UnityEngine.Networking;
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
    public string userid = CheckLoginOnStart.userid;

    void Start()
    {
        StartCoroutine(GetFriendList(userid)); // ユーザーIDを指定
    }

    IEnumerator GetFriendList(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", userId);

        UnityWebRequest request = UnityWebRequest.Post(URL, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            FriendList friendList = JsonUtility.FromJson<FriendList>(jsonResponse);
            PopulateFriendList(friendList.friends);
        }
        else
        {
            Debug.LogError("Failed to fetch friend list: " + request.error);
        }
    }

    void PopulateFriendList(List<FriendData> friends)
    {
        foreach (var friend in friends)
        {
            GameObject button = Instantiate(buttonPrefab, contentParent);
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = friend.name;
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnFriendSelected(friend));
        }
    }

    void OnFriendSelected(FriendData friend)
    {
        Debug.Log($"Selected Friend: {friend.name}");
    }
}
