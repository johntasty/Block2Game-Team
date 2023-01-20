using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTrack : MonoBehaviour
{
    [SerializeField ] TrackSaving _TrackLoad;
    FinishLine _FinishLineSpawn;
    // The LineRenderer component
    private LineRenderer lineRenderer;
    [SerializeField] GameObject _Collider;
    // Start is called before the first frame update
    void Start()
    {
        
        Vector3[] trackRender = _TrackLoad.LoadTrack();

        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
       
        // Set the number of points in the LineRenderer
        lineRenderer.positionCount = trackRender.Length;
        // Set the points in the LineRenderer
        lineRenderer.SetPositions(trackRender);
        lineRenderer.alignment = LineAlignment.TransformZ;
        MeshCollider meshCollider = _Collider.AddComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
        //meshCollider.convex = true;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        _FinishLineSpawn = GetComponent<FinishLine>();

        _FinishLineSpawn.AddFinishLine(trackRender[trackRender.Length - 1], trackRender[trackRender.Length - 2]);

    }

   
}
