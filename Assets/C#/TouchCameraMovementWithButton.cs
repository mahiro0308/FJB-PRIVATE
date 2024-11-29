using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;

public class TouchCameraMovementWithButton : MonoBehaviour
{
    public AbstractMap map;          // マップへの参照
    public Camera mainCamera;        // マップ操作に使うカメラ
    public Button backToCurrentLocationButton; // ボタンへの参照

    private void Start()
    {
        // ボタンにリスナーを追加
        backToCurrentLocationButton.onClick.AddListener(MoveToCurrentLocation);

        // 現在地サービスを開始
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("位置情報サービスが有効になっていません。");
            return;
        }

        Input.location.Start(); // 位置情報の取得を開始
    }

    private void Update()
    {
        // 他のカメラ操作のコード
    }

    // ボタンをクリックしたら現在地に戻る
    private void MoveToCurrentLocation()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // 現在地を取得
            LocationInfo location = Input.location.lastData;
            Vector2d currentLatLong = new Vector2d(location.latitude, location.longitude);

            // マップを現在地に更新
            map.UpdateMap(currentLatLong, map.Zoom);
        }
        else
        {
            Debug.LogWarning("現在地を取得できません。位置情報サービスが有効でない可能性があります。");
        }
    }

    private void OnDestroy()
    {
        // 位置情報サービスを停止
        if (Input.location.isEnabledByUser)
        {
            Input.location.Stop();
        }
    }
}
