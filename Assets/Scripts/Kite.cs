using UnityEngine;

public class Kite : MonoBehaviour
{
    [SerializeField] public double surfaceArea { get; private set; }
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

        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(kite_width, 0.01f, kite_height));
    }

    private void Start()
    {
        InitKite();
    }

    void InitKite()
    {
        //set left and right point positions relative to kite position, kite width and harness position
        Vector3 harness_direction = lineOrigin - this.transform.position;
        //kite_width_direction = Vector3.Cross(kite_direction, harness_direction);        
    }

    void FixedUpdate()
    {
    }

    //get width of kite
    public float getSpan()
    {
        return Vector3.Distance(leftPoint.transform.position, rightPoint.transform.position);
    }
}