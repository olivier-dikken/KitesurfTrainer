using System;
using UnityEngine;

public class Kite : MonoBehaviour
{

    public Vector3 wind;

    public Line leftLine;
    public Line rightLine;
    
    // parameters
    [SerializeField] private float dragScale = 1;
    [SerializeField] private float liftScale = 1;
    [SerializeField] private float dragTorqueScale = 1;
    [SerializeField] private float liftTorqueScale = 1;

    // recomputed at each iteration
    public Vector3 totalWindForce;
    private Vector3 _liftForce;
    private Vector3 _dragForce;
    private Vector3 _liftTorque;
    private Vector3 _dragTorque;
    private float _liftMagnitude;
    private float _dragMagnitude;
    private float _angleOfAttack;
    
    // components
    private Rigidbody _rb;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }


    private void UpdatePhysics()
    {
        // angle of attack is angle between wind and kite direction
        _angleOfAttack = Mathf.Deg2Rad * Vector3.Angle(wind, transform.up);
        _angleOfAttack = _angleOfAttack == 0 ? 0.001f : _angleOfAttack;
        
        // compute lift and drag magnitudes
        _liftMagnitude = (wind.magnitude * wind.magnitude / _angleOfAttack) * liftScale;
        _liftMagnitude = _liftMagnitude < 0 ? 0 : _liftMagnitude;
        
        _dragMagnitude = Mathf.Sin(_angleOfAttack) * dragScale;

        // get vector forms of drag and lift
        _dragForce = wind.normalized * _dragMagnitude;
        _liftForce = transform.up * _liftMagnitude;

        float liftRelAngle = Mathf.Deg2Rad * Vector3.Angle(transform.up, Vector3.up);
        float dragRelAngle = Mathf.Deg2Rad * Vector3.Angle(transform.up, wind.normalized);
        Debug.Log(dragRelAngle);
        
        _liftTorque = -Vector3.Cross(Vector3.up, transform.up) * liftRelAngle * liftTorqueScale * _liftMagnitude;
        _dragTorque = -Vector3.Cross(wind, transform.up).normalized * dragRelAngle * Vector3.Angle(transform.up, wind) * dragTorqueScale * _dragMagnitude;
            
        // total wind force, sum of lift and drag
        totalWindForce = _dragForce + _liftForce;
    }

    private void OnDrawGizmos()
    {
        
        // wind
        DrawArrow.ForGizmo(transform.position, wind, Color.blue, 2.0f);
        
        // direction
        // DrawArrow.ForGizmo(transform.position, transform.up, Color.red);
        
        // draw forces
        DrawArrow.ForGizmo(transform.position, totalWindForce, Color.cyan);
        // DrawArrow.ForGizmo(transform.position, _dragForce, Color.magenta);
       
        // draw torque-axis
        // DrawArrow.ForGizmo(transform.position, _liftTorque, Color.cyan);
        // DrawArrow.ForGizmo(transform.position, _dragTorque, Color.green);

    }

    public void FixedUpdate()
    {
        // recompute physics
        UpdatePhysics();

        // apply forces
        _rb.AddForce(totalWindForce);
        _rb.AddForceAtPosition(leftLine.GetTensionForce(), leftLine.transform.position);
        _rb.AddForceAtPosition(rightLine.GetTensionForce(), rightLine.transform.position);

        // apply torques
        _rb.AddTorque(_dragTorque);
        _rb.AddTorque(_liftTorque);
    }
}