using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public class TouchCameraMovement : MonoBehaviour
{
    public AbstractMap map;         // �}�b�v�ւ̎Q��
    public Camera mainCamera;       // �}�b�v����Ɏg���J����

    private Vector3 lastTouchPosition; // �Ō�̃^�b�`�ʒu���L�^

    void Update()
    {
        if (Input.touchCount == 1) // 1�{�w�Ńp������
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // �^�b�`�̈ړ��ʂ��v�Z
                Vector2 delta = touch.deltaPosition;
                float factor = 0.000001f; // �ړ����x�����i�K�؂Ȓl�ɕύX�j

                // ���݂̒��S���W���擾
                Vector2d currentLatLong = map.CenterLatitudeLongitude;

                // �^�b�`�ړ��ʂ��ܓx�o�x�ɕϊ�
                currentLatLong.x -= delta.y * factor; // �ܓx�i�㉺����j
                currentLatLong.y -= delta.x * factor; // �o�x�i���E����j

                // �}�b�v�̒��S���X�V
                map.UpdateMap(currentLatLong, map.Zoom);
            }
        }

        else if (Input.touchCount == 2) // 2�{�w�ŃY�[������
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // �e�^�b�`�̑O��ƌ��݈ʒu���擾
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            // �s���`�W�F�X�`���[�̋������v�Z
            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            // �����̕ω��ʂ��擾
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            // �Y�[���̍X�V
            float zoomFactor = 0.01f; // �Y�[�����x����
            float newZoom = Mathf.Clamp(map.Zoom + deltaMagnitudeDiff * zoomFactor, 1f, 20f); // �Y�[���͈͐���

            map.UpdateMap(map.CenterLatitudeLongitude, newZoom);
        }
    }
}
