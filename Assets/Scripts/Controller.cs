using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Kite kite;

    public GameObject leftPoint;
    public GameObject rightPoint;

    void FixedUpdate()
    {
        // place handles at bar 
        kite.leftLine.lineOrigin = leftPoint.transform.position;
        kite.rightLine.lineOrigin = rightPoint.transform.position;

        Vector3 leftLineDir = kite.leftLine.transform.position - kite.leftLine.lineOrigin;
        Vector3 rightLineDir = kite.rightLine.transform.position - kite.rightLine.lineOrigin;

        float rot = Input.GetAxis("Horizontal");
        transform.RotateAround(transform.position, Vector3.Cross(leftLineDir, rightLineDir), rot);

        float tension = Input.GetAxis("Vertical");
        Debug.Log(tension);
        // kite.leftLine.tensionScale += tension;
        // kite.rightLine.tensionScale += tension;
    }
}
