using System.Collections; // IEnumeratorのために必要
using System.Collections.Generic; // Listなどのために必要
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AccountRegistration : MonoBehaviour
{
    public InputField userIdInput; // UserId用のInputField
    public InputField emailInput;   // Email用のInputField
    public InputField passwordInput; // Password用のInputField
    public Button registerButton;    // 登録ボタン
    public Text resultText;          // 結果を表示するText

    private string url = "http://localhost/register.php"; // PHPスクリプトのURL

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
        form.AddField("userId", userIdInput.text); // UserIdを追加
        form.AddField("email", emailInput.text);   // Emailを追加
        form.AddField("password", passwordInput.text); // Passwordを追加

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
                // 追加処理（必要に応じて）
            }
        }
    }
}
