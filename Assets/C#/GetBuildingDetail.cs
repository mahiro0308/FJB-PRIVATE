using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // UI�v�f�̎g�p�ɕK�v

public class GetBuildingDetail : MonoBehaviour
{
    private const string phpUrl = "https://requin.jp/FJB/PHP/building-sql.php"; // PHP�X�N���v�g��URL

    public Text result_building_text; // Unity��Text�R���|�[�l���g���A�^�b�`
    private string className;

    public float typingSpeed = 0.05f; // �����̕\�����x�i�b�j

    private string fullText = ""; // ���S�ȃe�L�X�g
    private string currentText = ""; // ���ݕ\������Ă���e�L�X�g
    public static string TypingStatus { get; private set; } = "NotStarted";

    // Start is called before the first frame update
    void Start()
    {
        // CameraCapture����class_name���擾
        className = CameraCapture.ClassName; // CameraCapture��static ClassName�����邱�Ƃ�O��

        if (!string.IsNullOrEmpty(className))
        {
            // �f�[�^�擾�R���[�`�����J�n
            StartCoroutine(GetBuildingData(className));
        }
        else
        {
            Debug.LogError("className is null or empty.");
        }
    }

    IEnumerator GetBuildingData(string className)
    {
        // POST�f�[�^�̍쐬
        WWWForm form = new WWWForm();
        form.AddField("class_name", className); // "class_name"�L�[�ɒl��ǉ�

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

            // JSON���p�[�X
            try
            {
                BuildingData data = JsonUtility.FromJson<BuildingData>(jsonResponse);

                if (data != null && !string.IsNullOrEmpty(data.buildingName))
                {
                    Debug.Log("Building Name: " + data.buildingName);
                    Debug.Log("Text One: " + data.TextOne);

                    // �e�L�X�g��UI�ɕ\���i�^�C�v���C�^�[���ʗp��fullText��ݒ�j
                    fullText = data.TextOne;
                    StartCoroutine(ShowText());
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

    IEnumerator ShowText()
    {
        TypingStatus = "InProgress"; // �^�C�v���C�^�[�J�n���ɍX�V

        currentText = ""; // �^�C�v���C�^�[���J�n����ۂɃN���A
        foreach (char c in fullText)
        {
            currentText += c;
            result_building_text.text = currentText; // ������Text�R���|�[�l���g���g�p
            yield return new WaitForSeconds(typingSpeed);
        }

        TypingStatus = "Completed"; // �I�����ɍX�V

    }

    // BuildingData�N���X
    [System.Serializable]
    public class BuildingData
    {
        public string buildingName;
        public string TextOne;
        public string TextTwo;
        public string TextThree;
    }
}
