using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        // プロジェクトが開始されたときにパネルを非表示にする
        panel.SetActive(false);
    }

    public void TogglePanel()
    {
        // ボタンがクリックされたときにパネルの表示・非表示を切り替える
        panel.SetActive(!panel.activeSelf);
    }
}
