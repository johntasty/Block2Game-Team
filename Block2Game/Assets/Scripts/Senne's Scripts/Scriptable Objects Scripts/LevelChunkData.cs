using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelChunkData")]
public class LevelChunkData : ScriptableObject
{
    public enum Direction
    {
        North, East, South, West
    }
    //Holds the size along the x and z axis
    public Vector2 chunkSize = new Vector2(10f, 10f);

    //array of the prefabs 
    public GameObject[] levelChunks;
    //Makes sure the pieces are connected
    public Direction entryDirection;
    public Direction exitDirection; 
}
