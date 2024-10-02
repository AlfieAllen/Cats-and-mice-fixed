using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum MovementType
    {
        moving, 
        jerk    
    }

    public bool moveForward = true; 
    public MovementType type = MovementType.moving;
    public MovementPath myPath;
    public float speed = 1f;
    public float maxDistance = .1f;
    public int nextPointIndex = 0;

    public Transform CurrentPoint;  

    void Start()
    {
        //if (myPath == null) 
        //{
        //    Debug.Log("Path is null");
        //    return;  
        //}

        //nextPointIndex = -1;++++++++++++++++++++++++++++++++++++++
        //NextMove();

        //if(_pointInPath == null)
        //{
        //    Debug.Log("Points is null");
        //    return;
        //}

        //transform.position = _pointInPath.position; 
    }

    void Update()
    {
        //update
        Movement();
        CheckForSwitch(); // check the keystroke to switch
    }

    private void Movement()
    {
        if (CurrentPoint == null)
        {
            Debug.Log("AAAA");
            Debug.Log(CurrentPoint.name);
            return;
        }

        if (type == MovementType.moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, CurrentPoint.position, Time.deltaTime * speed);
        }
        else if (type == MovementType.jerk)
        {
            transform.position = Vector3.Lerp(transform.position, CurrentPoint.position, Time.deltaTime * speed);
        }

        float distannceSqure = (transform.position - CurrentPoint.position).sqrMagnitude;

        if (distannceSqure < maxDistance * maxDistance)
        {
            NextMove();
        }
    }

    public void NextMove()
    {
        if (moveForward) 
        {
            if (CurrentPoint == myPath.PathElements[myPath.PathElements.Length - 1])
            {
                Transition(myPath.ForwardPathStartPoint);
                return;
            }
        }
        else
        {
            if (CurrentPoint == myPath.PathElements[0])
            {
                Transition(myPath.BackwardPathStartPoint);
                return;
            }
        }

        CurrentPoint = GetNextPathPoint();
    }

    

    public void Transition(Transform newPathFirstPoint)
    {
        myPath = newPathFirstPoint.GetComponentInParent<MovementPath>();
        if (myPath == null)
        {
            Debug.Log("Recieved point has no MovementPath");
            return;
        }


        if (myPath.PathElements[0] == newPathFirstPoint)
        {
            moveForward = true;
            nextPointIndex = 0;
        }
        else
        {
            moveForward = false;
            nextPointIndex = myPath.PathElements.Length - 1;
            
        }

        CurrentPoint = newPathFirstPoint;
    }

    public Transform GetNextPathPoint()
    {
        if (moveForward == true)
        {
            nextPointIndex++;
        }
        else
        {
            nextPointIndex--;
        }

        return myPath.PathElements[nextPointIndex];
    }

    private int switchPressCount = 0; // ������� ������� ������

    private void CheckForSwitch()
    {
        // �������� ��� ������� ���� Intersection
        Intersection[] intersections = FindObjectsOfType<Intersection>();

        foreach (var intersection in intersections)
        {
            if (Input.GetKeyDown(intersection.switchKey))
            {
                switchPressCount++; // ����������� ������� �������
                SwitchPath(intersection.GetAvailablePaths());
            }
        }
    }

    private void SwitchPath(MovementPath[] newPaths)
    {
        if (newPaths.Length > 0)
        {
            // ��������� ������ ���������� ���� �� ������ �������� �������
            int nextPathIndex = switchPressCount % newPaths.Length; // ����������� ����� ����

            myPath = newPaths[nextPathIndex]; // ������������� �� ���� �� �������

            // ������������� nextPointIndex � 0 � ��������� ������� �����
            nextPointIndex = 0; // �������� � ������ ����� ������ ����
            CurrentPoint = myPath.PathElements[nextPointIndex]; // ������������� ����� ������� �����

            // �������������, ���� ������ ��������� ������� ���������, ����� ����� ��������� ����� �� ����� ����:
            float closestDistance = float.MaxValue;
            int closestIndex = 0;

            // ���� ��������� ����� �� ����� ����
            for (int i = 0; i < myPath.PathElements.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, myPath.PathElements[i].position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            // ������������� ������� ������� �� ��������� ����� �� ����� ����
            transform.position = myPath.PathElements[closestIndex].position;
            nextPointIndex = closestIndex; // ������������� ������ �� ��������� �����
            CurrentPoint = myPath.PathElements[nextPointIndex]; // ������������� ������� �����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ����������� �� � ��������, �� ������� ���� ������ Intersection
        Intersection intersection = other.GetComponent<Intersection>();
        if (intersection != null)
        {
            // �������� ����� � Intersection ��� ���������� ���������
            intersection.OnUnitEnter(this);
        }
    }


}
