using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;  // JSON��͗p�ɒǉ��iNewtonsoft.Json��Unity�ɃC���|�[�g���K�v�j

public class LoginChecker : MonoBehaviour
{
    private string url = "http://localhost/logincheck.php";

    void Start()
    {
        StartCoroutine(CheckLoginStatus());
    }

    private IEnumerator CheckLoginStatus()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string response = request.downloadHandler.text;

                // JSON���
                var json = JObject.Parse(response);
                string message = (string)json["message"];

                // "���O�C��" �ƕԂ��Ă����ꍇ�ɃV�[����J��
                if (message == "���O�C��")
                {
                   Debug.Log("���O�C��");

                }
                else
                {
                    SceneManager.LoadScene("login-page");  // ���̃V�[���ɑJ�� 
                }
            }
        }
    }
}
