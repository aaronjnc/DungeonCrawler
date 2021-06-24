using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    GameManager manager { get { return ChunkGen.currentWorld.manager; } }
    public Tilemap map;
    public Tilemap floor;
    GameObject floormap { get { return ChunkGen.currentWorld.floormap; } }
    GameObject tilemap { get { return ChunkGen.currentWorld.map; } }
    Transform grid { get { return ChunkGen.currentWorld.grid; } }
    Biomes[] biomeScripts { get { return ChunkGen.currentWorld.biomes; } }
    int mapz { get { return ChunkGen.currentWorld.mapz; } }
    int floorz { get { return ChunkGen.currentWorld.floorz; } }
    int width { get { return ChunkGen.currentWorld.chunkWidth; } }
    int height { get { return ChunkGen.currentWorld.chunkHeight; } }
    int randomFillPercent { get { return ChunkGen.currentWorld.randomFillPercent; } }
    int smooths { get { return ChunkGen.currentWorld.smooths; } }
    int biomesmooths { get { return ChunkGen.currentWorld.biomesmooths; } }
    float enemyChance { get { return ChunkGen.currentWorld.enemyChance; } }
    byte[,] blocks;
    byte[,] biomes;
    int seed;
    int biomeseed;
    System.Random random;
    public bool generated = false;
    public Vector2Int chunkPos;
    List<Vector2Int> presetTiles = new List<Vector2Int>();
    public Chunk(Vector2Int pos)
    {
        chunkPos = pos;
        seed = ChunkGen.currentWorld.seed;
        seed -= pos.ToString().GetHashCode();
        biomeseed = ChunkGen.currentWorld.biomeseed;
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
        for (int i = 0; i < biomesmooths;i++)
        {
            SmoothBiomes();
        }
        DetermineBlock();
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
                }
                float strongestWeight = 0f;
                byte strongestBiomeIndex = 0;
                for (int i = 0; i < biomeScripts.Length;i++)
                {
                    float weight = biomeScripts[i].weight*Noise.Get2DPerlin(new Vector2Int(chunkPos.x * width + x, chunkPos.y * height + y), biomeseed, biomeScripts[i].scale);
                    if (weight > strongestWeight)
                    {
                        strongestWeight = weight;
                        strongestBiomeIndex = (byte)i;
                    }
                }
                biomes[x, y] = strongestBiomeIndex;
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
        if (ChunkGen.currentWorld.ChunkGenerated(relPos))
        {
            /*blocks[gridX, gridY] = ChunkGen.currentWorld.AdjacentChunk(relPos).GetBlock(newgridx, newgridY);
            biomes[gridX,gridY] = ChunkGen.currentWorld.AdjacentChunk(relPos).GetBiome(newgridx, newgridY);*/
            byte block = ChunkGen.currentWorld.GetChunk(relPos).GetBlock(newgridx, newgridY);
            blockFillVal += (block == 1) ? 10 : -10;
        }
        /*else
        {
            blocks[gridX, gridY] = (byte)((random.Next(0, 100) < randomFillPercent) ? 1 : 0);
            biomes[gridX, gridY] = (byte)((random.Next(0, 100) < randomBiomePercent) ? 0 : 1);
        }*/
        blocks[gridX, gridY] = (byte)((random.Next(0, 100) < blockFillVal) ? 1 : 0);
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
                        wallCount += GetType(adjacentChunk.GetBlock(newgridX, newgridY));
                        if (calc && i == 1)
                        {
                            return (byte)((GetType(adjacentChunk.GetBlock(newgridX, newgridY)) == 1) ? 5 : 0);
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
                }
                else if (x != gridX || y != gridY)
                {
                    biomeCount += biomes[x, y];
                }
            }
        }
        return biomeCount;
    }
    void DetermineBlock()
    {
        for (int x = 0; x < width;x++)
        {
            for (int y = 0; y < height;y++)
            {
                if (!presetTiles.Contains(new Vector2Int(x,y)))
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
                        if (Random.Range(0,100) < enemyChance && manager.spawnEnemies)
                        {
                            float maxWeightEnemy = 0f;
                            byte maxEnemyIndex = 0;
                            List<GameObject> enemies = biomeScripts[biomes[x, y]].enemies;
                            for (int i = 0; i < enemies.Count;i++)
                            {
                                EnemyInfo enemy = enemies[i].GetComponent<EnemyInfo>();
                                float weight = enemy.weight * Noise.Get2DPerlin(new Vector2Int(x, y), biomeseed, enemy.scale);
                                if (weight > maxWeightEnemy)
                                {
                                    maxWeightEnemy = weight;
                                    maxEnemyIndex = (byte)i;
                                }
                            }
                            blocks[x, y] = 127;
                            GameObject enemyObj = GameObject.Instantiate(enemies[maxEnemyIndex]) as GameObject;
                            enemyObj.transform.position = new Vector3(x + width * chunkPos.x, y + height * chunkPos.y, -5);
                        }
                        else
                        {
                            float maxWeightEmpty = 0f;
                            byte maxEmptyIndex = 0;
                            List<Blocks> emptyBlocks = biomeScripts[biomes[x, y]].emptyBlocks;
                            for (int i = 0; i < emptyBlocks.Count; i++)
                            {
                                float weight = emptyBlocks[i].weight * Noise.Get2DPerlin(new Vector2Int(x, y), biomeseed, emptyBlocks[i].scale);
                                if (weight > maxWeightEmpty)
                                {
                                    maxWeightEmpty = weight;
                                    maxEmptyIndex = (byte)i;
                                }
                            }
                            blocks[x, y] = emptyBlocks[maxEmptyIndex].index;
                        }
                    }
                }
            }
        }
    }
    void DrawMap()
    {
        GenerateMaps();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, mapz);
                Vector3Int floorPos = pos;
                floorPos.z = floorz;
                if (blocks[x,y] == 127)
                {
                    map.SetTile(pos, null);
                }
                else
                {
                    map.SetTile(pos, manager.GetBlock(blocks[x, y]).tile);
                }
                floor.SetTile(floorPos, biomeScripts[biomes[x, y]].baseFloor.tile);
            }
        }
    }
    void GenerateMaps()
    {
        GameObject newMap = GameObject.Instantiate(tilemap, grid);
        newMap.transform.position = new Vector3(chunkPos.x * width, chunkPos.y * height,mapz);
        GameObject newFloor = GameObject.Instantiate(floormap, grid);
        newFloor.transform.position = new Vector3(chunkPos.x * width, chunkPos.y * height, floorz);
        map = newMap.GetComponent<Tilemap>();
        floor = newFloor.GetComponent<Tilemap>();
    }
    byte GetType(byte tile)
    {
        if (tile == 127)
            return 0;
        if (manager.GetBlock(tile).blockType == Blocks.Type.Wall)
            return 1;
        else
            return 0;
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
    public void UpdateByte(int x, int y, byte block)
    {
        blocks[x, y] = block;
        if (block == 127)
            map.SetTile(new Vector3Int(x, y, mapz), null);
        else
        {
            map.SetTile(new Vector3Int(x, y, mapz), manager.GetBlock(block).tile);
            if (manager.GetBlock(block).solid)
                UpdateCollider(x, y, Tile.ColliderType.Grid);
        }
        map.RefreshTile(new Vector3Int(x, y, mapz));
    }
    public void UnloadChunk()
    {
        map.GetComponent<TilemapRenderer>().enabled = false;
        floor.GetComponent<TilemapRenderer>().enabled = false;
    }
    public void LoadChunk()
    {
        map.GetComponent<TilemapRenderer>().enabled = true;
        floor.GetComponent<TilemapRenderer>().enabled = true;
    }
    public void UpdateColor(int x, int y, Color newColor)
    {
        map.GetTile<Tile>(new Vector3Int(x, y, mapz)).color = newColor;
        map.RefreshTile(new Vector3Int(x, y, mapz));
    }
    public Tile GetTile(Vector3Int tilePos)
    {
        return map.GetTile<Tile>(tilePos);
    }
    public void UpdateCollider(int x, int y, Tile.ColliderType tileCollider)
    {
        map.GetTile<Tile>(new Vector3Int(x, y, mapz)).colliderType = tileCollider;
    }
}
