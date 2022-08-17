using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawUp : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        DrawArrow.ForGizmo(this.transform.position, this.transform.up, Color.black);
    }
}
