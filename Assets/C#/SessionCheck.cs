using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SessionCheck : MonoBehaviour
{
    public Text sessionStatusText; // UI�ɕ\������Text

    private string url = "http://localhost/logincheck.php"; // PHP�X�N���v�g��URL

    void Start()
    {
        StartCoroutine(CheckSession());
    }

    private IEnumerator CheckSession()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                sessionStatusText.text = "Error: " + www.error;
            }
            else
            {
                // JSON�f�[�^���f�V���A���C�Y
                string jsonResponse = www.downloadHandler.text;
                SessionResponse response = JsonUtility.FromJson<SessionResponse>(jsonResponse);

                // ���b�Z�[�W��UI�ɕ\��
                sessionStatusText.text = response.message;

                // ���b�Z�[�W�ɉ����ĉ�ʑJ��
                if (response.message == "���O�C��") // ������K�؂ȃ��b�Z�[�W�ɕύX
                {
                    ChangeScene("Top"); // "Top"�V�[���ɑJ��
                }
                else
                {
                    ChangeScene("Register"); // "Register"�V�[���ɑJ��
                }
            }
        }
    }

    // �V�[���J�ڂ̂��߂̃��\�b�h
    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

// JSON�f�[�^���}�b�s���O���邽�߂̃N���X
[System.Serializable]
public class SessionResponse
{
    public string message;
}
