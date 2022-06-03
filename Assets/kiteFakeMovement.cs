using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kiteFakeMovement : MonoBehaviour
{
    //needed variables

    public float lineLength = 25;
    public float kiteBaseAngle = 10;

    public float speed = 10;
    
    public Wind theWind;
    public float apparentWindScaler = 1;
    
    public Transform LE;

    public Transform harnessTransform;


    //shared values / force computation results
    Vector3 apparentWind = Vector3.zero;

    Vector3 previousMove = Vector3.zero;

    float previousAngularRotation = 0f;
    public float maxRotationPower = 100;
    public float rotationBasePower = 20;
    public float rotationSpeedScalar = 3;
    public float rotationAlpha = 0.3f;

    private void FixedUpdate()
    {
        //move direction between kite.up and kite.fwd (close to kite.up, but angled fwd slightly)
        //cant go past 25M lines
        //move direction can't go past AW plane (can't go straight into the apparent wind)
        //add move power every update:
        // - kite_load depends on previous_movement, AW magnitude & projection, lines must be fully extended
        // - kite_movement dimmed by friction/drag (not exceed AW speed?)
        // - kite_movement depends on previous_movement

        apparentWind = getApparentWind(theWind.getWindVector(), previousMove);

        Vector3 harnessToKiteDirection = -1*(harnessTransform.position - this.transform.position).normalized;
        Vector3 moveTowards = speed * (this.transform.forward * (1 + previousMove.magnitude* 0.99f) + harnessToKiteDirection * 10);
        //if move towards wind, project on plant of AW so can't move straight into wind
        if(Vector3.Dot(apparentWind, moveTowards) < 0)
        {
            moveTowards = Vector3.ProjectOnPlane(moveTowards, apparentWind);
        }
        MoveKite(moveTowards);

        //steer depending on input controls
        SteerKite();
    }

    Vector3 getApparentWind(Vector3 windVector, Vector3 kiteMove)
    {
        Vector3 result = windVector - (kiteMove * apparentWindScaler);
        return result;
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
        }
        else
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
        //rotation speed scales with kite speed and a bit with previous rotation        
        float speed = previousMove.magnitude + rotationAlpha * Mathf.Abs(previousAngularRotation);        
        float rotationPower = Mathf.Min(rotationBasePower + rotationSpeedScalar*speed, maxRotationPower);
        float addRotation = 0;
        
        if (Input.GetKey("left"))
        {
            //add angular rotation
            addRotation += rotationPower * Time.fixedDeltaTime;
        }
        if (Input.GetKey("right"))
        {
            //add angular rotation
            addRotation -= rotationPower * Time.fixedDeltaTime;
        }

        previousAngularRotation = addRotation;
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
        // DrawArrow.ForGizmo(Vector3.zero, apparentWind, Color.yellow);
        // DrawArrow.ForGizmo(Vector3.zero, previousMove, Color.magenta);
    }
}
