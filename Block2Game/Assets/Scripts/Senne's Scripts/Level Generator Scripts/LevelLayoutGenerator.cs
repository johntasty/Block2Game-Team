using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
public class LevelLayoutGenerator : MonoBehaviour
{
    [SerializeField]
    private LevelChunkData[] levelChunkData;
    [SerializeField]
    private LevelChunkData firstChunk;
    //To know what the exit of the previous chunk is to connect the new one
    private LevelChunkData previousChunk;

    //Where to spawn the new pieces
    [SerializeField]
    private Vector3 spawnOrigin;
    
    //Where the new pieces are actually spawned with offset from the previous spawned chunk
    private Vector3 spawnPosition;

    //Initial amount of chunks to spawn
    [SerializeField]
    [Tooltip("Initial amount of chunks to spawn")]
    private int chunksToSpawn = 10;

    //When the player leaves a chunk it triggers to spawn a new chunk
    private void OnEnable()
    {
        TriggerExit.OnChunkExited += PickAndSpawnChunk;
    }
    private void OnDisable()
    {
        TriggerExit.OnChunkExited -= PickAndSpawnChunk;
    }

    //Spawn new chunks when T is pressed 
    //Used for debugging 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PickAndSpawnChunk();
        }
    }

    
    private void Start()
    {
        //To make sure we always have the same first chunk
        previousChunk = firstChunk;

        //For loop for the initial chunks to spawn
        for (int i = 0; i < chunksToSpawn; i++)
        {
            PickAndSpawnChunk();
        }
    }

    LevelChunkData PickNextChunk()
    {
        //Create a list of allowed chunks and when spawning a random one is picked
        List<LevelChunkData> allowedChunkList = new List<LevelChunkData>();
        //Starts at null but gets filled with the code below
        LevelChunkData nextChunk = null;

        //Declare a direction which is later overwritten by the code below
        LevelChunkData.Direction nextRequiredDirection = LevelChunkData.Direction.North;

        //Makes the list of desired chunks based on the exit direction of the previous chunk
        switch (previousChunk.exitDirection)
        {
            case LevelChunkData.Direction.North:
                nextRequiredDirection = LevelChunkData.Direction.South;
                //Creates the offset for the new chunk so it won't overlap
                spawnPosition = spawnPosition + new Vector3(0f, 0, previousChunk.chunkSize.y);
                break;

            case LevelChunkData.Direction.East:
                nextRequiredDirection = LevelChunkData.Direction.West;
                spawnPosition = spawnPosition + new Vector3(previousChunk.chunkSize.x, 0, 0);
                break;

            case LevelChunkData.Direction.South:
                nextRequiredDirection = LevelChunkData.Direction.North;
                spawnPosition = spawnPosition + new Vector3(0f, 0, -previousChunk.chunkSize.y);
                break;

            case LevelChunkData.Direction.West:
                nextRequiredDirection = LevelChunkData.Direction.East;
                spawnPosition = spawnPosition + new Vector3(-previousChunk.chunkSize.x, 0, 0);

                //Get the bounds of the mesh for chunksize
                //mesh.render bounds
                //Create a class for vector2 to keep track of sizes in a list

                //caching 

                break;
                default:
                break;
        }

        for (int i=0; i < levelChunkData.Length; i++)
        {
            //Goes over the list and looks for viable chunks
            if(levelChunkData[i].entryDirection == nextRequiredDirection)
            {
                //adds the viable chunks to the allowed chunk list 
                allowedChunkList.Add(levelChunkData[i]);
            }
        }

        //Picks a random chunk for the allowed chunk list
        nextChunk = allowedChunkList[Random.Range(0, allowedChunkList.Count)];
        //Returns the randomly picked chunk from the allowed chunk list
        return nextChunk;

        //Create a weight list 
        //Decrease weight based on spawn amount 
        //increase the rest 
    }

    private void PickAndSpawnChunk()
    {
        //Returns a desirable chunk that can connect from the PickNextChunk function
        LevelChunkData chunkToSpawn = PickNextChunk();
        //Declares a new gameobject that is a random chunk which can connect to the previous chunk
        GameObject objectFromChunk = chunkToSpawn.levelChunks[Random.Range(0, chunkToSpawn.levelChunks.Length)];
        //Updates the previous chunk to the new spawned chunk
        previousChunk = chunkToSpawn;
        //Spawns it into the game with an offset of the origin which keeps it relative to the spawn origin instead of a absolute world space position
        //after that we use the rotation stored in the prefab
        Instantiate(objectFromChunk, spawnPosition + spawnOrigin, Quaternion.identity);
    }

    //Updates the origin for where the track pieces need to be spawned
    public void UpdateSpawnOrigin(Vector3 originDelta)
    {
        spawnOrigin = spawnOrigin + originDelta;
    }
}
