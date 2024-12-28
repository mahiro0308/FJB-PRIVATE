using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonColor : MonoBehaviour
{
    public Button button1; // ボタン1
    public Button button2; // ボタン2
    private Color defaultColor; // デフォルトの色
    private Color selectedColor; // クリックされた時の色
    public GameObject panel1;
    public GameObject panel2;

    void Start()
    {
        // 色を設定
        ColorUtility.TryParseHtmlString("#9ADEAE", out selectedColor); // クリックされたときの色
        ColorUtility.TryParseHtmlString("#FFFFFF", out defaultColor); // デフォルトの色（白）

        // 初期状態で button1 を選択状態に設定し、button2 はデフォルトの色
        button1.GetComponent<Image>().color = selectedColor;
        button2.GetComponent<Image>().color = defaultColor;

        // ボタンのクリックイベントを追加
        button1.onClick.AddListener(() => OnButtonClick(button1));
        button2.onClick.AddListener(() => OnButtonClick(button2));

        // 初期状態で panel1 を表示し、panel2 を非表示に設定
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    void OnButtonClick(Button clickedButton)
    {
        // button1がクリックされた場合、button1を選択色にし、button2はデフォルト色に戻す
        if (clickedButton == button1)
        {
            button1.GetComponent<Image>().color = selectedColor;
            button2.GetComponent<Image>().color = defaultColor;
            panel1.SetActive(true);
            panel2.SetActive(false);
        }
        // button2がクリックされた場合、button2を選択色にし、button1はデフォルト色に戻す
        else if (clickedButton == button2)
        {
            button2.GetComponent<Image>().color = selectedColor;
            button1.GetComponent<Image>().color = defaultColor;
            panel1.SetActive(false);
            panel2.SetActive(true);
        }
    }
}
