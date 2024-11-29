using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public class TouchCameraMovement : MonoBehaviour
{
    public AbstractMap map;         // マップへの参照
    public Camera mainCamera;       // マップ操作に使うカメラ

    private Vector3 lastTouchPosition; // 最後のタッチ位置を記録

    void Update()
    {
        if (Input.touchCount == 1) // 1本指でパン操作
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // タッチの移動量を計算
                Vector2 delta = touch.deltaPosition;
                float factor = 0.000001f; // 移動速度調整（適切な値に変更）

                // 現在の中心座標を取得
                Vector2d currentLatLong = map.CenterLatitudeLongitude;

                // タッチ移動量を緯度経度に変換
                currentLatLong.x -= delta.y * factor; // 緯度（上下操作）
                currentLatLong.y -= delta.x * factor; // 経度（左右操作）

                // マップの中心を更新
                map.UpdateMap(currentLatLong, map.Zoom);
            }
        }

        else if (Input.touchCount == 2) // 2本指でズーム操作
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // 各タッチの前回と現在位置を取得
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            // ピンチジェスチャーの距離を計算
            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            // 距離の変化量を取得
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            // ズームの更新
            float zoomFactor = 0.01f; // ズーム速度調整
            float newZoom = Mathf.Clamp(map.Zoom + deltaMagnitudeDiff * zoomFactor, 1f, 20f); // ズーム範囲制限

            map.UpdateMap(map.CenterLatitudeLongitude, newZoom);
        }
    }
}
