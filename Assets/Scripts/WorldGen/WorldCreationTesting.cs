using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreationTesting : MonoBehaviour
{
    [Tooltip("Width/Height of generation test")]
    [HideInInspector] public int size;
    // Start is called before the first frame update
    void OnEnable()
    {
        for(int x = 0; x < size/2; x++)
        {
            for (int y = 0; y < size/2;y++)
            {
                ChunkGen.Instance.GenerateChunk(new Vector2Int(x, y));
                ChunkGen.Instance.GenerateChunk(new Vector2Int(-x-1, -y-1));
                ChunkGen.Instance.GenerateChunk(new Vector2Int(x, -y-1));
                ChunkGen.Instance.GenerateChunk(new Vector2Int(-x-1, y));
            }
        }
    }

}
