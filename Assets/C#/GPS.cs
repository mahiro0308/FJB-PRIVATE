using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GPS : MonoBehaviour
{
    public Text gpsOut; // �\���p��Text UI
    private bool isUpdating = false;

    private void Update()
    {
        // �X�V���łȂ��ꍇ�̂݊J�n
        if (!isUpdating)
        {
            StartCoroutine(GetLocation());
            isUpdating = true;
        }
    }

    IEnumerator GetLocation()
    {
        // ���[�U�[���ʒu��񋖉����Ă��邩�m�F
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);

            // �����m�F�����܂őҋ@
            yield return new WaitForSeconds(5);
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Debug.Log("Location permission not granted");
                if (gpsOut != null)
                {
                    gpsOut.text = "Location permission not granted";
                }
                isUpdating = false;
                yield break;
            }
        }

        // ���[�U�[���ʒu���T�[�r�X��L�������Ă��邩�m�F
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services are disabled by the user.");
            if (gpsOut != null)
            {
                gpsOut.text = "Location services are disabled.";
            }
            yield return new WaitForSeconds(10);
            isUpdating = false;
            yield break;
        }

        // �T�[�r�X�J�n
        Input.location.Start();

        // �������̑ҋ@
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // �������Ɏ��s
        if (maxWait < 1)
        {
            Debug.Log("Location service initialization timed out.");
            if (gpsOut != null)
            {
                gpsOut.text = "Timed out while initializing location service.";
            }
            isUpdating = false;
            Input.location.Stop();
            yield break;
        }

        // �T�[�r�X�ڑ��Ɏ��s
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location.");
            if (gpsOut != null)
            {
                gpsOut.text = "Unable to determine location.";
            }
            isUpdating = false;
            Input.location.Stop();
            yield break;
        }

        // ����Ɉʒu�����擾
        var location = Input.location.lastData;
        Debug.Log($"Location: {location.latitude}, {location.longitude}, Altitude: {location.altitude}, Accuracy: {location.horizontalAccuracy}");
        if (gpsOut != null)
        {
            gpsOut.text = $"Location: {location.latitude}, {location.longitude}, Altitude: {location.altitude}, Accuracy: {location.horizontalAccuracy}";
        }

        // �T�[�r�X���~���A�X�V�t���O�����Z�b�g
        isUpdating = false;
        Input.location.Stop();
    }
}
