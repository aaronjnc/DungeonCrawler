using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldInfo
{
    private int worldSeed;
    private int biomeSeed;
    private double playTime;
    public WorldInfo(ChunkGen gen, GameManager manager)
    {
        worldSeed = gen.seed;
        biomeSeed = gen.biomeseed;
        TimeSpan playTime = System.DateTime.Now.Subtract(manager.startTime);
        this.playTime = manager.hours + playTime.TotalHours;
    }
    public int GetWorldSeed()
    {
        return worldSeed;
    }
    public int GetBiomeSeed()
    {
        return biomeSeed;
    }
    public double GetPlayTime()
    {
        return playTime;
    }
}
