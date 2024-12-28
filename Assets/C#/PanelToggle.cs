using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        // �v���W�F�N�g���J�n���ꂽ�Ƃ��Ƀp�l�����\���ɂ���
        panel.SetActive(false);
    }

    public void TogglePanel()
    {
        // �{�^�����N���b�N���ꂽ�Ƃ��Ƀp�l���̕\���E��\����؂�ւ���
        panel.SetActive(!panel.activeSelf);
    }
}
