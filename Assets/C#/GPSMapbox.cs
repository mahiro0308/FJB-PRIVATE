using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class GPSMapbox : MonoBehaviour
{
    public Text gpsOut; // �\���p��Text UI
    public AbstractMap map; // AbstractMap �ւ̎Q��
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

            yield return new WaitForSeconds(5); // ���ҋ@
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Debug.Log("Location permission not granted");
                gpsOut.text = "Location permission not granted";
                isUpdating = false;
                yield break;
            }
        }

        // ���[�U�[���ʒu���T�[�r�X��L�������Ă��邩�m�F
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services are disabled by the user.");
            gpsOut.text = "Location services are disabled.";
            yield return new WaitForSeconds(10);
            isUpdating = false;
            yield break;
        }

        // �T�[�r�X�J�n
        Input.location.Start();

        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Failed to initialize or determine location.");
            gpsOut.text = "Failed to get location.";
            isUpdating = false;
            Input.location.Stop();
            yield break;
        }

        // ����Ɉʒu�����擾
        var location = Input.location.lastData;
        Debug.Log($"Location: {location.latitude}, {location.longitude}");
        gpsOut.text = $"Lat: {location.latitude}, Lon: {location.longitude}";

        // Mapbox �Ɉʒu���𔽉f
        if (map != null)
        {
            Vector2d coordinates = new Vector2d(location.latitude, location.longitude);
            map.SetCenterLatitudeLongitude(coordinates); // �n�}�̒��S��ύX
            map.UpdateMap(); // �n�}���X�V
        }

        // �T�[�r�X��~
        isUpdating = false;
        Input.location.Stop();
    }
}
