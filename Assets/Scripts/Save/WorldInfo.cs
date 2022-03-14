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
    /// <param name="gen"></param>
    /// <param name="manager"></param>
    public WorldInfo(ChunkGen gen, GameManager manager)
    {
        worldSeed = gen.seed;
        biomeSeed = gen.biomeseed;
        TimeSpan playTime = System.DateTime.Now.Subtract(manager.startTime);
        this.playTime = manager.hours + playTime.TotalHours;
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
