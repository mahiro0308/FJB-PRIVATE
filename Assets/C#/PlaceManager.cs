using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    private PlaceIndicator placeIndicator; // FindObjectOfTypeで取得するための変数
    public GameObject objectToPlace;       // 配置するオブジェクト
    private GameObject currentObject;      // 現在配置されているオブジェクト

    void Start()
    {
        // PlaceIndicatorを検索
        placeIndicator = FindObjectOfType<PlaceIndicator>();
    }

    // ボタンを押したときに呼び出される配置処理
    public void ClickToPlace()
    {
        // 現在配置されているオブジェクトがあれば削除
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        // PlaceIndicatorの位置と回転に基づいて新しいオブジェクトを配置
        currentObject = Instantiate(objectToPlace, placeIndicator.transform.position, placeIndicator.transform.rotation);
    }
}
