using UnityEngine;

public class Test : MonoBehaviour
{
    Material material;

    void Start()
    {
        material.SetColor("_Color", Color.white); // ������������� ����� ���� � ��������
        float height = material.GetFloat("_MixValue"); // ��������� �������� ��������� ���������� �� ���������
    }

    void Update()
    {
        
    }
}
