using UnityEngine;

public class Intersection : MonoBehaviour
{
    public MovementPath[] availablePaths; // ��������� ���� �� �����������
    public KeyCode switchKey; // ������� ��� ������������ �� ���� �����������

    // ����� ��� ��������� ��������� �����
    public MovementPath[] GetAvailablePaths()
    {
        return availablePaths;
    }
}
