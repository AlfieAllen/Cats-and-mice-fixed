using UnityEngine;

public class Intersection : MonoBehaviour
{
    public MovementPath[] availablePaths; // ��������� ���� �� �����������
    private int currentPathIndex;
    public KeyCode switchKey; // ������� ��� ������������ �� ���� �����������

    // ����� ��� ��������� ��������� �����
    public MovementPath[] GetAvailablePaths()
    {
        return availablePaths;
    }

    private void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            if (currentPathIndex >= availablePaths.Length - 1) 
            {
                currentPathIndex++;
            }
            
        }
    }

    public Transform CurrentPath()
    {
        return availablePaths[currentPathIndex].transform;
    }

    public void OnUnitEnter(Unit unit)
    {
       
    }
}
