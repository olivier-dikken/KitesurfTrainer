//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class KiteEffects : MonoBehaviour
//{
//    //needed variables
//    public Transform harnessTransform;
//    public float lineLength = 25;
//    public Wind theWind;
//    public float outerWindBaseBias; //wind created from leading edge round shape; basically if AOA is 90 degrees then the round shape of the leading edge still pushes the kite a bit in the fwd direction
//    public float downWindBaseBias; //down wind effect at angle 0
//    public float coandaEffectLiftAngle;
//    public float coandaCoef = 1;
//    public float dwCoef = 1;
//    public float liftCoef = 1;
//    public float kiteEffectCoef = 1;


//    //shared values / force computation results
//    Vector3 apparentWind = Vector3.zero;
//    float AOA = 0f;
//    Vector3 CE = Vector3.zero;    
//    Vector3 DWE = Vector3.zero;    
//    Vector3 lift = Vector3.zero;
//    Vector3 totalForce = Vector3.zero;


//    Vector3 CoandaEffect(Vector3 apparentWind, float AOA, Transform kiteTranform)
//    {
//        Debug.Log("Coanda Effect debug. AOA : " + AOA.ToString());
        
//        float outerWind = (apparentWind.magnitude * (90-AOA) / 90); 
//        float outerWindLeadingEdgeBias = outerWindBaseBias * (AOA/90);
//        float totalOuterWind = outerWindLeadingEdgeBias + outerWind;
//        //float totalOuterWind = outerWind;
//        Debug.Log("outerWind : " + outerWind.ToString());

//        Vector3 coandaEffect = Quaternion.AngleAxis(-coandaEffectLiftAngle, Vector3.right) * kiteTranform.forward * coandaCoef * Mathf.Pow(totalOuterWind,1);
//        return coandaEffect;
//    }

//    Vector3 DownWindEffect(Vector3 apparentWind, float AOA)
//    {
//        float angleMultiplier = AOA / 90;
//        float downWindBias = downWindBaseBias * (1 - angleMultiplier);
//        return apparentWind * angleMultiplier * dwCoef + apparentWind.normalized * downWindBaseBias;
//    }

//    Vector3 getApparentWind(Vector3 windVector, Vector3 kiteVector)
//    {
//        return windVector - kiteVector;
//    }

//    float getAOA(Vector3 apparentWind, Transform kiteTransform)
//    {
//        Vector3 locationOnPlane = Vector3.ProjectOnPlane(apparentWind, -kiteTransform.up);
//        //Gizmos.DrawSphere(locationOnPlane, 0.2f);

//        float AOA = Vector3.Angle(apparentWind, locationOnPlane);
//        if (AOA > 90 || AOA < 0)
//        {
//            Debug.Log("Angle of attack out of bounds!!! AOA : " + AOA.ToString());
//        }

//        return AOA;
//    }

//    Vector3 getLift(Vector3 apparentWind, float AOA)
//    {
//        //using intuition to make formulas
//        float angleMult = 1-(Mathf.Abs(AOA-45) / 45);
//        return angleMult * apparentWind.magnitude * this.transform.up * liftCoef;
//    }

//    private void OnDrawGizmos()
//    {             
//        if(!Application.isPlaying)
//        {
//            CalculateKiteForces(); //to draw gizmos
//        }
//        DrawArrow.ForGizmo(this.transform.position, CE, Color.red);
//        DrawArrow.ForGizmo(this.transform.position, DWE, Color.blue);
//        DrawArrow.ForGizmo(this.transform.position, lift, Color.yellow);
//        DrawArrow.ForGizmo(this.transform.position, totalForce, Color.magenta);
//    }

//    private void CalculateKiteForces()
//    {
//        apparentWind = getApparentWind(theWind.getWindVector(), Vector3.zero);
//        AOA = getAOA(apparentWind, this.transform);
//        CE = CoandaEffect(apparentWind, AOA, this.transform);
//        DWE = DownWindEffect(apparentWind, AOA);
//        lift = getLift(apparentWind, AOA);

//        Vector3 tempForce = DWE + CE + lift;
//        tempForce = tempForce * 0.5f;

//        if(Vector3.Dot(tempForce, apparentWind) < 0)
//        {
//            //remove wind in direction of totalForce
//            Vector3 windToRemove = Vector3.Project(tempForce, apparentWind);

//            Debug.Log("windToRemove magnitude: " + windToRemove.magnitude);
//            totalForce = tempForce + windToRemove;
//        } else
//        {            
//            totalForce = tempForce;
//        }        
//    }

//    private void Update()
//    {
//        CalculateKiteForces();

//        //gameObject.GetComponent<Rigidbody>().AddForce(totalForce);

//        //find location to move kite to, vector from harness to kite.position + totalForce and reduce magnitude to 25 meters
//        Vector3 wantedPosition = this.transform.position + (totalForce * kiteEffectCoef);
//        Vector3 harnassToWantedPosition = wantedPosition - harnessTransform.position;
//        Vector3 newPosition = harnassToWantedPosition.normalized* lineLength;

//        //now move the kite to the position
//        this.transform.position = newPosition;

//        RotateKiteTowardsHarness(harnessTransform.position);
//    }

//    private void RotateKiteTowardsHarness(Vector3 harnessPosition)
//    {
//        //get angle between kiteforward and harness
//        Vector3 kiteToHarness = harnessPosition - this.transform.position;
//        float angleKiteHarness = Vector3.Angle(this.transform.forward, kiteToHarness);
//        Debug.Log("kite-harness angle: " + angleKiteHarness.ToString());

//        //rotate kite around x-axis to have 90 degree angle to harness
//        //Quaternion rotationToHarness = Quaternion.AngleAxis(angleKiteHarness - 90, Vector3.right);
//        this.transform.Rotate(new Vector3(angleKiteHarness - 90, 0, 0));
//    }
//}
