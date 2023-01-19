using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGeneratorConvex : MonoBehaviour
{// The width of the racetrack
    [SerializeField]private float trackWidth = 10f;

    // The length of the racetrack
    [SerializeField] private float trackLength = 100f;

    [SerializeField] private float displaced = 10f;
    public float _Displaced
    {
        get => displaced;

        set => displaced = value;
    }
    [SerializeField] private float strenght = 10f;

    [SerializeField] private float _difficulty = 5;
    public float Difficulty
    {
        get => _difficulty;

        set => _difficulty = value;
    }
    [SerializeField] private float  threshold = 15f;
    public float _Threshold
    {
        get => threshold;

        set => threshold = value;
    }

    // The number of points to use for the racetrack
    [SerializeField] private int numPoints = 50;

    // The LineRenderer component
    private LineRenderer lineRenderer;
    List<Vector3> points = new List<Vector3>();
    List<Vector3> _points = new List<Vector3>();

    public List<Vector3> _smooth = new List<Vector3>();
    public List<Vector3> _Smooth
    {
        get => _smooth;        
    }
    public void MakeTrack()
    {       
        // Generate the racetrack
        GenerateTrack();
    }
   
    void GenerateTrack()
    {
        // Generate a smooth curve for the racetrack
        points = GeneratePoints(numPoints, trackWidth, trackLength);
        // Set the first and last points to be the same, to close the loop
        _points = GenerateConvexHull(points,_difficulty, displaced);

        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();

        // Set the number of points in the LineRenderer
        lineRenderer.positionCount = _points.Count;
        // Set the points in the LineRenderer
        lineRenderer.SetPositions(_points.ToArray());
        lineRenderer.Simplify(1f);

        //reset positions in the LineRenderer to smooth out the end points
        Vector3[] pos = new Vector3[lineRenderer.positionCount];
        
        lineRenderer.GetPositions(pos);

        _smooth = new List<Vector3>(pos);
        //IncreasePoints(_smooth, 3f);
        _smooth = SmoothPoints(_smooth, 100);

        lineRenderer.positionCount = _smooth.Count;
        // Set the points in the LineRenderer
        lineRenderer.SetPositions(_smooth.ToArray());
        lineRenderer.Simplify(1f);
    }

    List<Vector3> GeneratePoints(int numPoints, float width, float length)
    {
        // Create an array of points for the curve
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < numPoints; i++)
        {
            // Generate a random x and y position within the given width and length
            float x = Random.Range(-width / 2f, width / 2f);
            float y = Random.Range(-length / 2f, length / 2f);

            // Add the point to the list
            points.Add(new Vector3(x, 0f, y));
        }

        // Return the list of points


        return points;
    }
   
    public List<Vector3> GenerateConvexHull(List<Vector3> points, float difficulty, float displaced)
    {
        // The list of points on the convex hull
        List<Vector3> hull = new List<Vector3>();
      
        // Find the leftmost point in the set
        Vector3 leftmost = points[0];
        foreach (Vector3 point in points)
        {
            if (point.x < leftmost.x)
            {
                leftmost = point;
            }
        }

        // Start the hull with the leftmost point
        Vector3 currentPoint = leftmost;
        Vector3 nextPoint;
        do
        {
            // Add the current point to the hull
            hull.Add(currentPoint);
            
            // Find the next point on the hull
            nextPoint = points[0];
            foreach (Vector3 point in points)
            {
                if (point == currentPoint)
                {
                    continue;
                }
                if (nextPoint == currentPoint || IsCounterClockwise(currentPoint, nextPoint, point))
                {
                    nextPoint = point;
                }
            }
            Vector3 midwayPoint = (currentPoint + nextPoint) / 2;
           
            // Calculate the angle to rotate the vector based on the difficulty factor
            float angle = difficulty;
            midwayPoint += (midwayPoint - currentPoint).normalized * displaced; 
            // Calculate the new point by rotating the vector by the angle
            Vector3 newPoint = Quaternion.Euler(0,angle,0) * (midwayPoint - currentPoint) + currentPoint; 
           
            hull.Add(newPoint);
            
            // Set the current point to the next point
            currentPoint = nextPoint;
        } while (currentPoint != leftmost);
        hull.Add(hull[0]);

        PushPoints(hull, threshold);

        List<Vector3> smoothPoints = SmoothPoints(hull, 100);

        // Return the convex hull
        return smoothPoints;
    }
    public static Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Calculate the point on the Bezier curve
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        // Return the point
        return p;
    }

    public static List<Vector3> SmoothPoints(List<Vector3> points, int numPoints)
    {
        // The list of smooth points
        List<Vector3> smoothPoints = new List<Vector3>();

        // The step size between each point on the Bezier curve
       
        
        // Iterate through the points
        for (int i = 0; i < points.Count - 1; i++)
        {
            
            for (int j = 1; j <= numPoints; j++)
            {
              
                float t = i / (float)numPoints;               
                smoothPoints.Add(CalculateBezierPoint(points[i], points[(i + 1)% points.Count], points[(i + 2) % points.Count], points[(i + 3) % points.Count], t));
            }

        }

        // Return the smooth points
        return smoothPoints;
    }
    // Check if three points form a counterclockwise turn
    static bool IsCounterClockwise(Vector3 a, Vector3 b, Vector3 c)
    {
        // Calculate the cross product of the vectors AB and AC
        float crossProduct = (b.x - a.x) * (c.z - a.z) - (b.z - a.z) * (c.x - a.x);

        // Check if the cross product is positive
        return crossProduct > 0f;
    }

    static void PushPoints(List<Vector3> points, float threshold)
    {
        for(int j = 0; j < 10; j++)
        {
            
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 nextPoint = points[(i + 1) % points.Count];
                
                float distance = Vector3.Distance(points[i], nextPoint);
                

                // Check if the distance is below the threshold
                if (distance < threshold)
                {
                    
                    // Calculate the vector between the current point and the next point
                    Vector3 vector = nextPoint - points[i];

                    // Normalize the vector
                    vector = vector.normalized;

                    // Calculate the push amount based on the threshold
                    float pushAmount = (threshold - distance);
                   
                    // Push the current point and the next point apart by the push amount
                    points[i] -= vector * pushAmount;
                    points[(i + 1) % points.Count] += vector * pushAmount;
                }
            }
        }      
    }
    public static void IncreasePoints(List<Vector3> points, float distance)
    {
        for(int i = 0; i < points.Count - 1; i++)
        {
            Vector3 nextPoint = points[(i + 1) % points.Count];
            // Calculate the vector between the current point and the next point
            Vector3 vector = nextPoint - points[i];

            // Normalize the vector
            vector = vector.normalized;

            Vector3 newPoint = points[i] + (vector * distance);
            points.Insert((i + 1) % points.Count, newPoint);

        }
    }
}
