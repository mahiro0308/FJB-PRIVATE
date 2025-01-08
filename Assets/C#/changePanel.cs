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
    // ����������
    public void Start()
    {
        // �ŏ��ɕ\������p�l���̂݃A�N�e�B�u�ɐݒ�
        informationpanel.SetActive(false);
        friendlistpanel.SetActive(false);
        homepanel.SetActive(false);  // �z�[���p�l�����f�t�H���g�ŃA�N�e�B�u��
        arpanel.SetActive(false);
        chatpanel.SetActive(false);
        shoppanel.SetActive(false);
        if (AllPanelManager.pubAdPanelManager != null)
        {
            string aa = AllPanelManager.pubAdPanelManager;
            Debug.LogError(aa);
            // �w�肳�ꂽ�p�l���̂݃A�N�e�B�u�ɂ���
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

    // �p�l����؂�ւ��郁�\�b�h
    public void changepanel(string panelName)
    {
        // ���ׂẴp�l�����A�N�e�B�u�ɂ���
        informationpanel.SetActive(false);
        friendlistpanel.SetActive(false);
        homepanel.SetActive(false);
        arpanel.SetActive(false);
        chatpanel.SetActive(false);
        shoppanel.SetActive(false);


        // �w�肳�ꂽ�p�l���̂݃A�N�e�B�u�ɂ���
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
