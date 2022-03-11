using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkSave
{
    private int chunkX;
    private int chunkY;
    private string[] changed;
    public ChunkSave(Chunk chunk)
    {
        chunkX = chunk.chunkPos.x;
        chunkY = chunk.chunkPos.y;
        changed = chunk.GetChanges();
    }
    public Vector2Int GetChunkPos()
    {
        return new Vector2Int(chunkX, chunkY);
    }
    public string[] GetChanges()
    {
        return changed;
    }
}
