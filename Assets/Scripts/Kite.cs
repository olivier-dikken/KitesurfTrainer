using UnityEngine;

public class Kite : MonoBehaviour
{
    [SerializeField] private double surfaceArea;
    [SerializeField] private double airDensity;
    [SerializeField] private double windVelocity;

    [SerializeField] private double tensionLeft;
    [SerializeField] private double tensionRight;

    [SerializeField] private Vector3 lineOrigin = Vector3.zero;

    public GameObject leftPoint;
    public GameObject rightPoint;

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
    }

    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(0, 10, 0);
    }
}