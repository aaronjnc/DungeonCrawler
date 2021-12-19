using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterBiome : Chunk
{
    public override float chance
    {
        get
        {
            return 0.45f;
        }
    }
    public WaterBiome(Vector2Int pos) : base(pos)
    {
        biomeId = 1;
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

    public override void GenerateChunk()
    {
        FillBiomeMap();
        GenerateMaps();
        RandomFillMap();
        for (int i = 0; i < smooths; i++)
        {
            SmoothMap(i);
        }
        //DetermineBlock();
        DetermineFloor();
        for (int i = 0; i < 1; i++)
        {
            SmoothRiver();
        }
        DetermineWall();
        SpecialBlockGeneration();
        DrawMap();
        generated = true;
    }

    private void DetermineFloor()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blocks[x, y] != 1)
                {
                    int id = ((random.Next(0, 100) < 33) ? 1 : 0);
                    floor[x, y] = biomeScripts[biomeId].floorBlocks[id].index;
                } 
                else
                {
                    floor[x, y] = biomeScripts[biomeId].floorBlocks[0].index;
                }
            }
        }
    }
    private void SmoothRiver()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (GetSurroundingFloor(x,y)<4)
                {
                    floor[x,y] = floor[x, y] = biomeScripts[biomeId].floorBlocks[0].index;
                }
                else
                {
                    floor[x, y] = floor[x, y] = biomeScripts[biomeId].floorBlocks[1].index;
                }
            }
        }
    }
    private byte GetSurroundingFloor(int gridX, int gridY)
    {
        byte riverIndex = (byte)biomeScripts[biomeId].floorBlocks[1].index;
        byte riverCount = 0;
        for (int x = gridX-1; x <= gridX+1; x++)
        {
            for (int y = gridY-1; y <= gridY+1; y++)
            {
                if (y < 0 || x < 0 || y >= height || x >= width || (x == gridX && y == gridY))
                {
                    continue;
                }
                if (floor[x,y] == riverIndex)
                {
                    riverCount++;
                }
            }
        }
        return riverCount;
    }

    private void DetermineWall()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!presetTiles.Contains(new Vector3Int(x, y, mapz)))
                {
                    if (blocks[x, y] == 1)
                    {
                        float maxWeight = 0f;
                        byte maxBlockIndex = 0;
                        List<Blocks> biomeBlocks = biomeScripts[biomes[x, y]].biomeBlocks;
                        for (int i = 0; i < biomeBlocks.Count; i++)
                        {
                            float weight = biomeBlocks[i].weight * Noise.Get2DPerlin(new Vector2Int(x, y), biomeseed, biomeBlocks[i].scale);
                            if (weight > maxWeight)
                            {
                                maxWeight = weight;
                                maxBlockIndex = (byte)i;
                            }
                        }
                        blocks[x, y] = biomeBlocks[maxBlockIndex].index;
                    }
                    else
                    {
                        int rando = UnityEngine.Random.Range(0, 100);
                        Vector3 worldPos = GetWorldPos(x, y, -1);
                        if (worldPos.x < 10 && worldPos.x > -10 && worldPos.y < 10 && worldPos.y > -10)
                            continue;
                        if (rando > enemyChance && manager.spawnEnemies && GetSurroundingWalls(x, y, 2) == 0 && numEnemies > 0)
                        {
                            SpawnEnemy(x, y);
                        }
                        else
                        {
                            DetermineEmptyType(x, y);
                        }
                        if (blocks[x, y] == 0)
                            Debug.Log(x + " " + y);
                    }
                }
            }
        }
    }
}
