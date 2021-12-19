using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixedBiome : Chunk
{
    public MixedBiome(Vector2Int pos) : base(pos)
    {
    }

    public override float chance => throw new System.NotImplementedException();

    public override byte biomeId => throw new System.NotImplementedException();
}
