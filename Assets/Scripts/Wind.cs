using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    public float WindPower;

    public Vector3 getWindVector()
    {
        return this.transform.forward * WindPower;
    }


    private void OnDrawGizmos()
    {
        DrawArrow.ForGizmo(this.transform.position, this.transform.forward * WindPower, Color.magenta);
    }
}
