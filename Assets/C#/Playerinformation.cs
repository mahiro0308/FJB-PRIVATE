using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    public Text useridText; // Textコンポーネントへの参照
    public string userid;   // ユーザーIDを保持する文字列

    // Start is called before the first frame update
    void Start()
    {
        // CheckLoginOnStartからuseridを取得
        userid = CheckLoginOnStart.userid;

        // Textコンポーネントが正しく設定されているか確認
        if (useridText != null)
        {
            useridText.text = userid; // TextコンポーネントにユーザーIDを表示
        }
        else
        {
            Debug.LogError("useridText is not assigned in the Inspector.");
        }
    }
}
