using UnityEngine;
using UnityEngine.UI;

public class ArTextProceedBtn : MonoBehaviour
{
    public Button proceedBtn;

    private void Start()
    {
        if (proceedBtn != null)
        {
            proceedBtn.gameObject.SetActive(false);
            proceedBtn.onClick.AddListener(OnProceedButtonClicked);
        }
        else
        {
            Debug.LogError("Proceed Button is not assigned in the Inspector.");
        }
    }

    private void Update()
    {
        if (GetBuildingDetail.TypingStatus == "Completed" && !proceedBtn.gameObject.activeSelf)
        {
            proceedBtn.gameObject.SetActive(true);
        }
        else if (GetBuildingDetail.TypingStatus == "InProgress" && proceedBtn.gameObject.activeSelf)
        {
            proceedBtn.gameObject.SetActive(false);
        }
    }

    private void OnProceedButtonClicked()
    {
        if (GetBuildingDetail.TypingStatus == "Completed")
        {
            proceedBtn.gameObject.SetActive(false);
            FindObjectOfType<GetBuildingDetail>().ShowNextText();
        }
    }
}
