using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllPanelManager : MonoBehaviour
{
    // ���̃X�N���v�g����擾�\�ȃv���p�e�B
    public static string pubAdPanelManager { get; private set; } 

    void Awake()
    {
        // �V�[���J�ڌ�����̃I�u�W�F�N�g��j�����Ȃ��悤�ɐݒ�
        DontDestroyOnLoad(this.gameObject);
    }

    // �{�^�����N���b�N���ꂽ�ۂɌĂяo����郁�\�b�h
    public void AdminPanelManager(string adminPanelManager)
    {
        // �����Ŏ󂯎�����������ۑ�
        pubAdPanelManager = adminPanelManager;

        // �m�F�p���O

        // "home" �V�[���ɑJ��
        SceneManager.LoadScene("home");
    }


}
