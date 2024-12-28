using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CheckLoginStatus : MonoBehaviour
{
    private string url = "http://localhost/check_session.php"; // �Z�b�V�����m�F�p��PHP�X�N���v�g��URL
    public Text resultText; // ���ʂ�\������Text�i�I�v�V�����j

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
                Debug.Log("Error: " + www.error);
                resultText.text = "Error: " + www.error;
            }
            else
            {
                // PHP����Ԃ��ꂽ���ʂ��擾
                string jsonResult = www.downloadHandler.text;
                Debug.Log("Session Result: " + jsonResult);

                // ���ʂ����
                if (jsonResult.Contains("\"status\":\"loggedin\""))
                {
                    resultText.text = "User is logged in!";
                    Debug.Log("User is logged in!");
                }
                else if (jsonResult.Contains("\"status\":\"not_logged_in\""))
                {
                    resultText.text = "User is not logged in.";
                    Debug.Log("User is not logged in.");
                }
            }
        }
    }
}
