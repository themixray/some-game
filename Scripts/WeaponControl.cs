using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    Transform parent; //�������� ���� �� �������� ��������

    int i, ind = 0; //������ �������� �������� ����

    public int count; //������������ ����� �����

    public GameObject[] ar;  //������ ������

    [SerializeField] GameObject prefab;
    void Start()
    {
        parent = new GameObject().transform;

        ar = new GameObject[count];

        while (i < count)
        {

            ar[i] = Instantiate(prefab, transform.position, transform.rotation, parent);
            ar[i].SetActive(false);
            i++;
        }
    }
    // Update is called once per frame
    void Update()
    {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject obj = parent.GetChild(ind).gameObject;
                obj.SetActive(true);
                obj.transform.position = gameObject.transform.position;
            obj.transform.rotation = gameObject.transform.rotation;
                ind++;
                if (ind > parent.childCount - 1) ind = 0;
            }
    }
}
