using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{
    public InputField userIdInput; // UserId�p��InputField
    public InputField passwordInput; // Password�p��InputField
    public Button loginButton;      // ���O�C���{�^��
    public Text resultText;         // ���ʂ�\������Text

    private string url = "http://localhost/login.php"; // ���O�C���p��PHP�X�N���v�g��URL

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        StartCoroutine(LoginUser());
    }

    private IEnumerator LoginUser()
    {
        // ���͒l�����邩�m�F
        if (string.IsNullOrEmpty(userIdInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            resultText.text = "User ID��Password����͂��Ă�������";
            yield break;
        }

        // �t�H�[���f�[�^�̍쐬
        WWWForm form = new WWWForm();
        form.AddField("UserId", userIdInput.text); // UserId��ǉ�
        form.AddField("password", passwordInput.text); // Password��ǉ�

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                resultText.text = "Error: " + www.error;
            }
            else
            {
                // �T�[�o�[����̉��������
                string responseText = www.downloadHandler.text;

                // JSON�̃p�[�X�Ɖ����`�F�b�N
                try
                {
                    var response = JsonUtility.FromJson<LoginResponse>(responseText);

                    if (response.status == "���O�C��")
                    {
                        resultText.text = "Login successful!";
                        // ���O�C���������̏����i�K�v�ɉ����ĉ�ʑJ�ڂȂǁj
                    }
                    else
                    {
                        resultText.text = "Login failed: " + response.message;
                    }
                }
                catch
                {
                    resultText.text = "Unexpected response format";
                }
            }
        }
    }
}

// �T�[�o�[����̉����p�̃N���X
[System.Serializable]
public class LoginResponse
{
    public string status;
    public string message;
}
