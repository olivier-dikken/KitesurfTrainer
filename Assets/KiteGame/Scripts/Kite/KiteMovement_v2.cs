using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// drag
/// lift from coanda effect(air passing fast above kite)
/// lift from kite redirecting the air
/// lift from leading edge coanda effect
/// </summary>
public class KiteMovement_v2 : MonoBehaviour
{
    //needed variables
    public float ForceScale;
    
    public float lineLength = 25;
    public float kiteBaseAngle = 10;

    public float correctedLiftFwdAngle = 20;
    
    public float dragCoef = 1;
    public float liftCoandaEffectCoef = 1;
    public float liftRedirectAirCoef = 1;
    public float liftLeadingEdgeCoandaEffectCoef = 1;

    public Wind theWind;
    public float apparentWindScaler = 1;

    public Transform LE_left;
    public Transform LE_right;
    public Transform LE;

    public Transform harnessTransform;


    //shared values / force computation results
    Vector3 apparentWind = Vector3.zero;
    float AOA = 0f;
    Vector3 liftCoandaEffect = Vector3.zero;
    Vector3 drag = Vector3.zero;
    Vector3 liftRedirectAir = Vector3.zero;
    Vector3 liftLeadingEdgeCoandaEffect = Vector3.zero;
    Vector3 totalForce = Vector3.zero;

    Vector3 previousMove = Vector3.zero;

    Vector3 getApparentWind(Vector3 windVector, Vector3 kiteMove)
    {
        //Debug.Log("Wind mag: " + windVector.magnitude);
        //Debug.Log("KiteMove mag: " + kiteMove.magnitude);
        //TODO check apparent wind units/scale
        Vector3 result = windVector - (kiteMove * apparentWindScaler);
        //Debug.Log("apparent wind mag: " + result.magnitude);
        return result;
    }

    float getAOA(Vector3 apparentWind, Transform kiteTransform)
    {
        //Vector3 locationOnPlane = Vector3.ProjectOnPlane(apparentWind, -kiteTransform.up);        

        float AOA = Vector3.Angle(apparentWind, -kiteTransform.forward);
        if (AOA > 90 || AOA < 0)
        {
            Debug.Log("Angle of attack out of bounds!!! AOA : " + AOA.ToString());
        }

        return AOA;
    }

    //direction orthogonal to wind, in dir of leading edge up
    Vector3 getLiftCoandaEffect(Vector3 apparentWind, float AOA, Transform kiteTranform)
    {
        //use custom AOA

        Vector3 liftDirection = Vector3.ProjectOnPlane(this.transform.up, apparentWind).normalized;
        Vector3 fwdOnPlane = Vector3.ProjectOnPlane(this.transform.forward, apparentWind).normalized;

        Vector3 correctedLiftDirection = (((90 - correctedLiftFwdAngle) / 90) * liftDirection + (correctedLiftFwdAngle / 90) * fwdOnPlane).normalized;

        Vector3 pop_right = Vector3.ProjectOnPlane(this.transform.right, apparentWind);
        Vector3 pop_left = Vector3.ProjectOnPlane(this.transform.forward, apparentWind);
        float projectedSurfaceArea = pop_right.magnitude * Vector3.ProjectOnPlane(pop_left, pop_right).magnitude;

        Debug.Log("lift projected surface area: " + projectedSurfaceArea);

        float ceAngleMult = Mathf.Max(Mathf.Sin(Mathf.Deg2Rad * AOA * 2), 0);
        Debug.Log("liftCoandaEffect angle mult: " + ceAngleMult.ToString());

        Vector3 coandaEffect = correctedLiftDirection * Mathf.Pow(apparentWind.magnitude, 2) * Mathf.PI * ceAngleMult * projectedSurfaceArea;
        return coandaEffect * liftCoandaEffectCoef;        
    }


    /// <summary>
    /// Drag AOA is different, uses entire surface AOA and not necessarily in forward direction of kite
    /// </summary>
    /// <param name="apparentWind"></param>
    /// <param name="AOA"></param>
    /// <returns></returns>
    Vector3 getDrag(Vector3 apparentWind, float AOA)
    {
        float dragAOA = Vector3.Angle(apparentWind, Vector3.ProjectOnPlane(apparentWind, this.transform.up));        

        Vector3 pop_right = Vector3.ProjectOnPlane(this.transform.right, apparentWind);
        Vector3 pop_left = Vector3.ProjectOnPlane(this.transform.forward, apparentWind);
        float projectedSurfaceArea = pop_right.magnitude * Vector3.ProjectOnPlane(pop_left, pop_right).magnitude;

        float dweAngleMult = Mathf.Sin(Mathf.Deg2Rad * dragAOA);
        
        Vector3 effect = apparentWind.normalized * Mathf.Pow(apparentWind.magnitude, 2) * 1.28f * dweAngleMult * projectedSurfaceArea;
        return effect * dragCoef;        
    }

