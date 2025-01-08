using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changePanel : MonoBehaviour
{
    public GameObject informationBtn;
    public GameObject friendlistBtn;
    public GameObject homeBtn;
    public GameObject arBtn;
    public GameObject chatBtn;
    public GameObject shopBtn;

    public GameObject informationpanel;
    public GameObject friendlistpanel;
    public GameObject homepanel;
    public GameObject arpanel;
    public GameObject chatpanel;
    public GameObject shoppanel;
    // 初期化処理
    public void Start()
    {
        // 最初に表示するパネルのみアクティブに設定
        informationpanel.SetActive(false);
        friendlistpanel.SetActive(false);
        homepanel.SetActive(false);  // ホームパネルをデフォルトでアクティブに
        arpanel.SetActive(false);
        chatpanel.SetActive(false);
        shoppanel.SetActive(false);
        if (AllPanelManager.pubAdPanelManager != null)
        {
            string aa = AllPanelManager.pubAdPanelManager;
            Debug.LogError(aa);
            // 指定されたパネルのみアクティブにする
            switch (aa)
            {
                case "information":
                    informationpanel.SetActive(true);
                    break;
                case "friendlist":
                    friendlistpanel.SetActive(true);
                    break;
                case "home":
                    homepanel.SetActive(true);
                    break;
                case "ar":
                    arpanel.SetActive(true);
                    break;
                case "chat":
                    chatpanel.SetActive(true);
                    break;
                case "shop":
                    shoppanel.SetActive(true);
                    break;
                default:
                    break;
            }
        }else{
            homepanel.SetActive(true);
        }
    }

    // パネルを切り替えるメソッド
    public void changepanel(string panelName)
    {
        // すべてのパネルを非アクティブにする
        informationpanel.SetActive(false);
        friendlistpanel.SetActive(false);
        homepanel.SetActive(false);
        arpanel.SetActive(false);
        chatpanel.SetActive(false);
        shoppanel.SetActive(false);


        // 指定されたパネルのみアクティブにする
        switch (panelName)
        {
            case "information":
                informationpanel.SetActive(true);
                break;
            case "friendlist":
                friendlistpanel.SetActive(true);
                break;
            case "home":
                homepanel.SetActive(true);
                break;
            case "ar":
                arpanel.SetActive(true);
                break;
            case "chat":
                chatpanel.SetActive(true);
                break;
            case "shop":
                shoppanel.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid panel name: " + panelName);
                break;
        }
    }
}
