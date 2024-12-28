using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;

public class TouchCameraMovementWithButton : MonoBehaviour
{
    public AbstractMap map;          // �}�b�v�ւ̎Q��
    public Camera mainCamera;        // �}�b�v����Ɏg���J����
    public Button backToCurrentLocationButton; // �{�^���ւ̎Q��

    private void Start()
    {
        // �{�^���Ƀ��X�i�[��ǉ�
        backToCurrentLocationButton.onClick.AddListener(MoveToCurrentLocation);

        // ���ݒn�T�[�r�X���J�n
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("�ʒu���T�[�r�X���L���ɂȂ��Ă��܂���B");
            return;
        }

        Input.location.Start(); // �ʒu���̎擾���J�n
    }

    private void Update()
    {
        // ���̃J��������̃R�[�h
    }

    // �{�^�����N���b�N�����猻�ݒn�ɖ߂�
    private void MoveToCurrentLocation()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // ���ݒn���擾
            LocationInfo location = Input.location.lastData;
            Vector2d currentLatLong = new Vector2d(location.latitude, location.longitude);

            // �}�b�v�����ݒn�ɍX�V
            map.UpdateMap(currentLatLong, map.Zoom);
        }
        else
        {
            Debug.LogWarning("���ݒn���擾�ł��܂���B�ʒu���T�[�r�X���L���łȂ��\��������܂��B");
        }
    }

    private void OnDestroy()
    {
        // �ʒu���T�[�r�X���~
        if (Input.location.isEnabledByUser)
        {
            Input.location.Stop();
        }
    }
}
