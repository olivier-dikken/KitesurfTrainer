using UnityEngine;

public class Kite : MonoBehaviour
{

    [SerializeField] public KiteEquations myKiteEq;
    [SerializeField] public double surfaceArea;
    [SerializeField] public double airDensity;
    [SerializeField] private double windVelocity;
    [SerializeField] public Vector3 wind;

    [SerializeField] private double tensionLeft;
    [SerializeField] private double tensionRight;

    [SerializeField] private Vector3 lineOrigin = Vector3.zero;

    [SerializeField] float kite_width;
    [SerializeField] float kite_height;

    public GameObject leftPoint;
    public GameObject rightPoint;

    Vector3 kite_width_direction;

    private void OnDrawGizmos()
    {
        // draw line points
        // lines
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(leftPoint.transform.position, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rightPoint.transform.position, 0.1f);

        // origin point
        Gizmos.DrawSphere(lineOrigin, 0.1f);

        // lines
        Gizmos.color = Color.green;
        Gizmos.DrawLine(lineOrigin, leftPoint.transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(lineOrigin, rightPoint.transform.position);

        DrawArrow.ForGizmo(this.transform.position, this.transform.forward, Color.cyan);
        DrawArrow.ForGizmo(this.transform.position, kite_width_direction, Color.magenta);

        //draw wind direction/power
        DrawArrow.ForGizmo(lineOrigin, wind, Color.white);

        //draw lift
        DrawArrow.ForGizmo(this.transform.position, new Vector3(0,myKiteEq.lift/10,0), Color.black);

        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(kite_width, 0.01f, kite_height));
                    
    }

    private void Start()
    {
        InitKite();
        myKiteEq = new KiteEquations(this);
    }

    void InitKite()
    {
        //set left and right point positions relative to kite position, kite width and harness position
        Vector3 harness_direction = lineOrigin - this.transform.position;
        kite_width_direction = Vector3.Cross(this.transform.forward, harness_direction);        
    }

    void FixedUpdate()
    {

        myKiteEq.CustomUpdate();
        
    }

    //get width of kite
    public float getSpan()
    {
        return kite_width;
    }
}