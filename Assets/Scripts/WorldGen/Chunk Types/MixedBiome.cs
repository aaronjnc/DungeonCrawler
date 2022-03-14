using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixedBiome : Chunk
{
    public MixedBiome(Vector2Int pos) : base(pos)
    {
        biomeId = 127;
    }

    public override float chance => throw new System.NotImplementedException();


    public override void GenerateChunk()
    {
        FillBiomeMap();
        CreateTileMaps();
        RandomFillMap();
        for (int i = 0; i < smooths; i++)
        {
            SmoothMap(i);
        }
        for (int i = 0; i < biomesmooths; i++)
        {
            SmoothBiomes();
        }
        DetermineBlock();
        SpecialBlockGeneration();
        AddChangedBlocks();
        DrawTileMap();
        generated = true;
    }

    protected override void FillBiomeMap()
    {
        byte[] surroundingBiomes = new byte[4];
        if (!ChunkGen.Instance.ChunkCreated(chunkPos + new Vector2Int(0, 1)))
        {
            ChunkGen.Instance.CreateChunk(chunkPos + new Vector2Int(0, 1));
        }
        surroundingBiomes[0] = ChunkGen.Instance.GetChunk(chunkPos + new Vector2Int(0, 1)).biomeId;
        if (!ChunkGen.Instance.ChunkCreated(chunkPos + new Vector2Int(1, 0)))
        {
            ChunkGen.Instance.CreateChunk(chunkPos + new Vector2Int(1, 0));
        }
        surroundingBiomes[1] = ChunkGen.Instance.GetChunk(chunkPos + new Vector2Int(1, 0)).biomeId;
        if (!ChunkGen.Instance.ChunkCreated(chunkPos + new Vector2Int(0, -1)))
            ChunkGen.Instance.CreateChunk(chunkPos + new Vector2Int(0, -1));
        surroundingBiomes[2] = ChunkGen.Instance.GetChunk(chunkPos + new Vector2Int(0, -1)).biomeId;
        if (!ChunkGen.Instance.ChunkCreated(chunkPos + new Vector2Int(-1, 0)))
            ChunkGen.Instance.CreateChunk(chunkPos + new Vector2Int(-1, 0));
        surroundingBiomes[3] = ChunkGen.Instance.GetChunk(chunkPos + new Vector2Int(-1, 0)).biomeId;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float[] chance = new float[4];
                chance[0] = Mathf.Clamp(100f - 2.5f * (64 - y), 0f, 100f); //top
                chance[1] = Mathf.Clamp(100f - 2.5f * (64 - x), 0f, 100f) + chance[0]; //right
                chance[2] = Mathf.Clamp(100f - 2.5f * y, 0f, 100f) + chance[1]; //bottom
                chance[3] = Mathf.Clamp(100f - 2.5f * x, 0f, 100f) + chance[2]; //left
                byte index = 0;
                float randomNum = UnityEngine.Random.Range(0, chance[3]);
                for (int i = 0; i < 4; i++)
                {
                    if (chance[i] >= randomNum)
                    {
                        index = surroundingBiomes[i];
                        break;
                    }
                }
                biomes[x, y] = index;
            }
        }
    }
    /// <summary>
    /// Smooths out the biomes so appear less random
    /// </summary>
    private void SmoothBiomes()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                byte[] biome = GetSurroundingBiomes(x, y);
                if (biome[biomes[x, y]] <= 1)
                {
                    byte maxBiomeIndex = 0;
                    byte maxBiomeWeight = 0;
                    for (int i = 0; i < biome.Length; i++)
                    {
                        byte weight = biome[i];
                        if (weight > maxBiomeWeight)
                        {
                            maxBiomeWeight = weight;
                            maxBiomeIndex = (byte)i;
                        }
                    }
                    biomes[x, y] = maxBiomeIndex;
                }
                if (blocks[x, y] == 0)
                {
                    biomes[x, y] = SurroundingWallBiomes(x, y);
                }
            }
        }
    }

    /// <summary>
    /// Returns the number of generic biome tiles surrounding the given position
    /// </summary>
    /// <param name="gridX">X position of center tile</param>
    /// <param name="gridY">Y position of center tile</param>
    /// <returns></returns>
    private byte[] GetSurroundingBiomes(int gridX, int gridY)
    {
        byte[] biomeCount = new byte[biomeScripts.Length];
        for (int i = 0; i < biomeCount.Length; i++)
        {
            biomeCount[i] = 0;
        }
        for (int x = gridX - 1; x <= gridX + 1; x++)
        {
            for (int y = gridY - 1; y <= gridY + 1; y++)
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    Vector2Int relPos = chunkPos;
                    int newgridX = gridX;
                    int newgridY = gridY;
                    bool calc = false;
                    if (x < 0)
                    {
                        if (y == gridY)
                            calc = true;
                        relPos += new Vector2Int(-1, 0);
                        newgridX = width - 1;
                    }
                    else if (x >= width)
                    {
                        calc = (y == gridY);
                        relPos += new Vector2Int(1, 0);
                        newgridX = 0;
                    }
                    if (y < 0)
                    {
                        calc = (x == gridX);
                        relPos += new Vector2Int(0, -1);
                        newgridY = height - 1;
                    }
                    else if (y >= height)
                    {
                        calc = (x == gridX);
                        relPos += new Vector2Int(0, 1);
                        newgridY = 0;
                    }
                    if (ChunkGen.Instance.ChunkGenerated(relPos))
                    {
                        Chunk adjacentChunk = ChunkGen.Instance.GetChunk(relPos);
                        biomeCount[adjacentChunk.GetTileBiome(newgridX, newgridY)] += 1;
                    }
                }
                else if (x != gridX || y != gridY)
                {
                    biomeCount[biomes[x, y]] += 1;
                }
            }
        }
        return biomeCount;
    }
}
