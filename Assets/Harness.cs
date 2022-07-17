using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harness : MonoBehaviour
{
    public GameObject bar;

    public Wind theWind;

    public GameObject barLeftTip;
    public GameObject barRightTip;

    public GameObject leftBridle;
    public GameObject rightBridle;

    public LineRenderer lr_left;
    public LineRenderer lr_right;

    public GameObject centerLeftBridle;
    public GameObject centerRightBridle;

    public LineRenderer lr_center_left;
    public LineRenderer lr_center_right;

    float angle = 0f;
    float power = 0f;
    float maxAngle = 70f;

    Vector3 debugArrowStart = Vector3.zero;
    Vector3 debugArrowDirection = Vector3.up;    

    public float getAngle()
    {
        return angle/maxAngle;
    }

    public float getPower()
    {
        return power;
    }

    private void Update()
    {
        drawKiteLine(barRightTip.transform.position, rightBridle.transform.position, lr_right);
        drawKiteLine(barLeftTip.transform.position, leftBridle.transform.position, lr_left);
        drawKiteLine(this.transform.position, centerLeftBridle.transform.position, lr_center_left);
        drawKiteLine(this.transform.position, centerRightBridle.transform.position, lr_center_right);

        
    }

    private void FixedUpdate()
    {
        handleInput();        
        setBarPosition(power, angle);
    }


    private void drawKiteLine(Vector3 origin, Vector3 destination, LineRenderer lr)
    {
        Vector3[] linePositions = new Vector3[2];
        linePositions[0] = origin;
        linePositions[1] = destination;
        lr.SetPositions(linePositions);
    }

    private void setBarPosition(float power, float angle)
    {
        //power between 0 and 1 is bar distance, power =1 --> bar pulled close; power = 0 --> bar fully out/away from person
        //max angle should be 90*, angle of 0 --> bar horizontal; -90 --> left hand full pull; +90 --> right hand full pull

        //get vector from harness to average(centerLeftBridle, centerRightBridle)
        Vector3 centerBridles = (centerLeftBridle.transform.position + centerRightBridle.transform.position) / 2;
        Vector3 barLine = (centerBridles - this.transform.position).normalized * 2;

        Vector3 newBarPositionOnLine = (0.1f + 1 - power) * barLine;
        bar.transform.position = this.transform.position + newBarPositionOnLine;

        //bar.transform.LookAt(centerBridles);
        //bar.transform.Rotate(new Vector3(0, angle, 0));

        //get plane with kitecenterbridle, harness and harness + vector3.up
        //Vector3 planeNormal = Quaternion.Euler(0, 90, 0) * new Vector3((centerBridles - (this.transform.position + Vector3.up)).x, 0, (centerBridles - (this.transform.position + Vector3.up)).z);
        //Plane plane = new Plane(planeNormal, this.transform.position);

        Vector3 personHead = this.transform.position + 2*Vector3.up - (theWind.transform.forward.normalized * 0.6f);

        //plane going through bar center, perpendicular to the center lines
        Plane plane = new Plane((bar.transform.position - this.transform.position), bar.transform.position);

        Ray ray = new Ray(this.transform.position, personHead-this.transform.position);

        float distanceAlongRay;
        plane.Raycast(ray, out distanceAlongRay);
        
        
        if(Vector3.Dot(ray.GetPoint(distanceAlongRay), Vector3.up) < 0)
        {
            bar.transform.LookAt(-1 * ray.GetPoint(distanceAlongRay));
        } else
        {
            bar.transform.LookAt(ray.GetPoint(distanceAlongRay));
        }
                
        bar.transform.Rotate(new Vector3(0, 0, angle));
    }

    private void OnDrawGizmos()
    {
        //DrawArrow.ForGizmo(debugArrowStart, debugArrowDirection, Color.yellow);
    }

    private void handleInput()
    {
        float addAngle = 3f;
        float addPower = 0.03f;
        if (Input.GetKey("left"))
        {
            //rm angle
            if(Mathf.Abs(angle) <= maxAngle)
            {
                angle = Mathf.Max(angle - addAngle, -maxAngle);
            }
        }
        if (Input.GetKey("right"))
        {
            //add angle
            if (Mathf.Abs(angle) <= maxAngle)
            {
                angle = Mathf.Min(angle + addAngle, maxAngle);
            }
        }
        if (Input.GetKey("up"))
        {
            //depower
            if(power >= 0f)
            {
                power = Mathf.Max(power - addPower, 0f);
            }
        }
        if (Input.GetKey("down"))
        {
            //more power
            if(power < 1f)
            {
                power = Mathf.Min(power + addPower, 1f);
            }
        }

        if (Input.GetKey("escape"))
        {
            GameManager.Instance.UpdateGameState(GameState.Paused);
        }
    }
}
