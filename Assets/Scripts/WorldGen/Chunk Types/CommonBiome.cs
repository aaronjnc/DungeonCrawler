using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBiome : Chunk
{
    public CommonBiome(Vector2Int pos) : base(pos)
    {
        biomeId = 0;
    }

    public override float chance {
        get 
        {
            return 1f;
        }
    }
    public override void GenerateChunk()
    {
        FillBiomeMap();
        CreateTileMaps();
        RandomFillMap();
        for (int i = 0; i < smooths; i++)
        {
            SmoothMap(i);
        }
        DetermineBlock();
        SpecialBlockGeneration();
        AddChangedBlocks();
        DrawTileMap();
        generated = true;
    }
    protected override void FillBiomeMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                biomes[i, j] = biomeId;
            }
        }
    }
}
