using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum MovementType
    {
        moveing, // ���������� �������� 
        jerk    // �����
    }

    public MovementType type = MovementType.moveing;
    public MovementPath myPath;
    public float speed = 1f;
    public float maxDistance = .1f; // �� ����� ���������� ������ ��������� ������ � �����, ����� ������ ��� ��� �����.

    private IEnumerator<Transform> _pointInPath;  // �������� �����

    // Start is called before the first frame update
    void Start()
    {
        if (myPath == null)  // �������� �� ������� ����
        {
            Debug.Log("������� ����");
            return;  
        }

        _pointInPath = myPath.GetNextPathPoint();
        _pointInPath.MoveNext();           // �������� ���������� � ��������� ����� � ����

        if(_pointInPath.Current == null)
        {
            Debug.Log("����� �����");
            return;
        }

        transform.position = _pointInPath.Current.position; // ������ ������ �� ��������� ����� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(_pointInPath == null || _pointInPath.Current == null)
        {
            return;
        }

        if(type == MovementType.moveing)
        {
            transform.position = Vector3.MoveTowards(transform.position, _pointInPath.Current.position, Time.deltaTime * speed);
        }
        else if(type == MovementType.jerk)
        {
            transform.position = Vector3.Lerp(transform.position, _pointInPath.Current.position, Time.deltaTime * speed);
        }

        float distannceSqure = (transform.position - _pointInPath.Current.position).sqrMagnitude;

        if(distannceSqure < maxDistance * maxDistance)  // ���������� ������ � �����?
        {
            _pointInPath.MoveNext();
        }
    }
}
