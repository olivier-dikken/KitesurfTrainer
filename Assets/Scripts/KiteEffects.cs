using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteEffects : MonoBehaviour
{
    //needed variables
    public Wind theWind;
    public float outerWindBaseBias; //wind created from leading edge round shape; basically if AOA is 90 degrees then the round shape of the leading edge still pushes the kite a bit in the fwd direction
    public float coandaEffectLiftAngle;


    Vector3 CoandaEffect(float coandaCoef, Vector3 apparentWind, float AOA, Transform kiteTranform)
    {
        Debug.Log("Coanda Effect debug. AOA : " + AOA.ToString());

        

        float outerWind = (apparentWind.magnitude * (90-AOA) / 90); 
        //float outerWindLeadingEdgeBias = outerWindBaseBias;
        //float totalOuterWind = outerWindLeadingEdgeBias + outerWind;
        float totalOuterWind = outerWind;
        Debug.Log("outerWind : " + outerWind.ToString());

        Vector3 coandaEffect = Quaternion.AngleAxis(-coandaEffectLiftAngle, Vector3.right) * kiteTranform.forward * coandaCoef * totalOuterWind;
        return coandaEffect;
    }

    Vector3 DownWindEffect(Vector3 apparentWind, float AOA)
    {
        float angleMultiplier = AOA / 90;
        return -Vector3.forward;
    }

    Vector3 getApparentWind(Vector3 windVector, Vector3 kiteVector)
    {
        return windVector - kiteVector;
    }

    float getAOA(Vector3 apparentWind, Transform kiteTransform)
    {
        Vector3 locationOnPlane = Vector3.ProjectOnPlane(apparentWind, -kiteTransform.up);
        //Gizmos.DrawSphere(locationOnPlane, 0.2f);

        float AOA = Vector3.Angle(apparentWind, locationOnPlane);
        if (AOA > 90 || AOA < 0)
        {
            Debug.Log("Angle of attack out of bounds!!! AOA : " + AOA.ToString());
        }

        return AOA;
    }

    private void OnDrawGizmos()
    {
        Vector3 apparentWind = getApparentWind(theWind.getWindVector(), Vector3.zero);
        float AOA = getAOA(apparentWind, this.transform);
        Vector3 CE = CoandaEffect(1, apparentWind, AOA, this.transform);
        DrawArrow.ForGizmo(this.transform.position, CE, Color.red);

        Vector3 DWE = DownWindEffect(apparentWind, AOA);
        DrawArrow.ForGizmo(this.transform.position, DWE, Color.red);


        DrawArrow.ForGizmo(this.transform.position, DWE + CE, Color.green);
    }
}
