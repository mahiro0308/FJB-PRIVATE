using UnityEngine;

public class PanelSwipe : MonoBehaviour
{
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isSwiping = false;

    private RectTransform panelRectTransform;

    // �X���C�v�ʂ�1f�ɐ���
    private float swipeThreshold = 1f;

    private void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // �X���C�v���n�܂����Ƃ�
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                isSwiping = true;
            }

            // �X���C�v�̓r��
            if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                endTouchPos = touch.position;
            }

            // �X���C�v�I����
            if (touch.phase == TouchPhase.Ended)
            {
                float swipeDistance = endTouchPos.x - startTouchPos.x;

                // �X���C�v����������1f�����X���C�h
                if (Mathf.Abs(swipeDistance) >= swipeThreshold)
                {
                    // 1f�P�ʂŃp�l���̈ʒu��ύX
                    float direction = Mathf.Sign(swipeDistance); // �X���C�v�̕���
                    panelRectTransform.anchoredPosition += new Vector2(direction * swipeThreshold, 0);
                }

                isSwiping = false;
            }
        }
    }
}
