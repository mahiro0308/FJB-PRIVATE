using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FriendListManager : MonoBehaviour
{
    public Text friendListText; // UI�Ƀt�����h���X�g��\�����邽�߂�Text
    private string serverUrl = "http://localhost/getFriendList.php"; // �t�����h���X�g���擾����PHP��URL
    private string userId = "1"; // ���̃��[�U�[ID�i���ۂɂ̓��O�C���������[�U�[ID���g���j

    private void Start()
    {
        // �t�����h���X�g���擾
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

                // �󂯎�����t�����h���X�g�̏����i�J���}��؂��ID���Ԃ���Ă���Ɖ���j
                string[] friendIds = responseText.Split(',');

                // �t�����h���X�g��UI�ɕ\��
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
