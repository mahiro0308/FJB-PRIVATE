using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class GPSMapbox : MonoBehaviour
{
    public Text gpsOut; // 表示用のText UI
    public AbstractMap map; // AbstractMap への参照
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

            yield return new WaitForSeconds(5); // 許可待機
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Debug.Log("Location permission not granted");
                gpsOut.text = "Location permission not granted";
                isUpdating = false;
                yield break;
            }
        }

        // ユーザーが位置情報サービスを有効化しているか確認
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services are disabled by the user.");
            gpsOut.text = "Location services are disabled.";
            yield return new WaitForSeconds(10);
            isUpdating = false;
            yield break;
        }

        // サービス開始
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

        // 正常に位置情報を取得
        var location = Input.location.lastData;
        Debug.Log($"Location: {location.latitude}, {location.longitude}");
        gpsOut.text = $"Lat: {location.latitude}, Lon: {location.longitude}";

        // Mapbox に位置情報を反映
        if (map != null)
        {
            Vector2d coordinates = new Vector2d(location.latitude, location.longitude);
            map.SetCenterLatitudeLongitude(coordinates); // 地図の中心を変更
            map.UpdateMap(); // 地図を更新
        }

        // サービス停止
        isUpdating = false;
        Input.location.Stop();
    }
}
