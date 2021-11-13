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
        for(int x = 0; x < size; x++)
        {
            for (int y = 0; y < size;y++)
            {
                if (x!=0 || y!=0)
                {
                    chunkGen.GenerateNewChunk(new Vector2Int(x, y));
                }
            }
        }
    }

}
