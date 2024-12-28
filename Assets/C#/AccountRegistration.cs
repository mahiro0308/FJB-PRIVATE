using System.Collections; // IEnumerator�̂��߂ɕK�v
using System.Collections.Generic; // List�Ȃǂ̂��߂ɕK�v
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AccountRegistration : MonoBehaviour
{
    public InputField userIdInput; // UserId�p��InputField
    public InputField emailInput;   // Email�p��InputField
    public InputField passwordInput; // Password�p��InputField
    public Button registerButton;    // �o�^�{�^��
    public Text resultText;          // ���ʂ�\������Text

    private string url = "http://localhost/register.php"; // PHP�X�N���v�g��URL

    void Start()
    {
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
    }

    private void OnRegisterButtonClicked()
    {
        StartCoroutine(RegisterUser());
    }

    private IEnumerator RegisterUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userIdInput.text); // UserId��ǉ�
        form.AddField("email", emailInput.text);   // Email��ǉ�
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
                resultText.text = "Registration successful!";
                // �ǉ������i�K�v�ɉ����āj
            }
        }
    }
}
