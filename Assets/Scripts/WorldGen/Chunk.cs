﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using System;
using static System.Collections.Generic.Dictionary<int, UnityEngine.GameObject>;

public abstract class Chunk
{
    public abstract float chance { get; }
    [Tooltip("Biome ID of this chunk")]
    public byte biomeId;
    [Tooltip("Tilemap used for this chunk")]
    [SerializeField] private Tilemap map;
    protected GameObject tilemap { get { return ChunkGen.Instance.map; } }
    protected Transform grid { get { return ChunkGen.Instance.grid; } }
    protected Biomes[] biomeScripts { get { return ChunkGen.Instance.biomes; } }
    [Tooltip("Wall z position")]
    protected int mapz = 0;
    [Tooltip("Floor z position")]
    protected int floorz = 1;
    protected int width { get { return ChunkGen.Instance.chunkWidth; } }
    protected int height { get { return ChunkGen.Instance.chunkHeight; } }
    protected int randomFillPercent { get { return ChunkGen.Instance.randomFillPercent; } }
    protected int smooths { get { return ChunkGen.Instance.smooths; } }
    protected int biomesmooths { get { return ChunkGen.Instance.biomesmooths; } }
    protected float monsterChance { get { return ChunkGen.Instance.monsterChance; } }
    protected int maxMonsters { get { return ChunkGen.Instance.maxMonsters; } }
    protected Transform monsterParent { get { return ChunkGen.Instance.monsterParent; } }
    [Tooltip("Array of bytes representing chunk walls")]
    protected byte[,] blocks;
    [Tooltip("Array of bytes representing block biomes")]
    protected byte[,] biomes;
    [Tooltip("Array of bytes representing floor blocks in chunk")]
    protected byte[,] floor;
    [Tooltip("Chunk seed")]
    protected int seed;
    [Tooltip("Biome seed")]
    protected int biomeseed;
    [Tooltip("Number of monsters to spawn in this chunk")]
    protected int numMonsters;
    [Tooltip("Chunk is generated")]
    public bool generated = false;
    [Tooltip("Chunk position")]
    public Vector2Int chunkPos;
    [Tooltip("List of preset tile positions")]
    protected List<Vector3Int> presetTiles = new List<Vector3Int>();
    [Tooltip("Dictionary of preset monsters and their positon")]
    protected Dictionary<Vector2Int, byte> presetMonsters = new Dictionary<Vector2Int, byte>();
    [Tooltip("List of monsters in chunk")]
    protected Dictionary<int, GameObject> monsters = new Dictionary<int, GameObject>();
    [Tooltip("List of interactables in chunk")]
    protected List<GameObject> interactables = new List<GameObject>();
    [Tooltip("List of changed blocks in chunk")]
    protected Dictionary<Vector2Int, byte> changes = new Dictionary<Vector2Int, byte>();
    [Tooltip("Chunk has been changed")]
    public bool changed = false;
    [Tooltip("Used for chunks that are entirely pregenerated")]
    public bool specialChunk = false;
    [Tooltip("Number representing order of chunk generation")]
    protected int numGen;
    protected System.Random rand;
    protected List<Vector2Int> emptyBlocks = new List<Vector2Int>();
    protected abstract void FillBiomeMap();
    /// <summary>
    /// Initializes chunk script at given chunk position
    /// </summary>
    /// <param name="pos">The position of the chunk</param>
    public Chunk(Vector2Int pos)
    {
        chunkPos = pos;
        seed = ChunkGen.Instance.seed;
        seed -= pos.ToString().GetHashCode();
        biomeseed = ChunkGen.Instance.biomeseed;
        blocks = new byte[width, height];
        biomes = new byte[width, height];
        floor = new byte[width, height];
        rand = new System.Random(seed);
        numMonsters = rand.Next(1, maxMonsters + 1);
    }
    /// <summary>
    /// Saves the position and type of tile that should be located at that position
    /// </summary>
    /// <param name="pos">Chunk position of the tile</param>
    /// <param name="tile">ID of the tile</param>
    public void AddPresetTile(Vector3Int pos, byte tile)
    {
        if (pos.z == 0)
            blocks[pos.x, pos.y] = tile;
        else
            floor[pos.x, pos.y] = tile;
        presetTiles.Add(pos);
    }
    /// <summary>
    /// Adds monsters to list of preset
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="monster"></param>
    public void AddPresetMonster(Vector2Int pos, byte monster)
    {
        presetMonsters.Add(pos, monster);
    }
    /// <summary>
    /// Method used to call other methods and generate the chunk
    /// </summary>
    public abstract void GenerateChunk(int genNum);
    /// <summary>
    /// Generates array of bytes determining type of block (empty or wall)
    /// </summary>
    protected void RandomFillMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (presetTiles.Contains(new Vector3Int(x, y,mapz)))
                {
                    AddInteractable(x, y);
                    continue;
                }
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    SmoothEdgeBlocks(x, y);
                }
                else
                {
                    blocks[x, y] = (byte)((rand.Next(0, 100) < randomFillPercent) ? 1 : 0);
                }
            }
        }
    }
    /// <summary>
    /// Sets block equal to adjacent block in neighboring chunk to provide more clean transitions between chunks
    /// </summary>
    /// <param name="gridX">X position of the tile</param>
    /// <param name="gridY">Y position of the tile</param>
    protected void SmoothEdgeBlocks(int gridX, int gridY)
    {
        if (specialChunk)
            return;
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
        if (ChunkGen.Instance.ChunkGenerated(relPos))
        {
            byte block = ChunkGen.Instance.GetChunk(relPos).GetBlockID(newgridx, newgridY);
            blockFillVal += (block == 1) ? 10 : -10;
        }
        blocks[gridX, gridY] = (byte)((rand.Next(0, 100) < blockFillVal) ? 1 : 0);
    }
    /// <summary>
    /// Goes through block array and smooths out the walls so it appears less random
    /// </summary>
    /// <param name="i">The iteration number</param>
    protected void SmoothMap(int i)
    {
        if (specialChunk)
            return;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (presetTiles.Contains(new Vector3Int(x, y,mapz)))
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
    protected byte GetSurroundingWalls(int gridX,int gridY, int i)
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
                    if (ChunkGen.Instance.ChunkGenerated(relPos))
                    {
                        Chunk adjacentChunk = ChunkGen.Instance.GetChunk(relPos);
                        wallCount += GetBlockType(adjacentChunk.GetBlockID(newgridX, newgridY));
                        if (calc && i == 1)
                        {
                            wallCount += (byte)((GetBlockType(adjacentChunk.GetBlockID(newgridX, newgridY)) == 1) ? 1 : 0);
                        }
                    }
                    else
                    {
                        wallCount += (byte)((rand.Next(0, 100) < randomFillPercent) ? 1 : 0);
                    }
                }
                else if (x != gridX || y != gridY)
                {
                    wallCount += GetBlockType(blocks[x, y]);
                }
            }
        }
        return wallCount;
    }
    
    /// <summary>
    /// Returns the biomes of adjacent walls so the floor biome will match the walls
    /// </summary>
    /// <param name="gridX">X position of center tile</param>
    /// <param name="gridY">Y position of center tile</param>
    /// <returns></returns>
    protected byte SurroundingWallBiomes(int gridX, int gridY)
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
    /// Refills the array after generation of empty or wall blocks with the actual type of block
    /// </summary>
    protected void DetermineBlock()
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
                if (!presetTiles.Contains(new Vector3Int(x,y,floorz))) 
                    floor[x, y] = strongFloorIndex;
                if (!presetTiles.Contains(new Vector3Int(x,y,mapz)))
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
                        Vector3 worldPos = GetTileWorldPos(x, y, -1);
                        if (worldPos.x < 10 && worldPos.x > -10 && worldPos.y < 10 && worldPos.y > -10)
                            continue;
                        emptyBlocks.Add(new Vector2Int(x, y));
                    }
                }
                else
                {
                    if (presetMonsters.ContainsKey(new Vector2Int(x, y)))
                    {
                        GenerateEnemy(x, y, presetMonsters[new Vector2Int(x, y)]);
                    }
                }
            }
        }
        if (GameManager.Instance.spawnMonsters)
        {
            for (int i = 0; i < maxMonsters && i < 5; i++)
            {
                Vector2Int pos = emptyBlocks[rand.Next(0, emptyBlocks.Count)];
                if (GetSurroundingWalls(pos.x, pos.y, 2) != 0)
                {
                    continue;
                }
                SpawnMonsters(pos.x, pos.y);
                emptyBlocks.Remove(pos);
                if (monsters.Count > 3)
                {
                    break;
                }
            }
        }
        foreach (Vector2Int emptyBlock in emptyBlocks)
        {
            DetermineEmptyType(emptyBlock.x, emptyBlock.y);
        }
    }
    /// <summary>
    /// Spawn enemy at given location
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    protected void SpawnMonsters(int x, int y)
    {
        blocks[x, y] = 127;
        if (GameManager.Instance.loadFromFile)
            return;
        byte maxByte = 0;
        float maxIndexWeight = 0;
        for (int i = 0; i < biomeScripts[biomes[x, y]].enemies.Count; i++)
        {
            MonsterInfo info = biomeScripts[biomes[x, y]].enemies[i].GetComponent<MonsterInfo>();
            float weight = Noise.Get2DPerlin(new Vector2Int(x, y), seed, info.weight);
            if (weight > maxIndexWeight)
            {
                maxByte = (byte)info.id;
                maxIndexWeight = weight;
            }
        }
        GenerateEnemy(x, y, maxByte);
    }
    /// <summary>
    /// Generates the enemy GameObject at given x and y coordinates and with given enemy ID
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="enemyID"></param>
    protected GameObject GenerateEnemy(int x, int y, byte enemyID)
    {
        GameObject enemy = GameObject.Instantiate(GameManager.Instance.GetMonsterObject(enemyID), monsterParent) as GameObject;
        enemy.transform.position = GetTileWorldPos(x, y, -1);
        enemy.GetComponent<MonsterInfo>().chunk = chunkPos;
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, monsterParent.position.z);
        monsters.Add(enemy.GetHashCode(), enemy);
        return enemy;
    }
    /// <summary>
    /// Determines empty tile type at given location
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    protected void DetermineEmptyType(int x, int y)
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
    /// Determines where special blocks generate in the chunk
    /// </summary>
    protected void SpecialBlockGeneration()
    {
        if (specialChunk)
            return;
        float heighestWeight = 0;
        int highestX = 0;
        int highestY = 0;
        for (int i = 0; i < emptyBlocks.Count; i++)
        {
            Vector2Int pos = emptyBlocks[i];
            float weight = rand.Next(0, 100);
            int walls = GetSurroundingWalls(pos.x, pos.y, 2);
            if (walls > 3 && walls < 7)
                weight *= 2;
            if (weight > heighestWeight)
            {
                highestX = pos.x;
                highestY = pos.y;
                heighestWeight = weight;
            }
        }
        int val = rand.Next(0, biomeScripts[biomes[highestX, highestY]].specialBlocks.Count);
        Blocks specialBlock = biomeScripts[biomes[highestX, highestY]].specialBlocks[val];
        if (specialBlock.blockType == Blocks.Type.Floor)
        {
            floor[highestX, highestY] = specialBlock.index;
            blocks[highestX, highestY] = 127;
        }
        else
        {
            blocks[highestX, highestY] = specialBlock.index;
            AddInteractable(highestX, highestY);
        }
    }
    /// <summary>
    /// Places the tiles related to the block array in the tilemap
    /// </summary>
    protected void DrawTileMap()
    {
        if (map == null)
        {
            CreateTileMaps();
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, mapz);
                Vector3Int floorPos = pos;
                floorPos.z = floorz;
                SetTileMapTile(pos, blocks[x, y]);
                if (blocks[x,y] == 127)
                    SetTileMapTile(floorPos, floor[x,y]);
            }
        }
        if (!GameManager.Instance.testingmode)
            UnloadChunk();
    }
    /// <summary>
    /// Changes the tile at the given position using the new block index (used during inital generation)
    /// </summary>
    /// <param name="tilePos">Chunk pos of the tile</param>
    /// <param name="index">Index of the new block</param>
    protected void SetTileMapTile(Vector3Int tilePos, byte index)
    {
        if (index == 127)
        {
            map.SetTile(tilePos, null);
        }
        else
        {
            Blocks block = GameManager.Instance.GetBlock(index);
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
    protected void CreateTileMaps()
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
    protected byte GetBlockType(byte tile)
    {
        if (tile == 127)
            return 0;
        if (GameManager.Instance.GetBlock(tile).blockType == Blocks.Type.Wall)
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
    public byte GetBlockID(int x, int y)
    {
        return blocks[x, y];
    }
    /// <summary>
    /// Returns the biome number at the coordinates
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    /// <returns></returns>
    public byte GetTileBiome(int x, int y)
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
            map.SetTile(new Vector3Int(x, y, floorz), GameManager.Instance.GetBlock(floor[x,y]).tile);
        }
        else
        {
            map.SetTile(new Vector3Int(x, y, mapz), GameManager.Instance.GetBlock(block).tile);
            if (GameManager.Instance.GetBlock(block).solid)
                UpdateTileCollider(x, y,Tile.ColliderType.Grid);
            map.SetTile(new Vector3Int(x, y, floorz), null);
        }
        if (changes.ContainsKey(new Vector2Int(x, y)))
        {
            changes[new Vector2Int(x, y)] = block;
        }
        else
        {
            changes.Add(new Vector2Int(x, y), block);
        }
        changed = true;
        map.RefreshTile(new Vector3Int(x, y, mapz));
    }
    /// <summary>
    /// Disables the renderer of the map and enemies
    /// </summary>
    public void UnloadChunk()
    {
        map.GetComponent<TilemapRenderer>().enabled = false;
        foreach(GameObject enemy in monsters.Values)
        {
            enemy.SetActive(false);
        }
        foreach(GameObject interactable in interactables)
        {
            interactable.SetActive(false);
        }
    }
    /// <summary>
    /// Activates the map renderer and enemies
    /// </summary>
    public void LoadChunk()
    {
        changed = true;
        map.GetComponent<TilemapRenderer>().enabled = true;
        foreach(GameObject enemy in monsters.Values)
        {
            enemy.SetActive(true);
        }
        foreach (GameObject interactable in interactables)
        {
            interactable.SetActive(true);
        }
    }
    /// <summary>
    /// Changes the tile at location to be a new color of the same tile to indicate it being highlighted/unhighlighted
    /// </summary>
    /// <param name="x">Chunk tile position x</param>
    /// <param name="y">Chunk tile position y</param>
    /// <param name="newTile">Tile with new color scheme</param>
    public void UpdateTileColor(int x, int y, Tile newTile)
    {
        Tile t = map.GetTile<Tile>(new Vector3Int(x, y, mapz));
        if (t == null)
            return;
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
    public void UpdateTileCollider(int x, int y, Tile.ColliderType tileCollider)
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
        if (map == null)
        {
            CreateTileMaps();
        }
        if (blocks[x, y] == 127)
            return;
        Blocks interactBlock = GameManager.Instance.GetBlock(blocks[x, y]);
        if (interactBlock.interactable)
        {
            Vector3 interactablePos = map.GetCellCenterWorld(new Vector3Int(x, y, mapz));
            interactablePos.z = mapz - 1;
            Transform interactParent = GameObject.Find("Interactables").transform;
            GameObject interactable = GameObject.Instantiate(interactBlock.gameObject.GetComponent<InteractReference>().interactable, interactParent);
            interactable.transform.position = interactablePos;
            interactables.Add(interactable);
        }
    }
    /// <summary>
    /// Returns the world position given chunk relative position
    /// </summary>
    /// <param name="x">Chunk relative x</param>
    /// <param name="y">Chunk relative y</param>
    /// <param name="z">Chunk relative z</param>
    /// <returns></returns>
    protected Vector3 GetTileWorldPos(int x, int y, int z)
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
    /// Called to kill enemy and open up spawn spot
    /// </summary>
    /// <param name="enemy">Gameobject for enemy</param>
    public void DestroyEnemy(GameObject enemy)
    {
        monsters.Remove(enemy.GetHashCode());
        GameObject.Destroy(enemy);
    }
    /// <summary>
    /// returns list of all enemies
    /// </summary>
    /// <returns></returns>
    public ValueCollection GetEnemies()
    {
        return monsters.Values;
    }
    /// <summary>
    /// Returns string array of map changes
    /// </summary>
    /// <returns></returns>
    public string[] GetChanges()
    {
        string[] blocksChanged = new string[changes.Count];
        int i = 0;
        foreach (Vector2Int change in changes.Keys)
        {
            blocksChanged[i] = change.x + " " + change.y + "|" + changes[change];
            i++;
        }
        return blocksChanged;
    }
    /// <summary>
    /// Adds block change to list
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="id"></param>
    private void AddChange(Vector2Int pos, byte id)
    {
        changes.Add(pos, id);
    }
    /// <summary>
    /// Change block in array to changed versions
    /// </summary>
    public void AddChangedBlocks()
    {
        foreach (Vector2Int pos in changes.Keys)
        {
            blocks[pos.x, pos.y] = changes[pos];
        }
    }
    /// <summary>
    /// Load chunk contents from chunk save script
    /// </summary>
    /// <param name="c"></param>
    public void LoadFromFile(ChunkSave c)
    {
        changed = false;
        string[] changes = c.GetChanges();
        for (int i = 0; i < changes.Length; i++)
        {
            string[] split = changes[i].Split('|');
            byte id = (byte)Int32.Parse(split[1]);
            string[] posSplit = split[0].Split(' ');
            int posX = Int32.Parse(posSplit[0]);
            int posY = Int32.Parse(posSplit[1]);
            Vector2Int tilePos = new Vector2Int(posX, posY);
            AddChange(tilePos, id);
        }
        DestroyMap();
        MonsterSave[] enemy = c.GetEnemies();
        for (int i = 0; i < enemy.Length; i++)
        {
            Vector3 enemyPos = enemy[i].GetPosition();
            Vector2Int localPos = ChunkGen.Instance.GetChunkTilePos(new Vector2Int((int)enemyPos.x, (int)enemyPos.y));
            GameObject enemyObj = GenerateEnemy(localPos.x, localPos.y, enemy[i].GetID());
            enemyObj.transform.eulerAngles = enemy[i].GetRotation();
        }
    }

    public int GetChunkGenNum()
    {
        return numGen;
    }
    public void DestroyMap()
    {
        if (map != null)
            GameObject.Destroy(map.gameObject);
    }
}
