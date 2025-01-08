using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour
{
    // ログアウトボタンが押されたときに呼ばれるメソッド
    public void OnLogoutButtonClicked()
    {
        // ログイン状態をリセット
        PlayerPrefs.SetString("UserLoggedIn", "logout"); // ログイン状態を未ログインに変更
        PlayerPrefs.Save(); // 変更を保存

        Debug.Log("User logged out!");

        // ログイン画面に遷移
        SceneManager.LoadScene("Top"); // ログイン画面のシーン名に変更
    }
}
