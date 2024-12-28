using UnityEngine;

public class PanelSwipe : MonoBehaviour
{
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isSwiping = false;

    private RectTransform panelRectTransform;

    // スワイプ量を1fに制御
    private float swipeThreshold = 1f;

    private void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // スワイプが始まったとき
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                isSwiping = true;
            }

            // スワイプの途中
            if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                endTouchPos = touch.position;
            }

            // スワイプ終了時
            if (touch.phase == TouchPhase.Ended)
            {
                float swipeDistance = endTouchPos.x - startTouchPos.x;

                // スワイプした方向に1fだけスライド
                if (Mathf.Abs(swipeDistance) >= swipeThreshold)
                {
                    // 1f単位でパネルの位置を変更
                    float direction = Mathf.Sign(swipeDistance); // スワイプの方向
                    panelRectTransform.anchoredPosition += new Vector2(direction * swipeThreshold, 0);
                }

                isSwiping = false;
            }
        }
    }
}
