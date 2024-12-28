using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonColor : MonoBehaviour
{
    public Button button1; // �{�^��1
    public Button button2; // �{�^��2
    private Color defaultColor; // �f�t�H���g�̐F
    private Color selectedColor; // �N���b�N���ꂽ���̐F
    public GameObject panel1;
    public GameObject panel2;

    void Start()
    {
        // �F��ݒ�
        ColorUtility.TryParseHtmlString("#9ADEAE", out selectedColor); // �N���b�N���ꂽ�Ƃ��̐F
        ColorUtility.TryParseHtmlString("#FFFFFF", out defaultColor); // �f�t�H���g�̐F�i���j

        // ������Ԃ� button1 ��I����Ԃɐݒ肵�Abutton2 �̓f�t�H���g�̐F
        button1.GetComponent<Image>().color = selectedColor;
        button2.GetComponent<Image>().color = defaultColor;

        // �{�^���̃N���b�N�C�x���g��ǉ�
        button1.onClick.AddListener(() => OnButtonClick(button1));
        button2.onClick.AddListener(() => OnButtonClick(button2));

        // ������Ԃ� panel1 ��\�����Apanel2 ���\���ɐݒ�
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    void OnButtonClick(Button clickedButton)
    {
        // button1���N���b�N���ꂽ�ꍇ�Abutton1��I��F�ɂ��Abutton2�̓f�t�H���g�F�ɖ߂�
        if (clickedButton == button1)
        {
            button1.GetComponent<Image>().color = selectedColor;
            button2.GetComponent<Image>().color = defaultColor;
            panel1.SetActive(true);
            panel2.SetActive(false);
        }
        // button2���N���b�N���ꂽ�ꍇ�Abutton2��I��F�ɂ��Abutton1�̓f�t�H���g�F�ɖ߂�
        else if (clickedButton == button2)
        {
            button2.GetComponent<Image>().color = selectedColor;
            button1.GetComponent<Image>().color = defaultColor;
            panel1.SetActive(false);
            panel2.SetActive(true);
        }
    }
}
