using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FriendList : MonoBehaviour
{
    public GameObject textPrefab;  // �v���n�u�Ƃ��Đݒ肳�ꂽText�I�u�W�F�N�g
    public Transform parentTransform;  // Text�I�u�W�F�N�g��z�u����eTransform

    private void Start()
    {
        StartCoroutine(GetFriendData());
    }

    IEnumerator GetFriendData()
    {
        string url = "http://localhost/getFriendRequestList.php";  // PHP�X�N���v�g��URL�ɒu�������Ă�������

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        // ���N�G�X�g�̑��M
        yield return request.SendWebRequest();

        // �G���[�`�F�b�N
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            CreateText("Error: " + request.error);
        }
        else
        {
            // ���X�|���X�f�[�^�̉��
            string jsonResponse = request.downloadHandler.text;

            // JSON�f�[�^��FriendListData�N���X�Ƀf�V���A���C�Y
            FriendListData friendListData = JsonUtility.FromJson<FriendListData>(jsonResponse);

            // ���ʂɊ�Â��ĕ\�����X�V
            if (friendListData.status == "success")
            {
                // �t�����h���̐�����Text�I�u�W�F�N�g���쐬
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

    // �V����Text�I�u�W�F�N�g���쐬���ĐeTransform�ɒǉ�����
    void CreateText(string message)
    {
        GameObject newText = Instantiate(textPrefab, parentTransform);
        newText.GetComponent<Text>().text = message;
    }
}

// PHP����̃��X�|���X���������߂̃N���X
[System.Serializable]
public class Friend
{
    public string user_id;
    public string friend_id;
    public string friend_status;
}

// FriendListData�N���X
[System.Serializable]
public class FriendListDataaa
{
    public string status;
    public string message;
    public Friend[] friends;  // ������Friend�I�u�W�F�N�g���i�[����z��
}
