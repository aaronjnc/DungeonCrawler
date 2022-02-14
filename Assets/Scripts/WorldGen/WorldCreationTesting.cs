using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreationTesting : MonoBehaviour
{
    GameManager manager;
    ChunkGen chunkGen { get { return ChunkGen.currentWorld; } }
    [HideInInspector]
    public int size;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        for(int x = 0; x < size/2; x++)
        {
            for (int y = 0; y < size/2;y++)
            {
                chunkGen.GenerateNewChunk(new Vector2Int(x, y));
                chunkGen.GenerateNewChunk(new Vector2Int(-x-1, -y-1));
                chunkGen.GenerateNewChunk(new Vector2Int(x, -y-1));
                chunkGen.GenerateNewChunk(new Vector2Int(-x-1, y));
            }
        }
    }

}
