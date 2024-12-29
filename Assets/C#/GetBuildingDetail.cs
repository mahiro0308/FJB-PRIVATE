using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetBuildingDetail : MonoBehaviour
{
    private const string phpUrl = "https://requin.jp/FJB/PHP/building-sql.php";

    public Text result_building_text;
    private string className;

    public float typingSpeed = 0.025f;

    private string fullText = ""; // �\������e�L�X�g�S��
    private string currentText = ""; // ���ݕ\�����̃e�L�X�g
    public static string TypingStatus { get; private set; } = "NotStarted";

    private BuildingData currentBuildingData; // ���݂̌����f�[�^
    private int textIndex = 0; // ���݂̃e�L�X�g�C���f�b�N�X

    void Start()
    {
        className = CameraCapture.ClassName;

        if (!string.IsNullOrEmpty(className))
        {
            StartCoroutine(GetBuildingData(className));
        }
        else
        {
            Debug.LogError("className is null or empty.");
        }
    }

    IEnumerator GetBuildingData(string className)
    {
        WWWForm form = new WWWForm();
        form.AddField("class_name", className);

        using (UnityWebRequest www = UnityWebRequest.Post(phpUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
                result_building_text.text = "Error connecting to server.";
                yield break;
            }

            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Response: " + jsonResponse);

            try
            {
                currentBuildingData = JsonUtility.FromJson<BuildingData>(jsonResponse);

                if (currentBuildingData != null && !string.IsNullOrEmpty(currentBuildingData.buildingName))
                {
                    Debug.Log("Building Name: " + currentBuildingData.buildingName);

                    textIndex = 0; // ������
                    ShowNextText(); // �ŏ��̃e�L�X�g��\��
                }
                else
                {
                    Debug.LogError("No building data found or invalid response.");
                    result_building_text.text = "Building not found.";
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to parse JSON: " + e.Message);
                result_building_text.text = "Error parsing data.";
            }
        }
    }

    public void ShowNextText()
    {
        if (currentBuildingData == null)
            return;

        TypingStatus = "InProgress";

        // ���ɕ\������e�L�X�g���擾
        if (textIndex == 0)
            fullText = currentBuildingData.TextOne;
        else if (textIndex == 1)
            fullText = currentBuildingData.TextTwo;
        else if (textIndex == 2)
            fullText = currentBuildingData.TextThree;
        else
        {
            TypingStatus = "Completed";
            return; // ���ׂẴe�L�X�g��\�����I������I��
        }

        textIndex++;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        currentText = "";
        foreach (char c in fullText)
        {
            currentText += c;
            result_building_text.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }

        TypingStatus = "Completed";
    }

    [System.Serializable]
    public class BuildingData
    {
        public string buildingName;
        public string TextOne;
        public string TextTwo;
        public string TextThree;
    }
}
