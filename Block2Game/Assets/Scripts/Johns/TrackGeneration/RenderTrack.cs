using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTrack : MonoBehaviour
{
    [SerializeField ] TrackSaving _TrackLoad;
    [SerializeField] GameManagerBehaviour _Manager;
    SpawnBilboards billboardSpawner;
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
        // Add a Mesh Collider component to the object
        MeshCollider meshCollider = _Collider.AddComponent<MeshCollider>();
        // Create a new Mesh
        Mesh mesh = new Mesh();
        // Bake the Line Renderer's geometry into the Mesh
        lineRenderer.BakeMesh(mesh, true);
        // Assign the Mesh to the Mesh Collider
        meshCollider.sharedMesh = mesh;
        // Recalculate the normals and bounds of the Mesh
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        // Get the FinishLine script component
        _FinishLineSpawn = GetComponent<FinishLine>();
        // Find the SpawnBillboards script component
        billboardSpawner = FindObjectOfType<SpawnBilboards>();
        // Spawn billboards along the line renderer
        billboardSpawner.SpawnBoards(lineRenderer, _Manager._facts);
        // Add the finish line to the scene
        _FinishLineSpawn.AddFinishLine(trackRender[trackRender.Length - 1], trackRender[trackRender.Length - 2]);


    }


}
