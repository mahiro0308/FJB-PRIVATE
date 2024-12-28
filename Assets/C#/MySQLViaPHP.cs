using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;  // Textコンポーネントを扱うために必要
using System.Collections;
using System.Collections.Generic;

public class MySQLViaPHP : MonoBehaviour
{
    // PHPスクリプトのURL
    private string url = "http://localhost/unity.php";

    // UIのTextコンポーネントへの参照
    public Text displayText;

    void Start()
    {
        // サーバーからデータを取得するコルーチンを開始
        StartCoroutine(GetDataFromServer());
    }

    IEnumerator GetDataFromServer()
    {
        // サーバーにリクエストを送信
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        // エラーチェック
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            // PHPからの応答を取得
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Response from server: " + jsonResponse);

            // JSONデータをデシリアライズ（パース）
            List<User> users = JsonHelper.GetJsonList<User>(jsonResponse);

            // 受け取ったデータをTextコンポーネントに表示
            string displayData = "";
            foreach (var user in users)
            {
                displayData += $"No: {user.No}, UserName: {user.UserName}, UserId: {user.UserId}, UserPassword: {user.UserPassword}\n";
            }

            // UIのTextコンポーネントにデータを表示
            displayText.text = displayData;
        }
    }
}

// JSONデータをマッピングするためのクラス
[System.Serializable]
public class User
{
    public string No;
    public string UserName;
    public string UserId;
    public string UserPassword;
}

// JSONパースのためのヘルパークラス
public static class JsonHelper
{
    public static List<T> GetJsonList<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> array;
    }
}
