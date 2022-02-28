using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteEquations : MonoBehaviour
{
    Kite myKite;

    float kite_area;
    float kite_span;
    Vector3 kite_direction;

    float wind_velocity;
    Vector3 wind_direction;
    
    float air_density;

    
    public KiteEquations(Kite sessionKite)
    {
        myKite = sessionKite;
        //fixed during a single session:
        kite_area = (float)myKite.surfaceArea;
        kite_span = myKite.getSpan();
        air_density = (float)myKite.airDensity;


        kite_direction = myKite.transform.forward;
        wind_direction = myKite.wind.normalized;
        wind_velocity = myKite.wind.magnitude;
    }

    void FixedUpdate()
    {
        kite_direction = myKite.transform.forward;
    }

    float getLift()
    {
        float kite_AOA = getKiteAOA(wind_direction, kite_direction);
        float clo = getCLO(kite_AOA);
        float kite_ratio = getKiteAR(kite_span, kite_area);
        float lift_coef = getLiftCoef(clo, kite_ratio);
        float lift = lift_coef * kite_area * air_density * Mathf.Pow(wind_velocity, 2) * 0.5f;
        return lift;
    }

    private float getLiftCoef(float clo, float kite_ratio)
    {               
        return clo / ((1 + clo) / (Mathf.PI * kite_ratio));
    }

    /// <summary>
    /// get kite lift coefficient
    /// approximated with formula for flat thin plate at low angle of attack
    /// </summary>
    /// <param name="kite_AOA">kite angle of attack</param>
    /// <returns></returns>
    private float getCLO(float kite_AOA)
    {                
        return 2f * Mathf.PI * kite_AOA;
    }


    /// <summary>
    /// get kite aspect ratio
    /// kite aspect ratio defined as the squared span (length from side to side), divided by the area
    /// </summary>
    /// <param name="kite_span"></param>
    /// <param name="kite_area"></param>
    /// <returns>kite aspect ratio</returns>
    private float getKiteAR(float kite_span, float kite_area)
    {                
        return Mathf.Pow(kite_span, 2) / kite_area;
    }

    /// <summary>
    /// get kite angle of attack (relative to wind)
    /// </summary>
    /// <returns>angle of attack relative to wind, in RADIANS</returns>
    private float getKiteAOA(Vector3 wind_direction, Vector3 kite_direction)
    {
        return Mathf.Deg2Rad * Vector3.Angle(wind_direction, kite_direction);
    }
}
