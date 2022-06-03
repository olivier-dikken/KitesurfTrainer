using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleKiteEffects : MonoBehaviour
{
    //needed variables
    public float ForceScale;
    public Transform harnessTransform;
    public float lineLength = 25;
    public Wind theWind;    
    public float dwCoef = 1;
    public float liftCoef = 1;
    public float fwdPushforce;
    public float dwBase = 1;

    public float apparentWindScaler;

    public Transform LE_left;
    public Transform LE_right;
    public Transform LE;


    //shared values / force computation results
    Vector3 apparentWind = Vector3.zero;
    float AOA = 0f;
    Vector3 CE = Vector3.zero;
    Vector3 DWE = Vector3.zero;
    Vector3 lift = Vector3.zero;
    Vector3 fwdPush = Vector3.zero;
    Vector3 totalForce = Vector3.zero;

    Vector3 previousMove = Vector3.zero;

    //direction orthogonal to wind, in dir of leading edge up
    Vector3 CoandaEffect(Vector3 apparentWind, float AOA, Transform kiteTranform)
    {
        Debug.Log("Coanda Effect debug. AOA : " + AOA.ToString());

        Vector3 liftDirection = Vector3.ProjectOnPlane(this.transform.up, apparentWind).normalized;
        
        float projectedSurfaceArea_X = Vector3.ProjectOnPlane(this.transform.right, apparentWind).magnitude;
        float projectedSurfaceArea_y = Vector3.ProjectOnPlane(this.transform.forward, apparentWind).magnitude;
        float projectedSurfaceArea = projectedSurfaceArea_X * projectedSurfaceArea_y;

        float ceAngleMult = Mathf.Deg2Rad * AOA;
        Debug.Log("ce angle mult: " + ceAngleMult.ToString());

        Vector3 coandaEffect = liftDirection * Mathf.Pow(apparentWind.magnitude, 2) * 2 * Mathf.PI * ceAngleMult * projectedSurfaceArea * 0.5f;
        return coandaEffect * liftCoef;
    }

    Vector3 DownWindEffect(Vector3 apparentWind, float AOA)
    {
        float projectedSurfaceArea_X = Vector3.ProjectOnPlane(this.transform.right, apparentWind).magnitude;
        float projectedSurfaceArea_y = Vector3.ProjectOnPlane(this.transform.forward, apparentWind).magnitude;
        float projectedSurfaceArea = projectedSurfaceArea_X * projectedSurfaceArea_y;

        Debug.Log("projected surface area: " + projectedSurfaceArea);

        float dweAngleMult = Mathf.Sin(Mathf.Deg2Rad * AOA);
        Debug.Log("dwe angle mult: " + dweAngleMult.ToString());
        Vector3 effect = apparentWind.normalized * Mathf.Pow(apparentWind.magnitude, 2) * 1.28f * dweAngleMult * projectedSurfaceArea * 0.5f;
        return effect * dwCoef + effect.normalized * dwBase;
    }

    Vector3 GetLeadingEdgeForwardPush(Vector3 apparentWind, float AOA)
    {
        if(AOA > -5 && AOA < 275)
        {
            Vector3 liftDirection = Vector3.ProjectOnPlane(this.transform.forward, apparentWind).normalized;
            return liftDirection * fwdPushforce * Mathf.Pow(apparentWind.magnitude, 2) * (Mathf.Min(AOA,45))/45;
        } else
        {
            Debug.Log("No leading edge fwd push at this AOA");
            return Vector3.zero;
        }
        
    }

    Vector3 getApparentWind(Vector3 windVector, Vector3 kiteMove)
    {
        return windVector - kiteMove * apparentWindScaler;
    }

    float getAOA(Vector3 apparentWind, Transform kiteTransform)
    {
        //Vector3 locationOnPlane = Vector3.ProjectOnPlane(apparentWind, -kiteTransform.up);        

        float AOA = Vector3.Angle(apparentWind, kiteTransform.forward);
        if (AOA > 90 || AOA < 0)
        {
            Debug.Log("Angle of attack out of bounds!!! AOA : " + AOA.ToString());
        }

        return AOA;
    }
    

    private void CalculateKiteForces()
    {
        apparentWind = getApparentWind(theWind.getWindVector(), previousMove/Time.fixedDeltaTime);
        AOA = getAOA(apparentWind, this.transform);
        CE = CoandaEffect(apparentWind, AOA, this.transform);
        DWE = DownWindEffect(apparentWind, AOA);
        fwdPush = GetLeadingEdgeForwardPush(apparentWind, AOA);

        Vector3 tempForce = DWE + CE + fwdPush;
        totalForce = tempForce * ForceScale;
    }


    private void FixedUpdate()
    {
        CalculateKiteForces();

        //find location to move kite to, vector from harness to kite.position + totalForce and reduce magnitude to 25 meters
        Vector3 wantedPosition = this.transform.position + totalForce;
        Vector3 harnassToWantedPosition = wantedPosition - harnessTransform.position;
        Vector3 newPosition = harnassToWantedPosition.normalized * lineLength;

        //now move the kite to the position
        Vector3 moveTowards = newPosition - this.transform.position;
        previousMove = moveTowards * Time.fixedDeltaTime;
        this.transform.position = this.transform.position + previousMove;        


        float addRotation = 0;
        if (Input.GetKey("left"))
        {
            //add angular rotation
            addRotation += 2;
        }
        if (Input.GetKey("right"))
        {
            //add angular rotation
            addRotation -= 2;
        }
        //this.transform.Rotate(new Vector3(0, addRotation, 0));// Quaternion.AngleAxis(addRotation, this.transform.up);
        this.transform.RotateAround(this.transform.position, this.transform.up, addRotation);


        RotateKiteTowardsHarness(harnessTransform.position);
    }

    /// <summary>
    /// with base angle beign 5 degrees (and when bar fully pulled in, 20 degrees)
    /// </summary>
    /// <param name="harnessPosition"></param>
    private void RotateKiteTowardsHarness(Vector3 harnessPosition)
    {
        //get angle between kiteforward and harness
        Vector3 kiteToHarness = harnessPosition - this.transform.position;
        float angleKiteHarness = Vector3.Angle(this.transform.forward, kiteToHarness);        
        Debug.Log("kite-harness angle: " + angleKiteHarness.ToString());

        //rotate kite around x-axis to have 90 degree angle to harness
        //this.transform.Rotate(new Vector3(angleKiteHarness - 90 + 20, 0, 0));
        this.transform.RotateAround(this.transform.position, this.transform.right, angleKiteHarness - 90 - 10);

        float sideAngleKiteHarness = Vector3.Angle(this.transform.right, kiteToHarness);
        this.transform.RotateAround(this.transform.position, this.transform.forward, -sideAngleKiteHarness + 90);
        
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            CalculateKiteForces(); //to draw gizmos
        }
        DrawArrow.ForGizmo(this.transform.position, CE, Color.red);
        DrawArrow.ForGizmo(this.transform.position, DWE, Color.blue);
        DrawArrow.ForGizmo(this.transform.position, fwdPush, Color.grey);
        DrawArrow.ForGizmo(this.transform.position, totalForce, Color.magenta);

        DrawArrow.ForGizmo(Vector3.zero, apparentWind, Color.magenta);
    }
}
