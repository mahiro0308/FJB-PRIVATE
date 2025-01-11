using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginOrRegister : MonoBehaviour
{
    public GameObject RegistratePanel;
    public GameObject LoginPanel;

    // Start is called before the first frame update
    private void Start()
    {
        // 初期状態: 登録パネルは非表示、ログインパネルは表示
        RegistratePanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void BtnClicked()
    {
        // 登録パネルが非表示の場合に切り替え処理を行う
        if (!RegistratePanel.activeSelf)
        {
            RegistratePanel.SetActive(true);
            LoginPanel.SetActive(false);
        }
        else
        {
            RegistratePanel.SetActive(false);
            LoginPanel.SetActive(true);
        }
    }
}
