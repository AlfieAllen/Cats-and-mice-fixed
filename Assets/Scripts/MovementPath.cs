using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPath : MonoBehaviour
{
   public enum PathTypes
    {
        opened, //����������� 
        closed  // �����������
    }

    public PathTypes pathType;        // ��������� ��� ����
    public int movementDirection = 1; // ����������� �������� 1/-1
    public int moveingTo = 0;          // � ����� ����� ���������
    public Transform[] PathElements;  // ������ �� ����� ��������

    public void OnDrawGizmos()        // ���������� ����� ����� ������� ����
    {
        if (PathElements == null || PathElements.Length < 2) return;

        for (int i = 1; i < PathElements.Length; i++)  // ��� ����� �������
        {
            Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position); // ������ ����� ����� ����� �������
        }

        if(pathType == PathTypes.closed) // ���� ����������� ���
        {
            Gizmos.DrawLine(PathElements[0].position, PathElements[PathElements.Length - 1].position); //������ �� 1 � ���������
        }
    }

    public IEnumerator<Transform> GetNextPathPoint()  //�������� ��������� ��������� �����
    {
        if(PathElements == null || PathElements.Length < 1) // ���� �� ����� ������� ����� ��������� ���������
        {
            yield break;
        }

        while(true)
        {
            yield return PathElements[moveingTo];

            if(PathElements.Length == 1) continue; // ���� ����� ����, �� �����

            if(pathType == PathTypes.opened)     // ���� ����� �� ���������
            {
                if(moveingTo <= 0)  // ���� ��������� �� �����������
                {
                    movementDirection = 1; // +1 � ��������
                }
                else if(moveingTo >= PathElements.Length - 1)  // ���� ��������� �� ���������
                {
                    movementDirection = -1;   
                }
            }

            moveingTo = moveingTo + movementDirection; // �������� �������� �� -1 �� 1

            if(pathType == PathTypes.closed)
            {
                if(moveingTo >= PathElements.Length)
                {
                    moveingTo = 0;
                }

                if(moveingTo < 0)
                {
                    moveingTo = PathElements.Length - 1;
                }
            }
        }
    }
}
