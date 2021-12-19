using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBiome : Chunk
{
    public CommonBiome(Vector2Int pos) : base(pos)
    {
    }

    public override float chance {
        get 
        {
            return 1f;
        }
    }
    public override byte biomeId
    {
        get
        {
            return 0;
        }
    }
}
