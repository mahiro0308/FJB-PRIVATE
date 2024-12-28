using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    private PlaceIndicator placeIndicator; // FindObjectOfType�Ŏ擾���邽�߂̕ϐ�
    public GameObject objectToPlace;       // �z�u����I�u�W�F�N�g
    private GameObject currentObject;      // ���ݔz�u����Ă���I�u�W�F�N�g

    void Start()
    {
        // PlaceIndicator������
        placeIndicator = FindObjectOfType<PlaceIndicator>();
    }

    // �{�^�����������Ƃ��ɌĂяo�����z�u����
    public void ClickToPlace()
    {
        // ���ݔz�u����Ă���I�u�W�F�N�g������΍폜
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        // PlaceIndicator�̈ʒu�Ɖ�]�Ɋ�Â��ĐV�����I�u�W�F�N�g��z�u
        currentObject = Instantiate(objectToPlace, placeIndicator.transform.position, placeIndicator.transform.rotation);
    }
}
