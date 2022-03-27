using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    
    public float desiredLineLength;
    public float actualLineLength;
    public float tensionScale;
    
    private Vector3 _tensionForce;
    public Vector3 lineOrigin;
    public Kite kite;

    public Vector3 GetTensionForce()
    {
        return _tensionForce;
    }
    
    private void OnDrawGizmos()
    {
        // draw line
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lineOrigin, 1);
        Gizmos.DrawSphere(transform.position, 1);
        Gizmos.DrawLine(lineOrigin, transform.position);
        
        // draw force
        DrawArrow.ForGizmo(transform.position, _tensionForce, Color.cyan);
        
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        // vector from line origin to line end
        Vector3 lineDirection = transform.position - lineOrigin;
        
        // get the amount of wind strength in direction of the line
        Vector3 windProjection = Vector3.Project(kite.totalWindForce, lineDirection);
        
        actualLineLength = Vector3.Distance(lineOrigin, transform.position);
        float lineLenDifference = actualLineLength - desiredLineLength;
    
        // force(len_diff) = scale * len_diff^2 - desired_force
        float tensionMagnitude = tensionScale * Mathf.Pow(lineLenDifference, 2) - windProjection.magnitude;
        tensionMagnitude = tensionMagnitude < 0 ? 0 : tensionMagnitude;
        
        _tensionForce = -windProjection.normalized * tensionMagnitude;
        
    }
}
