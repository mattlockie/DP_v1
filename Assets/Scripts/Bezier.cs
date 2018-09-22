using UnityEngine;

public class Bezier : MonoBehaviour {

    public LineRenderer lineRenderer;

    public readonly int numPoints = 50;
    public readonly Vector3[] positions = new Vector3[50];

    //public Transform point0;
    //public Transform point1;
    //public Transform point2;

    public static Bezier Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //lineRenderer.SetVertexCount(numPoints);
    }

    public void DrawQuadtraticCurve(Transform pointA, Transform pointB, Transform pointC)
    {
        lineRenderer.SetVertexCount(numPoints);
        // think we can maybe increment time in the update to draw over time rather than all at once here
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, pointA.position, pointB.position, pointC.position);
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return p0 + t * (p1 - p0);
    }

    void DrawLinearCurve(Transform pointA, Transform pointB)
    {
        lineRenderer.SetVertexCount(numPoints);
        // think we can maybe increment time in the update to draw over time rather than all at once here
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateLinearBezierPoint(t, pointA.position, pointB.position);
        }
        lineRenderer.SetPositions(positions);
    }
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    //void DrawQuadtraticCurve()
    //{
    //    // think we can maybe increment time in the update to draw over time rather than all at once here
    //    for (int i = 1; i < numPoints + 1; i++)
    //    {
    //        float t = i / (float)numPoints;
    //        positions[i - 1] = CalculateQuadraticBezierPoint(t, point0.position, point1.position, point2.position);
    //    }
    //    lineRenderer.SetPositions(positions);
    //}
    //void DrawLinearCurve()
    //{
    //    // think we can maybe increment time in the update to draw over time rather than all at once here
    //    for (int i = 1; i < numPoints + 1; i++)
    //    {
    //        float t = i / (float)numPoints;
    //        positions[i - 1] = CalculateLinearBezierPoint(t, point0.position, point1.position);
    //    }
    //    lineRenderer.SetPositions(positions);
    //}
}