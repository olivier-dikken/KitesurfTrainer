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

        float outerWind = (apparentWind.magnitude * (90 - AOA) / 90);
        float totalOuterWind = outerWind;
        //float totalOuterWind = outerWind;
        Debug.Log("outerWind : " + outerWind.ToString());

        //get projection of leading edge normal on wind orthogonal plane
        Vector3 liftDirection = Vector3.ProjectOnPlane(LE.right, apparentWind).normalized;
        
        float projectedSurfaceArea_X = Vector3.ProjectOnPlane(this.transform.right, apparentWind).magnitude;
        float projectedSurfaceArea_y = Vector3.ProjectOnPlane(this.transform.forward, apparentWind).magnitude;
        float projectedSurfaceArea = projectedSurfaceArea_X * projectedSurfaceArea_y;

        Vector3 coandaEffect = liftDirection * Mathf.Pow(totalOuterWind, 2);// * 2 * Mathf.PI * Mathf.Deg2Rad * AOA * projectedSurfaceArea;
        return coandaEffect;
    }

    Vector3 DownWindEffect(Vector3 apparentWind, float AOA)
    {
        //float angleMultiplier = AOA / 90;
        //Vector3 friction = Mathf.Pow(apparentWind.magnitude, 2) * apparentWind.normalized * dwBase;
        //return Mathf.Pow(apparentWind.magnitude, 2) * apparentWind.normalized * angleMultiplier * dwCoef + friction;
        float projectedSurfaceArea_X = Vector3.ProjectOnPlane(this.transform.right, apparentWind).magnitude;
        float projectedSurfaceArea_y = Vector3.ProjectOnPlane(this.transform.forward, apparentWind).magnitude;
        float projectedSurfaceArea = projectedSurfaceArea_X * projectedSurfaceArea_y;

        Vector3 effect = apparentWind.normalized * 1.28f * Mathf.Sin(Mathf.Deg2Rad * AOA) * projectedSurfaceArea;
        return effect;
    }

    Vector3 GetLeadingEdgeForwardPush(Vector3 apparentWind, float AOA)
    {        
        float angleMultiplier = Mathf.Min(AOA + 45, 90)  / 90;        
        return this.transform.forward * fwdPushforce * angleMultiplier;
    }

    Vector3 getApparentWind(Vector3 windVector, Vector3 kiteMoveDelta)
    {
        return windVector;// + kiteMoveDelta/Time.fixedDeltaTime;
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
    

    private void CalculateKiteForces()
    {
        apparentWind = getApparentWind(theWind.getWindVector(), previousMove);
        AOA = getAOA(apparentWind, this.transform);
        CE = CoandaEffect(apparentWind, AOA, this.transform);
        DWE = DownWindEffect(apparentWind, AOA);
        fwdPush = GetLeadingEdgeForwardPush(apparentWind, AOA);

        Vector3 tempForce = DWE + CE + fwdPush;
        totalForce = tempForce * ForceScale;
    }

    private void Update()
    {
        
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
        Vector3 previousMove = moveTowards * Time.fixedDeltaTime;
        this.transform.position = this.transform.position + previousMove;

        RotateKiteTowardsHarness(harnessTransform.position);


        float addRotation = 0;
        if (Input.GetKey("left"))
        {
            //add angular rotation
            addRotation += 10;
        }
        if (Input.GetKey("right"))
        {
            //add angular rotation
            addRotation -= 10;
        }
        this.transform.Rotate(new Vector3(0, addRotation, 0));// Quaternion.AngleAxis(addRotation, this.transform.up);

    }

    private void RotateKiteTowardsHarness(Vector3 harnessPosition)
    {
        //get angle between kiteforward and harness
        Vector3 kiteToHarness = harnessPosition - this.transform.position;
        float angleKiteHarness = Vector3.Angle(this.transform.forward, kiteToHarness);
        Debug.Log("kite-harness angle: " + angleKiteHarness.ToString());

        //rotate kite around x-axis to have 90 degree angle to harness
        this.transform.Rotate(new Vector3(angleKiteHarness - 90, 0, 0));
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
    }
}
