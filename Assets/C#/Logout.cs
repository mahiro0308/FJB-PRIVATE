using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    public Button logoutButton; // ���O�A�E�g�{�^��
    private string logoutUrl = "http://localhost/logout.php"; // ���O�A�E�g�p��PHP�X�N���v�g��URL

    void Start()
    {
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
    }

    private void OnLogoutButtonClicked()
    {
        StartCoroutine(LogoutUser());
    }

    private IEnumerator LogoutUser()
    {
        // ���O�A�E�g�������̃��b�Z�[�W

        using (UnityWebRequest www = UnityWebRequest.Get(logoutUrl))
        {
            yield return www.SendWebRequest();

            // �G���[�`�F�b�N
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError ||
                www.result == UnityWebRequest.Result.DataProcessingError)
            {
            }
            else
            {
                Debug.Log("���O�A�E�g�G���[");
            }
        }
    }
}
