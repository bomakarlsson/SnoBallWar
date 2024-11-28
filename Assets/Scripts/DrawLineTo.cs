using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawLineTo : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Vector3 target;

    void Awake()
    {
        DisableLine();
    }

    void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void OnDisable()
    {
        DisableLine();
    }

    void DisableLine()
    {
        lineRenderer.enabled = false;
    }
    
    public void DrawLineToTarget()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target);
    }

    void Update()
    {
        DrawLineToTarget();
    }
}
