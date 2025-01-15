using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    public Text useridText; // Textコンポーネントへの参照

    // Start is called before the first frame update
    void Start()
    {
        
        // CheckLoginOnStartからuseridを取得
        string userid = PlayerPrefs.GetString("username", "");

        // Textコンポーネントが正しく設定されているか確認
        if (useridText != null)
        {
            useridText.text = userid; // TextコンポーネントにユーザーIDを表示
            Debug.Log(userid);
        }
        else
        {
            Debug.LogError("useridText is not assigned in the Inspector.");
        }
    }
}
