using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllPanelManager : MonoBehaviour
{
    // 他のスクリプトから取得可能なプロパティ
    public static string pubAdPanelManager { get; private set; } 

    void Awake()
    {
        // シーン遷移後もこのオブジェクトを破棄しないように設定
        DontDestroyOnLoad(this.gameObject);
    }

    // ボタンがクリックされた際に呼び出されるメソッド
    public void AdminPanelManager(string adminPanelManager)
    {
        // 引数で受け取った文字列を保存
        pubAdPanelManager = adminPanelManager;

        // 確認用ログ

        // "home" シーンに遷移
        SceneManager.LoadScene("home");
    }


}
