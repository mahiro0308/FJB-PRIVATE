using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // UI要素の使用に必要

public class GetBuildingDetail : MonoBehaviour
{
    private const string phpUrl = "https://requin.jp/FJB/PHP/building-sql.php"; // PHPスクリプトのURL

    public Text result_building_text; // UnityのTextコンポーネントをアタッチ
    private string className;

    public float typingSpeed = 0.05f; // 文字の表示速度（秒）

    private string fullText = ""; // 完全なテキスト
    private string currentText = ""; // 現在表示されているテキスト
    public static string TypingStatus { get; private set; } = "NotStarted";

    // Start is called before the first frame update
    void Start()
    {
        // CameraCaptureからclass_nameを取得
        className = CameraCapture.ClassName; // CameraCaptureはstatic ClassNameがあることを前提

        if (!string.IsNullOrEmpty(className))
        {
            // データ取得コルーチンを開始
            StartCoroutine(GetBuildingData(className));
        }
        else
        {
            Debug.LogError("className is null or empty.");
        }
    }

    IEnumerator GetBuildingData(string className)
    {
        // POSTデータの作成
        WWWForm form = new WWWForm();
        form.AddField("class_name", className); // "class_name"キーに値を追加

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

            // JSONをパース
            try
            {
                BuildingData data = JsonUtility.FromJson<BuildingData>(jsonResponse);

                if (data != null && !string.IsNullOrEmpty(data.buildingName))
                {
                    Debug.Log("Building Name: " + data.buildingName);
                    Debug.Log("Text One: " + data.TextOne);

                    // テキストをUIに表示（タイプライター効果用にfullTextを設定）
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
        TypingStatus = "InProgress"; // タイプライター開始時に更新

        currentText = ""; // タイプライターを開始する際にクリア
        foreach (char c in fullText)
        {
            currentText += c;
            result_building_text.text = currentText; // 正しいTextコンポーネントを使用
            yield return new WaitForSeconds(typingSpeed);
        }

        TypingStatus = "Completed"; // 終了時に更新

    }

    // BuildingDataクラス
    [System.Serializable]
    public class BuildingData
    {
        public string buildingName;
        public string TextOne;
        public string TextTwo;
        public string TextThree;
    }
}
