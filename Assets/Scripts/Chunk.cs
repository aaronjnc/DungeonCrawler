using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    GameManager manager { get { return ChunkGen.currentWorld.manager; } }
    Tilemap map { get { return ChunkGen.currentWorld.map; } }
    Tilemap floor { get { return ChunkGen.currentWorld.floor; } }
    Tile[] walls { get { return ChunkGen.currentWorld.walls; } }
    Tile[] floors { get { return ChunkGen.currentWorld.floors; } }
    Tile empty { get { return ChunkGen.currentWorld.empty; } }
    int mapz { get { return ChunkGen.currentWorld.mapz; } }
    int floorz { get { return ChunkGen.currentWorld.floorz; } }
    int width { get { return ChunkGen.currentWorld.chunkWidth; } }
    int height { get { return ChunkGen.currentWorld.chunkHeight; } }
    int randomFillPercent { get { return ChunkGen.currentWorld.randomFillPercent; } }
    int randomBiomePercent { get { return ChunkGen.currentWorld.randomBiomePercent; } }
    int smooths { get { return ChunkGen.currentWorld.smooths; } }
    int biomeSmooths { get { return ChunkGen.currentWorld.biomesmooths; } }
    byte[,] blocks;
    byte[,] biomes;
    int seed;
    System.Random random;
    public bool generated = false;
    public Vector2Int chunkPos;
    List<Vector2Int> presetTiles = new List<Vector2Int>();
    public Chunk(Vector2Int pos)
    {
        chunkPos = pos;
        seed = ChunkGen.currentWorld.seed;
        seed -= pos.ToString().GetHashCode();
        blocks = new byte[width, height];
        biomes = new byte[width, height];
    }
    public void AddPreset(Vector2Int pos, byte tile)
    {
        blocks[pos.x, pos.y] = tile;
        presetTiles.Add(pos);
    }
    public void GenerateChunk()
    {
        RandomFillMap();
        for (int i = 0; i < smooths; i++)
        {
            SmoothMap(i);
        }
        for (int i = 0; i < biomeSmooths; i++)
        {
            SmoothBiomes();
        }
        DrawMap();
        generated = true;
    }
    void RandomFillMap()
    {
        random = new System.Random(seed);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (presetTiles.Contains(new Vector2Int(x, y)))
                    continue;
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    SmoothChunk(x, y);
                }
                else
                {
                    blocks[x, y] = (byte)((random.Next(0, 100) < randomFillPercent) ? 1 : 0);
                    biomes[x, y] = (byte)((random.Next(0, 100) < randomBiomePercent) ? 0 : 1);
                }
            }
        }
    }
    void SmoothChunk(int gridX, int gridY)
    {
        Vector2Int relPos = chunkPos;
        int newgridx = gridX;
        int newgridY = gridY;
        if (gridX == 0)
        {
            relPos.x--;
            newgridx = width - 1;
        }
        else if (gridX == width - 1)
        {
            relPos.x++;
            newgridx = 0;
        }
        if (gridY == 0)
        {
            relPos.y--;
            newgridY = height - 1;
        }
        else if (gridX == height - 1)
        {
            relPos.y++;
            newgridY = 0;
        }
        int blockFillVal = randomFillPercent;
        int biomeFillVal = randomBiomePercent;
        if (ChunkGen.currentWorld.ChunkGenerated(relPos))
        {
            /*blocks[gridX, gridY] = ChunkGen.currentWorld.AdjacentChunk(relPos).GetBlock(newgridx, newgridY);
            biomes[gridX,gridY] = ChunkGen.currentWorld.AdjacentChunk(relPos).GetBiome(newgridx, newgridY);*/
            byte block = ChunkGen.currentWorld.GetChunk(relPos).GetBlock(newgridx, newgridY);
            byte biome = ChunkGen.currentWorld.GetChunk(relPos).GetBiome(newgridx, newgridY);
            blockFillVal += (block == 1) ? 10 : -10;
            biomeFillVal += (biome == 0) ? 10 : -10;
        }
        /*else
        {
            blocks[gridX, gridY] = (byte)((random.Next(0, 100) < randomFillPercent) ? 1 : 0);
            biomes[gridX, gridY] = (byte)((random.Next(0, 100) < randomBiomePercent) ? 0 : 1);
        }*/
        blocks[gridX, gridY] = (byte)((random.Next(0, 100) < blockFillVal) ? 1 : 0);
        biomes[gridX, gridY] = (byte)((random.Next(0, 100) < biomeFillVal) ? 0 : 1);
    }
    void SmoothMap(int i)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (presetTiles.Contains(new Vector2Int(x, y)))
                    continue;
                byte walls = GetSurroundingWalls(x, y, i);
                if (walls > 4)
                    blocks[x, y] = 1;
                else if (walls < 4)
                    blocks[x, y] = 0;
            }
        }
    }
    byte GetSurroundingWalls(int gridX,int gridY, int i)
    {
        byte wallCount = 0;
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
                        calc = (y == gridY);
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
                    if (ChunkGen.currentWorld.ChunkGenerated(relPos))
                    {
                        Chunk adjacentChunk = ChunkGen.currentWorld.GetChunk(relPos);
                        wallCount += adjacentChunk.GetBlock(newgridX, newgridY);
                        if (calc && i == 1)
                        {
                            return (byte)((adjacentChunk.GetBlock(newgridX, newgridY) == 1) ? 5 : 0);
                        }
                    }
                    else
                    {
                        wallCount += (byte)((random.Next(0, 100) < randomFillPercent) ? 1 : 0);
                    }
                }
                else if (x != gridX || y != gridY)
                {
                    wallCount += blocks[x, y];
                }
            }
        }
        return wallCount;
    }
    void SmoothBiomes()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                byte biome = GetSurroundingBiomes(x, y);
                if (biome > 4)
                    biomes[x, y] = 1;
                else if (biome < 4)
                    biomes[x, y] = 0;
                if (blocks[x, y] == 0)
                {
                    biomes[x, y] = SurroundingWallBiomes(x, y);
                }
            }
        }
    }
    byte SurroundingWallBiomes(int gridX, int gridY)
    {
        for (int x = gridX - 1; x <= gridX + 1; x++)
        {
            for (int y = gridY - 1; y <= gridY + 1; y++)
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                    continue;
                if (blocks[x, y] == 0)
                    continue;
                return biomes[x, y];
            }
        }
        return biomes[gridX, gridY];
    }
    byte GetSurroundingBiomes(int gridX, int gridY)
    {
        byte biomeCount = 0;
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
                    if (ChunkGen.currentWorld.ChunkGenerated(relPos))
                    {
                        Chunk adjacentChunk = ChunkGen.currentWorld.GetChunk(relPos);
                        biomeCount += adjacentChunk.GetBiome(newgridX, newgridY);
                        if (calc)
                        {
                            return (byte)(5 * adjacentChunk.GetBiome(newgridX, newgridY));
                        }
                    }
                    else
                    {
                        biomeCount += (byte)((random.Next(0, 100) < randomFillPercent) ? 1 : 0);
                    }
                }
                else if (x != gridX || y != gridY)
                {
                    biomeCount += biomes[x, y];
                }
            }
        }
        return biomeCount;
    }
    void DrawMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x + (chunkPos.x * width), y + (chunkPos.y * height), mapz);
                Vector3Int floorPos = pos;
                floorPos.z = floorz;
                if (blocks[x, y] == 1)
                {
                    map.SetTile(pos, walls[biomes[x, y]]);
                }
                else
                {
                    map.SetTile(pos, empty);
                }
                floor.SetTile(floorPos, floors[biomes[x, y]]);
            }
        }
    }
    public byte GetBlock(int x, int y)
    {
        return blocks[x, y];
    }
    public byte GetBiome(int x, int y)
    {
        return biomes[x, y];
    }
    public Vector2Int topRight()
    {
        return new Vector2Int((chunkPos.x * width) + width - 1, (chunkPos.y * height) + height - 1);
    }
    public Vector2Int bottomLeft()
    {
        return new Vector2Int(chunkPos.x * width, chunkPos.y * height);
    }
}
