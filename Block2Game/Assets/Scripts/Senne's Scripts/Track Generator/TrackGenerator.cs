using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    ConvexHull cv;
    private GameObject pointObject;

    private int pointCount;
    [SerializeField]
    private int initialPointRange = 20;
    [SerializeField]
    private Vector3[] dataSet;
    public Vector3[] rSet;

    [SerializeField]
    private float difficulty;
    [SerializeField]
    private float pushIterations = 3;

    private Vector3 vector3 = Vector3.zero;

    public bool drawIt;

    private void Start()
    {
        cv = GetComponent<ConvexHull>();
        pointObject = new GameObject();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            dataSet = cv.outter.ToArray();
            //PushApart(dataSet);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            TrackDisplacement();
        }
    }
    
    private void TrackDisplacement()
    {
        rSet = new Vector3[dataSet.Length *2];
        Vector3 disp = new Vector3();
        
        float maxDisp = 20f;
        float disRot = Random.Range(0, 1) * 360;

        for (int i = 0; i < dataSet.Length; i++)
        {
            //Define a random amount of displacement 
            float dispLen = Mathf.Pow(Random.Range(0.0f, 1.0f), difficulty) * maxDisp;
            //Sets the displacement in the vector
            disp.Set(0, 0, 1);

            //Rotation
            //disp = disp * (Random.Range(0.0f, 1.0f) * 360);

            //Multiplies the displacement 
            disp = disp * dispLen;

            //Creates a new point before every existing point with a value of zero
            rSet[i * 2] = dataSet[i];
            //Creates a new point after every existing point 
            rSet[i * 2 + 1] = new Vector3(dataSet[i].x, dataSet[i].y, dataSet[i].z);

            rSet[i * 2 + 1] = rSet[i * 2 + 1] + (dataSet[(i+1) % dataSet.Length]) / 2 + disp;
            
            
        }
       


        dataSet = rSet;
        //Push apart
        for (int i = 0; i < pushIterations; i++)
            //FixAngles();
            PushApart(dataSet);


    }

    //Need to push points that are too close to eachother apart to prevent loops
    //Don't forget to turn the outter list from the convex hull script into the dataset array before doing this with ToArray
    private void PushApart(Vector3[] dataSet)
    {
        

        float dst = 20;
        float dst2 = dst * dst;

        //Gets the first point 
        for (int i = 0; i < dataSet.Length; i++)
        {
            //Gets the second point to calculate the midpoint 
            //which is why it starts at  i + 1
            for (int j = i +1; j < dataSet.Length; j++)
            {
              var  p_dst = Mathf.Sqrt(Mathf.Pow(dataSet[i].x - dataSet[j].x, 2) + Mathf.Pow(dataSet[i].z - dataSet[j].z, 2));
              if (p_dst < dst)
                {
                    //Finds the distance between the y and x of 2 points
                    float dx = dataSet[j].x - dataSet[i].x;
                    float dz = dataSet[j].z - dataSet[i].z;

                    //Calculation of arc length based on the Pythagorean Theorem
                    float dl = Mathf.Sqrt(dx *dx + dz * dz);

                    //not sure what is happening here
                    dx /= dl;
                    dz /= dl;

                    float dif = dst - dl;

                    dx *= dif;
                    dz *= dif;
                    //Pushing the points apart
                    dataSet[j].x += dx;
                    dataSet[j].z += dz;
                    dataSet[i].x -= dx;
                    dataSet[i].z -= dz;
                }
              
            }
        }
    }

    private void FixAngles()
    {
        for (int i = 0; i < dataSet.Length; i++)
        {
            int previous = (i - 1) < 0 ? dataSet.Length -1 : i -1;
            int next = (i + 1) % dataSet.Length;

            float px = dataSet[i].x - dataSet[previous].x;
            float pz = dataSet[i].z - dataSet[previous].z;
            float pl = Mathf.Sqrt(px*px + pz*pz);
            px /= pl;
            pz /= pl;

            float nx = dataSet[i].x - dataSet[next].x;
            float nz = dataSet[i].z - dataSet[next].z;
            nx = -nx;
            nz = -nz;
            float nl = Mathf.Sqrt(nx*nx - nz*nz);
            nx /= nl;
            nz /= nl;

            //Perp dot between the previous and next point
            float a = Mathf.Atan2(px * nz - pz * nz, px * nx + pz * nz);

            if (Mathf.Abs(a * Mathf.Rad2Deg) <= 100)
                continue;

            float nA = 100 * Mathf.Sign(a) * Mathf.Deg2Rad;
            float diff = nA - a;
            float cos = Mathf.Cos(diff);
            float sin = Mathf.Sin(diff);
            float newX = nx * cos - nz * sin;
            float newZ = nz * sin + nz * cos;
            newX *= nl;
            newZ *= nl;

            dataSet[next].x = dataSet[i].x + newX;
            dataSet[next].z = dataSet[i].z + newZ;
        }
    }

    private void OnDrawGizmos()
    {

        //We need a list to iterate over the values so we grab the values from the HashSet and putting them in the outter list
        if (drawIt)
        {
            for (int i = 0; i < dataSet.Length - 1; i++)
            {
                Gizmos.DrawLine(dataSet[i], dataSet[i + 1]);
            }
            //Then we draw those outter points 
            Gizmos.DrawLine(dataSet[0], dataSet[dataSet.Length - 1]);

            for (int i = 0; i < dataSet.Length; i++)
            {
                Gizmos.color = Color.green;

                Gizmos.DrawSphere(dataSet[i], 5f);
            }
        }


    }
}
