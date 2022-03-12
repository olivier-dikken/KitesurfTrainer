using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    
    [SerializeField] private float lineLength;
    [SerializeField] private float tensionScale;
    
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
        Vector3 lineDirection = transform.position - lineOrigin;
        Vector3 windProjection = Vector3.Project(kite.totalWindForce, lineDirection);
        
        float currentLineLen = Vector3.Distance(lineOrigin, transform.position);
        float lineLenDifference = (float) (currentLineLen - lineLength);
        _tensionForce = -lineLenDifference * windProjection;
        if (lineLenDifference < 0) _tensionForce = Vector3.zero;
        _tensionForce *= tensionScale;
        
    }
}
