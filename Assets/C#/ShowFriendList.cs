using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ShowFriendList : MonoBehaviour
{
    public Text friendInfoText;  // PHP����̃f�[�^��\������Text�R���|�[�l���g

    private void Start()
    {
        StartCoroutine(FriendInfo());
    }

    IEnumerator FriendInfo()
    {
        string url = "http://localhost/getFriendData.php";  // PHP�X�N���v�g��URL�ɒu�������Ă�������

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        // ���N�G�X�g�̑��M
        yield return request.SendWebRequest();

        // �G���[�`�F�b�N
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            friendInfoText.text = "Error: " + request.error;
        }
        else
        {
            // ���X�|���X�f�[�^�̉��
            string jsonResponse = request.downloadHandler.text;

            // JSON�f�[�^����łȂ����Ƃ��m�F
            if (!string.IsNullOrEmpty(jsonResponse))
            {
                // JSON�f�[�^��FriendListData�N���X�Ƀf�V���A���C�Y
                FriendListData friendList = JsonUtility.FromJson<FriendListData>(jsonResponse);

                // ���ʂɊ�Â��ĕ\�����X�V
                if (friendList.status == "success")
                {
                    // �t�����h����\��
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

// PHP����̃��X�|���X���������߂̃N���X
[System.Serializable]
public class FriendInfo
{
    public string user_id;
    public string friend_id;
    public string friend_status;
}

[System.Serializable]
public class FriendListData  // �N���X����ύX
{
    public string status;
    public string message;
    public FriendInfo[] friends;  // �z��Ńt�����h����ێ�
}
