using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harness : MonoBehaviour
{
    public GameObject leftBridle;
    public GameObject rightBridle;

    public LineRenderer lr_left;
    public LineRenderer lr_right;

    private void Update()
    {
        drawKiteLine(rightBridle.transform.position, lr_right);
        drawKiteLine(leftBridle.transform.position, lr_left);
    }


    private void drawKiteLine(Vector3 destination, LineRenderer lr)
    {
        Vector3[] linePositions = new Vector3[2];
        linePositions[0] = this.transform.position;
        linePositions[1] = destination;
        lr.SetPositions(linePositions);
    }
}
