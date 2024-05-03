using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexHull : MonoBehaviour
{
    //Script to make a convex hull based on the gift wrapping algorithm from Jarvis March
    [SerializeField]
    private int maxPointsGenerated = 25;


    private int pointCount;
    


    public Vector3[] points;
    [Range(0.05f, 1.5f)]
    public float size;
    public bool drawIt;
    //Using a hash to prevent duplicate points from entering 
    public HashSet<Vector3> results;
    public List<Vector3> outter;

    private void Update()
    {
        //H is used to instantiate the points
        if (Input.GetKeyDown(KeyCode.K))
            SpawnPoints();
        if (Input.GetKeyDown(KeyCode.J))
            SeekingTargets();
        if (Input.GetKeyDown(KeyCode.H))
            PointGenerator();




    }
    private void PointGenerator()
    {
        pointCount = Random.Range(15, maxPointsGenerated);
        points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            //125 Is subtracted to keep the square centered 
            float x = Random.Range(0.0f, 250f) - 125f;
            float y = 0;
            float z = Random.Range(0.0f, 250f) - 125f;
            points[i] = new Vector3(x,y, z);
        }
    }

    public void SeekingTargets()
    {
        results = new HashSet<Vector3>();
        
        //Finding the left most points and adding it to the result hash
        int leftMostIndex = 0;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[leftMostIndex].x > points[i].x)
                leftMostIndex = i;
        }
        results.Add(points[leftMostIndex]);

        //Making a list for collinear points 
        List<Vector3> collinearPoints = new List<Vector3>();
        Vector3 current = points[leftMostIndex];

        while (true)
        {
            //The point that is going around all the other points except for the current one 
            //The pair of current and nextTarget points make up the line to check if a points is on the left or not
            Vector3 nextTarget = points[0];
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i] == current)
                    continue;
                //Checks if the point is in points[i] is to the left or not
                float x1, x2, z1, z2;
                x1 = current.x - nextTarget.x;
                x2 = current.x - points[i].x;
                z1 = current.z - nextTarget.z;
                z2 = current.z - points[i].z;
                float val = (z2 * x1) - (z1 * x2);

                //If val > 0 it means that the points is on the left of the line 
                if (val > 0)
                {
                    nextTarget = points[i];
                    collinearPoints = new List<Vector3>();
                }
                //If val == 0 the points was lying along the line made by current and nextTarget
                //The point added to collinearPoints must be which ever point is closer to the current point which can be either the current point or the nextTarget point
                else if (val == 0)
                {
                    if (Vector3.Distance(current, nextTarget) < Vector3.Distance(current, points[i]))
                    {
                        collinearPoints.Add(nextTarget);
                        nextTarget = points[i];
                    }
                    else
                    {
                        collinearPoints.Add(points[i]);
                    }
                }
            }
            //We add all the collinear points to their own list because the line made by current and nextTarget might have multiple points on them
            //To get the points we want we only take the collinear points that are closest to the current point 
            foreach (Vector3 v in collinearPoints)
                results.Add(v);

            //When the entire set of points is checked we go back to the left most point that we started with to exit out of the loop
            if (nextTarget == points[leftMostIndex])
            {
                Debug.Log(results);
                break;
            }



            //if we haven't exited the loop we add NextTarget to list and we make that nextTarget the current so that checking for the left most point 
            //will start from the point that was recently added 
            results.Add(nextTarget);
            current = nextTarget;
            
            
        }
    }

    private void SpawnPoints()
    {
        if (results != null)
        {
            
            outter = new List<Vector3>();
            foreach (Vector3 value in results)
            {
                outter.Add(value);

            }

        }
    }
    private void OnDrawGizmos()
    {

        //We need a list to iterate over the values so we grab the values from the HashSet and putting them in the outter list
        if (drawIt)
        {
            if (results != null)
            {
                 outter = new List<Vector3>();

                foreach (var item in results)
                    outter.Add(item);

                for (int i = 0; i < outter.Count - 1; i++)
                {
                    Gizmos.DrawLine(outter[i], outter[i + 1]);
                }
                //Then we draw those outter points 
                Gizmos.DrawLine(outter[0], outter[outter.Count - 1]);
            }
            //Then we are drawing the spheres either cyan or yellow depending on whether they are in the convex or not
            for (int i = 0; i < points.Length; i++)
            {
                Gizmos.color = Color.cyan;

                if (results != null)
                {
                    if (results.Contains(points[i]))
                        Gizmos.color = Color.yellow;
                }

                Gizmos.DrawSphere(points[i], size);
            }
        }


    }


}
