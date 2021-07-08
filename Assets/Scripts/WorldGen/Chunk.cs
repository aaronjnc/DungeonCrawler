﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class Chunk
{
    GameManager manager { get { return ChunkGen.currentWorld.manager; } }
    public Tilemap map;
    GameObject tilemap { get { return ChunkGen.currentWorld.map; } }
    Transform grid { get { return ChunkGen.currentWorld.grid; } }
    Biomes[] biomeScripts { get { return ChunkGen.currentWorld.biomes; } }
    int mapz = 0;
    int floorz = 1;
    int width { get { return ChunkGen.currentWorld.chunkWidth; } }
    int height { get { return ChunkGen.currentWorld.chunkHeight; } }
    int randomFillPercent { get { return ChunkGen.currentWorld.randomFillPercent; } }
    int smooths { get { return ChunkGen.currentWorld.smooths; } }
    int biomesmooths { get { return ChunkGen.currentWorld.biomesmooths; } }
    float enemyChance { get { return ChunkGen.currentWorld.enemyChance; } }
    int maxenemies { get { return ChunkGen.currentWorld.maxenemies; } }
    Transform enemyParent { get { return ChunkGen.currentWorld.enemyParent; } }
    int specialTileChance { get { return ChunkGen.currentWorld.specialTileChance; } }
    byte[,] blocks;
    byte[,] biomes;
    byte[,] floor;
    int seed;
    int biomeseed;
    int specialTileCount;
    int numEnemies;
    System.Random random;
    public bool generated = false;
    public Vector2Int chunkPos;
    List<Vector2Int> presetTiles = new List<Vector2Int>();
    Dictionary<Vector2Int, InteractableTile> specialTiles = new Dictionary<Vector2Int, InteractableTile>();
    Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();
    /// <summary>
    /// Initializes chunk script at given chunk position
    /// </summary>
    /// <param name="pos">The position of the chunk</param>
    public Chunk(Vector2Int pos)
    {
        chunkPos = pos;
        seed = ChunkGen.currentWorld.seed;
        seed -= pos.ToString().GetHashCode();
        biomeseed = ChunkGen.currentWorld.biomeseed;
        blocks = new byte[width, height];
        biomes = new byte[width, height];
        floor = new byte[width, height];
        numEnemies = Random.Range(1, maxenemies+1);
    }
    /// <summary>
    /// Saves the position and type of tile that should be located at that position
    /// </summary>
    /// <param name="pos">Chunk position of the tile</param>
    /// <param name="tile">ID of the tile</param>
    public void AddPreset(Vector2Int pos, byte tile)
    {
        blocks[pos.x, pos.y] = tile;
        presetTiles.Add(pos);
    }
    /// <summary>
    /// Creates the array of blocks within the chunk and places it on the tilemap
    /// </summary>
    public void GenerateChunk()
    {
        specialTileCount = Random.Range(0, 2);
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
    /// <summary>
    /// Generates array of bytes determining type of block (empty or wall) and biome
    /// </summary>
    void RandomFillMap()
    {
        random = new System.Random(seed);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (presetTiles.Contains(new Vector2Int(x, y)))
                {
                    AddInteractable(x, y);
                    continue;
                }
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
    /// <summary>
    /// Sets block equal to adjacent block in neighboring chunk to provide more clean transitions between chunks
    /// </summary>
    /// <param name="gridX">X position of the tile</param>
    /// <param name="gridY">Y position of the tile</param>
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
            byte block = ChunkGen.currentWorld.GetChunk(relPos).GetBlock(newgridx, newgridY);
            blockFillVal += (block == 1) ? 10 : -10;
        }
        blocks[gridX, gridY] = (byte)((random.Next(0, 100) < blockFillVal) ? 1 : 0);
    }
    /// <summary>
    /// Goes through block array and smooths out the walls so it appears less random
    /// </summary>
    /// <param name="i">The iteration number</param>
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
    /// <summary>
    /// Returns the number of surrounding wall tiles
    /// </summary>
    /// <param name="gridX">X position of center tile</param>
    /// <param name="gridY">Y position of center tile</param>
    /// <param name="i">Iteration number</param>
    /// <returns></returns>
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
                    wallCount += GetType(blocks[x, y]);
                }
            }
        }
        return wallCount;
    }
    /// <summary>
    /// Smooths out the biomes so appear less random
    /// </summary>
    void SmoothBiomes()
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
                    for (int i = 0; i < biome.Length;i++)
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
    /// Returns the biomes of adjacent walls so the floor biome will match the walls
    /// </summary>
    /// <param name="gridX">X position of center tile</param>
    /// <param name="gridY">Y position of center tile</param>
    /// <returns></returns>
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
    /// <summary>
    /// Returns the number of generic biome tiles surrounding the given position
    /// </summary>
    /// <param name="gridX">X position of center tile</param>
    /// <param name="gridY">Y position of center tile</param>
    /// <returns></returns>
    byte[] GetSurroundingBiomes(int gridX, int gridY)
    {
        byte[] biomeCount = new byte[biomeScripts.Length];
        for (int i = 0; i < biomeCount.Length;i++)
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
                    if (ChunkGen.currentWorld.ChunkGenerated(relPos))
                    {
                        Chunk adjacentChunk = ChunkGen.currentWorld.GetChunk(relPos);
                        biomeCount[adjacentChunk.GetBiome(newgridX,newgridY)] += 1;
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
    /// <summary>
    /// Refills the array after generation of empty or wall blocks with the actual type of block
    /// </summary>
    void DetermineBlock()
    {
        for (int x = 0; x < width;x++)
        {
            for (int y = 0; y < height;y++)
            {
                float strongFloorWeight = 0f;
                byte strongFloorIndex = 0;
                for (int i = 0; i < biomeScripts[biomes[x, y]].floorBlocks.Count; i++)
                {
                    float weight = biomeScripts[biomes[x, y]].floorBlocks[i].weight *
                        Noise.Get2DPerlin(new Vector2Int(chunkPos.x * width + x, chunkPos.y * height),
                        biomeseed, biomeScripts[biomes[x, y]].floorBlocks[i].scale);
                    if (weight > strongFloorWeight)
                    {
                        strongFloorWeight = weight;
                        strongFloorIndex = biomeScripts[biomes[x, y]].floorBlocks[i].index;
                    }
                }
                floor[x, y] = strongFloorIndex;
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
                        int rando = Random.Range(0, 100);
                        Vector3 worldPos = GetWorldPos(x, y, -1);
                        if (worldPos.x < 10 && worldPos.x > -10 && worldPos.y < 10 && worldPos.y > -10)
                            continue;
                        if (rando > enemyChance && manager.spawnEnemies && GetSurroundingWalls(x,y,2)==0 && numEnemies > 0)
                        {
                            SpawnEnemy(x,y);
                        }
                        else if (rando > specialTileChance || specialTileCount==0)
                        {
                            DetermineEmptyType(x, y);
                        }
                        else
                        {
                            GenerateSpecial(x, y);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Spawn enemy at given location
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    void SpawnEnemy(int x, int y)
    {
        blocks[x, y] = 127;
        numEnemies--;
        int maxIndex = 0;
        float maxIndexWeight = 0;
        for (int i = 0; i < biomeScripts[biomes[x, y]].enemies.Count; i++)
        {
            float weight = Noise.Get2DPerlin(new Vector2Int(x, y), seed, biomeScripts[biomes[x, y]].enemies[i].GetComponent<EnemyInfo>().weight);
            if (weight > maxIndexWeight)
            {
                maxIndex = (byte)i;
                maxIndexWeight = weight;
            }
        }
        GameObject enemy = GameObject.Instantiate(biomeScripts[biomes[x, y]].enemies[maxIndex], enemyParent) as GameObject;
        enemy.transform.position = GetWorldPos(x, y, -1);
        enemy.GetComponent<EnemyInfo>().chunk = chunkPos;
        enemies.Add(enemy.GetHashCode(), enemy);
    }
    /// <summary>
    /// Determines empty tile type at given location
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    void DetermineEmptyType(int x, int y)
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
    /// <summary>
    /// Determine special tile type
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    void GenerateSpecial(int x, int y)
    {
        specialTileCount--;
        int maxIndex = 0;
        float maxIndexWeight = 0;
        for (int i = 0; i < biomeScripts[biomes[x, y]].specialBlocks.Count; i++)
        {
            float weight = biomeScripts[biomes[x, y]].specialBlocks[i].weight *
                Noise.Get2DPerlin(new Vector2Int(x, y), seed, biomeScripts[biomes[x, y]].specialBlocks[i].scale);
            if (weight > maxIndexWeight)
            {
                maxIndex = (byte)i;
                maxIndexWeight = weight;
            }
        }
        Blocks specialBlock = biomeScripts[biomes[x, y]].specialBlocks[maxIndex];
        if (specialBlock.blockType == Blocks.Type.Floor)
        {
            floor[x, y] = specialBlock.index;
            blocks[x, y] = 127;
        }
        else
        {
            blocks[x, y] = specialBlock.index;
            AddInteractable(x, y);
        }
    }
    /// <summary>
    /// Places the tiles related to the block array in the tilemap
    /// </summary>
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
                SetTile(pos, blocks[x, y]);
                if (blocks[x,y] == 127)
                    SetTile(floorPos, floor[x,y]);
            }
        }
        if (manager.spawnEnemies)
            BuildNavMeshes();
    }
    /// <summary>
    /// Changes the tile at the given position using the new block index (used during inital generation)
    /// </summary>
    /// <param name="tilePos">Chunk pos of the tile</param>
    /// <param name="index">Index of the new block</param>
    void SetTile(Vector3Int tilePos, byte index)
    {
        if (index == 127)
        {
            map.SetTile(tilePos, null);
        }
        else
        {
            Blocks block = manager.GetBlock(index);
            map.SetTile(tilePos, block.tile);
            if (block.solid)
            {
                map.GetTile<Tile>(tilePos).colliderType = Tile.ColliderType.Grid;
            }
            else
                map.GetTile<Tile>(tilePos).colliderType = Tile.ColliderType.None;
        }
        map.RefreshTile(tilePos);
    }
    /// <summary>
    /// Generates the tilemap
    /// </summary>
    void GenerateMaps()
    {
        GameObject newMap = GameObject.Instantiate(tilemap, grid);
        newMap.transform.position = new Vector3(chunkPos.x * width, chunkPos.y * height,mapz);
        map = newMap.GetComponent<Tilemap>();
    }
    /// <summary>
    /// Determine whether tile ID is an empty or wall block
    /// </summary>
    /// <param name="tile">ID of the block</param>
    /// <returns></returns>
    byte GetType(byte tile)
    {
        if (tile == 127)
            return 0;
        if (manager.GetBlock(tile).blockType == Blocks.Type.Wall)
            return 1;
        else
            return 0;
    }
    /// <summary>
    /// returns the block ID at the coordinates
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    /// <returns></returns>
    public byte GetBlock(int x, int y)
    {
        return blocks[x, y];
    }
    /// <summary>
    /// Returns the biome number at the coordinates
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    /// <returns></returns>
    public byte GetBiome(int x, int y)
    {
        return biomes[x, y];
    }
    /// <summary>
    /// Changes the map and block id at the given location, and updates the collider
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    /// <param name="block">ID of new block</param>
    public void UpdateByte(int x, int y, byte block)
    {
        blocks[x, y] = block;
        if (block == 127)
        {
            map.SetTile(new Vector3Int(x, y, mapz), null);
            map.SetTile(new Vector3Int(x, y, floorz), manager.GetBlock(floor[x,y]).tile);
        }
        else
        {
            map.SetTile(new Vector3Int(x, y, mapz), manager.GetBlock(block).tile);
            if (manager.GetBlock(block).solid)
                UpdateCollider(x, y,Tile.ColliderType.Grid);
            map.SetTile(new Vector3Int(x, y, floorz), null);
        }
        map.RefreshTile(new Vector3Int(x, y, mapz));
        if (manager.spawnEnemies)
            BuildNavMeshes();
    }
    /// <summary>
    /// Disables the renderer of the map and enemies
    /// </summary>
    public void UnloadChunk()
    {
        map.GetComponent<TilemapRenderer>().enabled = false;
        foreach(GameObject enemy in enemies.Values)
        {
            enemy.SetActive(false);
        }
    }
    /// <summary>
    /// Activates the map renderer and enemies
    /// </summary>
    public void LoadChunk()
    {
        map.GetComponent<TilemapRenderer>().enabled = true;
        foreach(GameObject enemy in enemies.Values)
        {
            enemy.SetActive(true);
        }
    }
    /// <summary>
    /// Changes the tile at location to be a new color of the same tile to indicate it being highlighted/unhighlighted
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    /// <param name="newTile">Tile with new color scheme</param>
    public void UpdateColor(int x, int y, Tile newTile)
    {
        newTile.sprite = map.GetTile<Tile>(new Vector3Int(x, y, mapz)).sprite;
        map.SetTile(new Vector3Int(x, y, mapz),newTile);
        map.RefreshTile(new Vector3Int(x, y, mapz));
    }
    /// <summary>
    /// Returns the tile in the tilemap at the given position
    /// </summary>
    /// <param name="tilePos">Chunk pos of the tile</param>
    /// <returns></returns>
    public Tile GetTile(Vector3Int tilePos)
    {
        tilePos.z = mapz;
        return map.GetTile<Tile>(tilePos);
    }
    /// <summary>
    /// Updates the tile collider type at the given location in the wall
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    /// <param name="tileCollider">Type of tilecollider</param>
    public void UpdateCollider(int x, int y, Tile.ColliderType tileCollider)
    {
        map.GetTile<Tile>(new Vector3Int(x, y, mapz)).colliderType = tileCollider;
    }
    /// <summary>
    /// Add new interactable script for tiles that can be entered
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    public void AddInteractable(int x, int y)
    {
        if (blocks[x, y] == 127)
            return;
        if (manager.GetBlock(blocks[x, y]).interactable)
        {
            InteractableTile newInteract = new InteractableTile();
            newInteract.SetUp(blocks[x, y]);
            specialTiles.Add(new Vector2Int(x, y), newInteract);
        }
    }
    /// <summary>
    /// Interact with tile at given position
    /// </summary>
    /// <param name="interactPos">Chunk tile position</param>
    public void Interact(Vector2Int interactPos)
    {
        if (specialTiles.ContainsKey(interactPos))
            specialTiles[interactPos].Interact();
    }
    /// <summary>
    /// Returns the world position given chunk relative position
    /// </summary>
    /// <param name="x">Chunk relative x</param>
    /// <param name="y">Chunk relative y</param>
    /// <param name="z">Chunk relative z</param>
    /// <returns></returns>
    Vector3 GetWorldPos(int x, int y, int z)
    {
        Vector3 worldPos = Vector3.zero;
        if (chunkPos.x < 0)
            worldPos.x = x - width + (chunkPos.x + 1) * width;
        else
            worldPos.x = x + chunkPos.x * width;
        if (chunkPos.y < 0)
            worldPos.y = y - height + (chunkPos.y + 1) * height;
        else
            worldPos.y = y + (chunkPos.y) * height;
        worldPos.z = z;
        return worldPos;
    }
    /// <summary>
    /// Bakes all active nav meshes
    /// </summary>
    void BuildNavMeshes()
    {
        foreach (NavMeshSurface2d surface in NavMeshSurface2d.activeSurfaces.ToArray())
        {
            surface.BuildNavMesh();
        }
    }
    /// <summary>
    /// Called to kill enemy and open up spawn spot
    /// </summary>
    /// <param name="enemy">Gameobject for enemy</param>
    public void KillEnemy(GameObject enemy)
    {
        enemies.Remove(enemy.GetHashCode());
        GameObject.Destroy(enemy);
        Debug.Log("Killed");
    }
}
