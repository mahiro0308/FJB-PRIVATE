using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class FriendSearch : MonoBehaviour
{
    public InputField friendIdInputField; // �t�����hID���͗p
    public Button addFriendButton; // �t�����h�ǉ��{�^��
    private string serverUrl = "http://localhost/friendSearch.php"; // PHP��URL
    private string friendId; // �t�����hID���ꎞ�ۑ�����ϐ�
    public Text friendIdDisplayText; // �������ʂ̃t�����hID�\���p�iText Legacy�j

    [System.Serializable]
    public class FriendSearchResponse
    {
        public string status;
        public string message;
        public string UserId; // �K�v�ȃf�[�^��ǉ�
    }

    private void Start()
    {
        // �t�����h�ǉ��{�^�����ŏ��͔�\���ɂ��Ă���
        addFriendButton.gameObject.SetActive(false);
        friendIdDisplayText.text = ""; // ������Ԃł͋�ɐݒ�
    }

    public void SearchFriend()
    {
        friendId = friendIdInputField.text;
        Debug.Log("Searching friend with ID: " + friendId); // �f�o�b�O�p�o��
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

                // ���X�|���X��JSON�Ƃ��ĉ��
                var jsonResponse = JsonUtility.FromJson<FriendSearchResponse>(responseText);

                if (jsonResponse.status == "error")
                {
                    Debug.Log("Friend not found.");
                    addFriendButton.gameObject.SetActive(false); // �t�����h��������Ȃ������ꍇ�A�{�^���͔�\��
                    friendIdDisplayText.text = ""; // ������Ȃ������ꍇ�͋�
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

    // �t�����h�ǉ��{�^�������������̏���
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
                addFriendButton.gameObject.SetActive(false); // �ǉ���Ƀ{�^�����\��
            }
            else
            {
                Debug.LogError("Error sending friend request: " + request.error);
            }
        }
    }
}
