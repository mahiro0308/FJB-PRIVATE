using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class CameraCapture : MonoBehaviour
{
    public Camera cam;   // Unity�̃J�����I�u�W�F�N�g
    public Text resultText;  // ���ʌ��ʂ�\������Text�R���|�[�l���g

    void Start()
    {
        // ���Ԋu�ŉ摜���L���v�`�����ăT�[�o�[�ɑ��M����R���[�`�����J�n
        StartCoroutine(CaptureAndSendImage());
    }

    IEnumerator CaptureAndSendImage()
    {
        while (true)
        {
            // �J�����̉f�����e�N�X�`���ɃL���v�`��
            RenderTexture renderTexture = new RenderTexture(256, 256, 24);
            cam.targetTexture = renderTexture;
            Texture2D screenshot = new Texture2D(256, 256, TextureFormat.RGB24, false);
            cam.Render();
            RenderTexture.active = renderTexture;
            screenshot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            // �摜��PNG�ɃG���R�[�h
            byte[] bytes = screenshot.EncodeToPNG();
            Destroy(screenshot);

            // �T�[�o�[�ɉ摜�𑗐M���Č��ʂ��擾
            yield return StartCoroutine(Upload(bytes));

            // ���̉摜�𑗐M����܂�0.5�b�ҋ@
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Upload(byte[] bytes)
    {
        // Flask�T�[�o�[��HTTP POST���N�G�X�g�ŉ摜�f�[�^�𑗐M
        UnityWebRequest www = new UnityWebRequest("http://localhost:5000/predict", "POST");
        www.uploadHandler = new UploadHandlerRaw(bytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/octet-stream");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
            resultText.text = "Error: " + www.error;  // �G���[���e��\��
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);  // �T�[�o�[����̉�����\��
            resultText.text = www.downloadHandler.text;  // ���ʌ��ʂ�Text�t�B�[���h�ɕ\��
        }
    }
}