using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI要素の使用に必要

public class ArTextProceedBtn : MonoBehaviour
{
    public Button proceedBtn; // ボタンを Unity エディタで設定する
    private bool isButtonShown = false; // ボタンが表示されているかのフラグ

    void Start()
    {
        // 初期状態でボタンを非表示
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
        // TypingStatus が Completed かつボタンが表示されていない場合
        if (GetBuildingDetail.TypingStatus == "Completed" && !isButtonShown)
        {
            ShowButton();
        }
        // TypingStatus が InProgress かつボタンが表示されている場合
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