    Vector3 getLiftRedirectAir(Vector3 apparentWind, float AOA)
    {

        return Vector3.zero;
    }

    Vector3 getLiftLeadingEdgeCoandaEffect(Vector3 apparentWind, float AOA)
    {
        Vector3 result = Vector3.zero;
        if(0 < AOA && AOA < 180)
        {
            Vector3 locationOnPlane_fwd = Vector3.ProjectOnPlane(LE.forward, apparentWind);
            Vector3 locationOnPlane_right = Vector3.ProjectOnPlane(LE.up, apparentWind);

            //on plane of apparent wind normal: direction is 90* angle to projection of LE_right 
            Vector3 force = Quaternion.AngleAxis(90, apparentWind) * locationOnPlane_right;

            Vector3 locationOnPlane_force = Vector3.ProjectOnPlane(force, apparentWind);
            if (Vector3.Dot(locationOnPlane_force, locationOnPlane_fwd) < 0)
            {
                force = force * -1;
            }

            //total power depends on LE_up projection on plane size
            result = (Mathf.Pow(apparentWind.magnitude, 2) * force) * liftCoandaEffectCoef;            
            Debug.Log("lift leading edge magnitude: " + result.magnitude);
        }
        return result;
    }
    

    private void CalculateKiteForces()
    {
        apparentWind = getApparentWind(theWind.getWindVector(), previousMove);
        AOA = getAOA(apparentWind, this.transform);
        Debug.Log("angle of attack: " + AOA);

        liftCoandaEffect = getLiftCoandaEffect(apparentWind, AOA, this.transform);
        drag = getDrag(apparentWind, AOA);
        liftRedirectAir = getLiftRedirectAir(apparentWind, AOA);
        liftLeadingEdgeCoandaEffect = getLiftLeadingEdgeCoandaEffect(apparentWind, AOA);

        Vector3 tempForce = liftCoandaEffect + drag + liftLeadingEdgeCoandaEffect;
        totalForce = tempForce * ForceScale;
    }


    private void FixedUpdate()
    {
        CalculateKiteForces();

        MoveKite(totalForce);

        SteerKite();        
    }

    private void MoveKite(Vector3 totalForceOnKite)
    {
        //find location to move kite to, vector from harness to kite.position + totalForce and reduce magnitude to 25 meters
        Vector3 wantedPosition = this.transform.position + totalForceOnKite;
        Vector3 harnassToWantedPosition = wantedPosition - harnessTransform.position;
        Vector3 newPosition = Vector3.zero;
        if (harnassToWantedPosition.magnitude > lineLength)//restricted by line length
        {
            newPosition = harnassToWantedPosition.normalized * lineLength;
        } else
        {
            newPosition = harnassToWantedPosition;
        }        

        //now move the kite towards the position
        Vector3 moveTowards = newPosition - this.transform.position;
        previousMove = moveTowards;
        this.transform.position = this.transform.position + (previousMove * Time.fixedDeltaTime);
    }

    private void SteerKite()
    {
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

        //rotate kite around x-axis to have 90-kiteBaseAngle degree angle to harness
        this.transform.RotateAround(this.transform.position, this.transform.right, angleKiteHarness - 90 - kiteBaseAngle);

        float sideAngleKiteHarness = Vector3.Angle(this.transform.right, kiteToHarness);
        this.transform.RotateAround(this.transform.position, this.transform.forward, -sideAngleKiteHarness + 90);

    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            CalculateKiteForces(); //to draw gizmos
        }

        //where to draw forces?
        Vector3 drawPosition = this.transform.position;

        //drawForceGizmosAt(drawPosition);
        drawForceGizmosAt(Vector3.zero);

        DrawArrow.ForGizmo(Vector3.zero, apparentWind, Color.red);
    }

    private void drawForceGizmosAt(Vector3 drawPosition)
    {
        DrawArrow.ForGizmo(drawPosition, liftCoandaEffect, Color.red);
        DrawArrow.ForGizmo(drawPosition, drag, Color.blue);
        DrawArrow.ForGizmo(drawPosition, liftLeadingEdgeCoandaEffect, Color.yellow);
       // DrawArrow.ForGizmo(drawPosition, liftRedirectAir, Color.yellow);

        DrawArrow.ForGizmo(drawPosition, totalForce, Color.magenta);
    }
}
