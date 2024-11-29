using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GPS : MonoBehaviour
{
    public Text gpsOut; // 表示用のText UI
    private bool isUpdating = false;

    private void Update()
    {
        // 更新中でない場合のみ開始
        if (!isUpdating)
        {
            StartCoroutine(GetLocation());
            isUpdating = true;
        }
    }

    IEnumerator GetLocation()
    {
        // ユーザーが位置情報許可をしているか確認
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);

            // 許可が確認されるまで待機
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

        // ユーザーが位置情報サービスを有効化しているか確認
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

        // サービス開始
        Input.location.Start();

        // 初期化の待機
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // 初期化に失敗
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

        // サービス接続に失敗
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

        // 正常に位置情報を取得
        var location = Input.location.lastData;
        Debug.Log($"Location: {location.latitude}, {location.longitude}, Altitude: {location.altitude}, Accuracy: {location.horizontalAccuracy}");
        if (gpsOut != null)
        {
            gpsOut.text = $"Location: {location.latitude}, {location.longitude}, Altitude: {location.altitude}, Accuracy: {location.horizontalAccuracy}";
        }

        // サービスを停止し、更新フラグをリセット
        isUpdating = false;
        Input.location.Stop();
    }
}
