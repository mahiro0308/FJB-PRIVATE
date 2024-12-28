using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI�v�f�̎g�p�ɕK�v

public class ArTextProceedBtn : MonoBehaviour
{
    public Button proceedBtn; // �{�^���� Unity �G�f�B�^�Őݒ肷��
    private bool isButtonShown = false; // �{�^�����\������Ă��邩�̃t���O

    void Start()
    {
        // ������ԂŃ{�^�����\��
        if (proceedBtn != null)
        {
            proceedBtn.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Proceed Button is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        // TypingStatus �� Completed ���{�^�����\������Ă��Ȃ��ꍇ
        if (GetBuildingDetail.TypingStatus == "Completed" && !isButtonShown)
        {
            ShowButton();
        }
        // TypingStatus �� InProgress ���{�^�����\������Ă���ꍇ
        else if (GetBuildingDetail.TypingStatus == "InProgress" && isButtonShown)
        {
            HideButton();
        }
    }

    private void ShowButton()
    {
        proceedBtn.gameObject.SetActive(true);
        isButtonShown = true;
    }

    private void HideButton()
    {
        proceedBtn.gameObject.SetActive(false);
        isButtonShown = false;
    }
}
