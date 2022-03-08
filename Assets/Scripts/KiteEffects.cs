using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteEffects : MonoBehaviour
{
    //needed variables
    public Wind theWind;



    float CoandaEffect(float coandaCoef, Vector3 windVector, Transform kiteTranform, Vector3 kiteVector)
    {
        Vector3 apparentWind = windVector - kiteVector;
        Vector3 locationOnPlane = Vector3.ProjectOnPlane(apparentWind, -kiteTranform.up);
        //Gizmos.DrawSphere(locationOnPlane, 0.2f);

        float AOA = Vector3.Angle(apparentWind, locationOnPlane);
        Debug.Log("Coanda Effect debug. AOA : " + AOA.ToString());

        return AOA;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CoandaEffect(1, -Vector3.forward, this.transform, this.transform.forward);
    }

    private void OnDrawGizmos()
    {
        float CE = CoandaEffect(1, theWind.getWindVector(), this.transform, Vector3.zero);
       
    }
}
