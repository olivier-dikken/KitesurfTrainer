using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class kiteFakeMovement : MonoBehaviour
{

    public TextMeshProUGUI DebugUIText;
    //needed variables

    public float lineLength = 25;
    public float kiteBaseAngle = 10;

    public float speed = 2;
    
    public Wind theWind;
    public float apparentWindScaler = 0.3f;
    
    public Transform LE;

    public Transform harnessTransform;

    public Harness theHarness;


    //shared values / force computation results
    Vector3 apparentWind = Vector3.zero;

    Vector3 previousMove = Vector3.zero;

    float previousAngularRotation = 0f;
    float maxRotationPower = 340;
    float rotationBasePower = 40;
    float rotationSpeedScaler = 8;
    float rotationAlphaScaler = 3f;

    // float momentumScalar = 5f;

    // float barPowerDirectionChangeScaler = 50f;

    private void FixedUpdate()
    {
        //move direction between kite.up and kite.fwd (close to kite.up, but angled fwd slightly)
        //cant go past 25M lines
        //move direction can't go past AW plane (can't go straight into the apparent wind)
        //add move power every update:
        // - kite_load depends on previous_movement, AW magnitude & projection, lines must be fully extended
        // - kite_movement dimmed by friction/drag (not exceed AW speed?)
        // - kite_movement depends on previous_movement

        float barPower = theHarness.getPower();
        float totalForceBarPower = (1f + 3f * barPower) / 4f;

        apparentWind = getApparentWind(theWind.getWindVector(), previousMove);

        Vector3 harnessToKiteDirection = -1*(harnessTransform.position - this.transform.position).normalized;
        Vector3 moveTowards = totalForceBarPower * speed * apparentWind.magnitude * (this.transform.forward * (1 + previousMove.magnitude) + harnessToKiteDirection * 10);
        //if move towards wind, project on plant of AW so can't move straight into wind
        if(Vector3.Dot(apparentWind, moveTowards) < 0)
        {
            moveTowards = Vector3.ProjectOnPlane(moveTowards, apparentWind);
        }
        MoveKite(moveTowards);

        //steer depending on input controls
        SteerKite(moveTowards);
    }

    Vector3 getApparentWind(Vector3 windVector, Vector3 kiteMove)
    {
        Vector3 result = windVector - (kiteMove * apparentWindScaler);
        return result;
    }

    private void MoveKite(Vector3 totalForceOnKite)
    {
        DebugUIText.text = "totalForceOnKite: " + Mathf.RoundToInt(totalForceOnKite.magnitude).ToString();
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

    private void SteerKite(Vector3 totalForceOnKite)
    {
        float barAngle = theHarness.getAngle();

        //rotation speed scales with kite speed and a bit with previous rotation        
        //float steeringSpeed = rotationSpeedScaler * previousMove.magnitude + rotationAlphaScaler * Mathf.Abs(previousAngularRotation);        
        float steeringSpeed = rotationSpeedScaler * totalForceOnKite.magnitude/10 + rotationAlphaScaler * Mathf.Abs(previousAngularRotation);
        float rotationPower = Mathf.Min(rotationBasePower + steeringSpeed, maxRotationPower);
        float addRotation = 0;

        addRotation = - barAngle * rotationPower * Time.fixedDeltaTime;


        previousAngularRotation = addRotation;
        
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
        DrawArrow.ForGizmo(Vector3.zero, apparentWind, Color.yellow);
        DrawArrow.ForGizmo(Vector3.zero, previousMove, Color.magenta);        
    }
}
