using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FriendAccept : MonoBehaviour
{
    // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnAcceptButtonClick()
    {
        // PHP�Ƀ��N�G�X�g�𑗐M����R���[�`�����J�n
        StartCoroutine(SendAcceptRequest());
    }

    // PHP��accept�f�[�^�𑗐M����R���[�`��
    IEnumerator SendAcceptRequest()
    {
        // PHP�X�N���v�g��URL
        string url = "http://localhost/friendAccept-program.php";  // �K�؂�URL�ɒu�������Ă�������

        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("status", "accept"); // "status"�t�B�[���h��"accept"��ǉ�

        // POST���N�G�X�g���쐬
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // ���N�G�X�g�̑��M
            yield return www.SendWebRequest();

            // �G���[�`�F�b�N
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Request sent successfully: " + www.downloadHandler.text);
            }
        }
    }
}
