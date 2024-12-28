using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;  // Text�R���|�[�l���g���������߂ɕK�v
using System.Collections;
using System.Collections.Generic;

public class MySQLViaPHP : MonoBehaviour
{
    // PHP�X�N���v�g��URL
    private string url = "http://localhost/unity.php";

    // UI��Text�R���|�[�l���g�ւ̎Q��
    public Text displayText;

    void Start()
    {
        // �T�[�o�[����f�[�^���擾����R���[�`�����J�n
        StartCoroutine(GetDataFromServer());
    }

    IEnumerator GetDataFromServer()
    {
        // �T�[�o�[�Ƀ��N�G�X�g�𑗐M
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        // �G���[�`�F�b�N
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            // PHP����̉������擾
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Response from server: " + jsonResponse);

            // JSON�f�[�^���f�V���A���C�Y�i�p�[�X�j
            List<User> users = JsonHelper.GetJsonList<User>(jsonResponse);

            // �󂯎�����f�[�^��Text�R���|�[�l���g�ɕ\��
            string displayData = "";
            foreach (var user in users)
            {
                displayData += $"No: {user.No}, UserName: {user.UserName}, UserId: {user.UserId}, UserPassword: {user.UserPassword}\n";
            }

            // UI��Text�R���|�[�l���g�Ƀf�[�^��\��
            displayText.text = displayData;
        }
    }
}

// JSON�f�[�^���}�b�s���O���邽�߂̃N���X
[System.Serializable]
public class User
{
    public string No;
    public string UserName;
    public string UserId;
    public string UserPassword;
}

// JSON�p�[�X�̂��߂̃w���p�[�N���X
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
