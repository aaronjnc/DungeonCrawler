using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkSave
{
    [Tooltip("Chunk x position")]
    private int chunkX;
    [Tooltip("Chunk y position")]
    private int chunkY;
    [Tooltip("String array of changed blocks")]
    private string[] changed;
    /// <summary>
    /// Saves chunk data
    /// </summary>
    /// <param name="chunk"></param>
    public ChunkSave(Chunk chunk)
    {
        chunkX = chunk.chunkPos.x;
        chunkY = chunk.chunkPos.y;
        changed = chunk.GetChanges();
    }
    /// <summary>
    /// Returns position of chunk
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetChunkPos()
    {
        return new Vector2Int(chunkX, chunkY);
    }
    /// <summary>
    /// Return string array of changed block
    /// </summary>
    /// <returns></returns>
    public string[] GetChanges()
    {
        return changed;
    }
}
