using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineScript : MonoBehaviour
{
    LineRenderer lr;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool drawLine = false;

    [SerializeField] private LayerMask circleLayer;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        lr.positionCount = 2;
    }

    void Update()
    {
        GetInput();
        if (drawLine)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DrawLine();
        }
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            drawLine = true;
            lr.enabled = true;
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0) && drawLine)
        {
            drawLine = false;
            lr.enabled = false;
            RemoveCircles();
        }
    }

    private void DrawLine()
    {
        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, endPoint);
    }

    private void RemoveCircles()
    {
        Vector3 direction = (startPoint - endPoint).normalized;
        float length = (startPoint - endPoint).magnitude;
        RaycastHit2D[] hit2D = Physics2D.RaycastAll(endPoint, direction, length, circleLayer);

        if (hit2D != null)
        {
            foreach (RaycastHit2D hit in hit2D)
            {
                Destroy(hit.transform.gameObject);
                Debug.Log("smack");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPoint, endPoint);
    }
}
