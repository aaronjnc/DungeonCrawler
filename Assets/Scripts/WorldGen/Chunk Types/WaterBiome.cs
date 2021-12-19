using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBiome : Chunk
{
    public override float chance
    {
        get
        {
            return 0.45f;
        }
    }
    public override byte biomeId
    {
        get
        {
            return 1;
        }
    }
    public WaterBiome(Vector2Int pos) : base(pos)
    {
        biome = 1;
    }
}
