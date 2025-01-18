using UnityEngine;
using UnityEngine.UI;

public class ButtonTextFinder : MonoBehaviour
{
    public Button myButton; // ボタンをアタッチする

    void Start()
    {
        // ボタンがクリックされたときの処理を追加
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // ボタンのすべての子オブジェクトを探索
        Text[] texts = myButton.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            // テキストが "friendnameserver" に一致するか確認
            if (text.text == "friendnamesaver")
            {
                Debug.Log("見つかったテキスト: " + text.text);
            }
        }
    }
}
