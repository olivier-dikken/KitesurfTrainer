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

    private void Update()
    {
        drawKiteLine(barRightTip.transform.position, rightBridle.transform.position, lr_right);
        drawKiteLine(barLeftTip.transform.position, leftBridle.transform.position, lr_left);
        drawKiteLine(this.transform.position, centerLeftBridle.transform.position, lr_center_left);
        drawKiteLine(this.transform.position, centerRightBridle.transform.position, lr_center_right);

        setBarPosition(0.1f, 0f);
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

        Vector3 newBarPositionOnLine = (1 - power) * barLine;
        bar.transform.position = this.transform.position + newBarPositionOnLine;

        //bar.transform.LookAt(centerBridles);
        //bar.transform.Rotate(new Vector3(0, angle, 0));

        //get plane with kitecenterbridle, harness and harness + vector3.up
        //Vector3 planeNormal = Quaternion.Euler(0, 90, 0) * new Vector3((centerBridles - (this.transform.position + Vector3.up)).x, 0, (centerBridles - (this.transform.position + Vector3.up)).z);
        //Plane plane = new Plane(planeNormal, this.transform.position);

        Vector3 personHead = Vector3.up - (theWind.transform.forward.normalized * 1);

        Plane plane = new Plane((bar.transform.position - this.transform.position), bar.transform.position);
        Ray ray = new Ray(this.transform.position, personHead);
        float distanceAlongRay;
        plane.Raycast(ray, out distanceAlongRay);

        bar.transform.LookAt(this.transform.position + personHead * distanceAlongRay);
        bar.transform.Rotate(new Vector3(angle, 0, 0));
    }
}
