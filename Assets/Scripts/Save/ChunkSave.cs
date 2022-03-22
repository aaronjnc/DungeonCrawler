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
    private EnemySave[] enemyInfo;
    
    /// <summary>
    /// Saves chunk data
    /// </summary>
    /// <param name="chunk"></param>
    public ChunkSave(Chunk chunk)
    {
        chunkX = chunk.chunkPos.x;
        chunkY = chunk.chunkPos.y;
        changed = chunk.GetChanges();
        int i = 0;
        enemyInfo = new EnemySave[chunk.GetEnemies().Count];
        foreach (GameObject enemy in chunk.GetEnemies())
        {
            enemyInfo[i] = new EnemySave(enemy);
            i++;
        }
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
    public EnemySave[] GetEnemies()
    {
        return enemyInfo;
    }
}
