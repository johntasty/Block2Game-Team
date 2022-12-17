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
            PushApart(dataSet);
        }

        if (Input.GetKey(KeyCode.M))
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
            disp = disp * (Random.Range(0.0f, 1.0f) * 360);

            //Multiplies the displacement 
            disp = disp * dispLen;

            //Creates a new point after every existing point with a value of zero
            rSet[i] = dataSet[i];
            //Creates a new point before every existing point 
            rSet[i * 2 + 1] = new Vector3(dataSet[i].x, dataSet[i].y, dataSet[i].z);

            rSet[i * 2 + 1] = rSet[i * 2 + 1] + dataSet[(i+1) % dataSet.Length] / 2 + disp;
          
        }

        dataSet = rSet;

        
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
              var  p_dst = Mathf.Sqrt(Mathf.Pow(dataSet[i][0] - dataSet[j][0], 2) + Mathf.Pow(dataSet[i][1] - dataSet[j][1], 2));
              if (p_dst < dst)
                {
                    //Finds the distance between the y and x of 2 points
                    float dx = dataSet[j][0] - dataSet[i][0];
                    float dy = dataSet[j][1] - dataSet[i][1];

                    //Calculation of arc length based on the Pythagorean Theorem
                    float dl = Mathf.Sqrt(dx *dx + dy * dy);

                    //not sure what is happening here
                    dx /= dl;
                    dy /= dl;

                    float dif = dst - dl;

                    dx *= dif;
                    dy *= dif;
                    //Pushing the points apart?
                    dataSet[j][0] = System.Convert.ToInt32(dataSet[j][0] + dx);
                    dataSet[j][1] = System.Convert.ToInt32(dataSet[j][1] + dy);
                    dataSet[i][0] = System.Convert.ToInt32(dataSet[i][0] - dx);
                    dataSet[i][1] = System.Convert.ToInt32(dataSet[i][1] - dy);
                }
              

            }
        }
        return;
    }

    private void OnDrawGizmos()
    {

        //We need a list to iterate over the values so we grab the values from the HashSet and putting them in the outter list
        if (drawIt)
        {
           
            for (int i = 0; i < dataSet.Length; i++)
            {
                Gizmos.color = Color.green;

                Gizmos.DrawSphere(dataSet[i], 1.5f);
            }
        }


    }
}
