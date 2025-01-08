using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckLoginOnStart : MonoBehaviour
{
    public static string email { get; private set; }
    public void LoginCheck()
    {
        Debug.Log("Checking login status...");
        string isLoggedIn = PlayerPrefs.GetString("UserLoggedIn", "No login"); // デフォルト値は0
        

        // 保存された値をログに出力
        Debug.Log("Current UserLoggedIn value: " + isLoggedIn);

        if (isLoggedIn == "login")
        {

            email = PlayerPrefs.GetString("accountEmail", "No email");
            Debug.Log("User is logged in!");
            SceneManager.LoadScene("home"); // ログイン画面に遷移
        }
        else
        {
            Debug.Log("User is not logged in. Redirecting to login screen.");
            SceneManager.LoadScene("register"); // ホーム画面に遷移

        }
    }
}
