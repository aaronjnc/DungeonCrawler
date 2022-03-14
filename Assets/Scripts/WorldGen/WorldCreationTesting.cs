using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreationTesting : MonoBehaviour
{
    ChunkGen chunkGen { get { return ChunkGen.currentWorld; } }
    [Tooltip("Width/Height of generation test")]
    [HideInInspector] public int size;
    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < size/2; x++)
        {
            for (int y = 0; y < size/2;y++)
            {
                chunkGen.GenerateChunk(new Vector2Int(x, y));
                chunkGen.GenerateChunk(new Vector2Int(-x-1, -y-1));
                chunkGen.GenerateChunk(new Vector2Int(x, -y-1));
                chunkGen.GenerateChunk(new Vector2Int(-x-1, y));
            }
        }
    }

}
