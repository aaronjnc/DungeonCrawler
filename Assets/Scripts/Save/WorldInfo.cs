using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldInfo
{
    [Tooltip("Seed used for world generation")]
    private int worldSeed;
    [Tooltip("Seed used for biome generation")]
    private int biomeSeed;
    [Tooltip("Play time in hours")]
    private double playTime;
    /// <summary>
    /// Creates new world info object
    /// </summary>
    public WorldInfo()
    {
        worldSeed = ChunkGen.Instance.seed;
        biomeSeed = ChunkGen.Instance.biomeseed;
        TimeSpan playTime = System.DateTime.Now.Subtract(GameManager.Instance.startTime);
        this.playTime = GameManager.Instance.hours + playTime.TotalHours;
    }
    /// <summary>
    /// Returns world seed
    /// </summary>
    /// <returns></returns>
    public int GetWorldSeed()
    {
        return worldSeed;
    }
    /// <summary>
    /// Returns biome seed
    /// </summary>
    /// <returns></returns>
    public int GetBiomeSeed()
    {
        return biomeSeed;
    }
    /// <summary>
    /// Returns play time
    /// </summary>
    /// <returns></returns>
    public double GetPlayTime()
    {
        return playTime;
    }
}
