﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using System;

public class ChunkGen : MonoBehaviour
{
    public static ChunkGen currentWorld;
    public GameManager manager;
    public GameObject map;
    public Transform grid;
    [HideInInspector]
    public int mapz;
    [HideInInspector]
    public int floorz;
    Vector3Int pos = Vector3Int.zero;
    Vector3Int previousPos = Vector3Int.zero;
    public Vector2Int currentChunk = Vector2Int.zero;
    int currentHash;
    Hashtable chunks = new Hashtable();
    public Biomes[] biomes;
    public FreePlayerMove playerMovement;
    public int chunkWidth;
    public int chunkHeight;
    public int seed;
    public bool randomSeed;
    public int biomeseed;
    public bool randomBiomeSeed;
    [Range(0, 100)]
    public int randomFillPercent;
    [Range(0, 100)]
    public int randomBiomePercent;
    public int smooths;
    public int biomesmooths;
    public float enemyChance;
    public int maxenemies;
    public Transform enemyParent;
    public int specialTileChance;
    [HideInInspector]
    void Awake()
    {
        mapz = 0;
        floorz = 1;
        currentWorld = this;
        if (randomSeed)
            seed = UnityEngine.Random.Range(0, int.MaxValue);
        if (randomBiomeSeed)
            biomeseed = UnityEngine.Random.Range(0, 1000000);
        PresetTiles(new Vector2Int(0, 0), manager.sections[0]);
        foreach(PremadeSection sections in manager.sections)
        {
            if (sections.CreatAtStart)
            {
                int startX = UnityEngine.Random.Range(sections.minStart.x, sections.maxStart.y);
                int startY = UnityEngine.Random.Range(sections.minStart.y, sections.maxStart.y);
                PresetTiles(new Vector2Int(startX, startY), sections);
            }
        }
        for (int x = -1; x <= 0; x++)
        {
            for (int y = -1; y <= 0; y++)
            {
                Vector2Int chunkPos = new Vector2Int(x, y);
                GenerateNewChunk(chunkPos);
            }
        }
        currentChunk = new Vector2Int(0, 0);
        currentHash = currentChunk.ToString().GetHashCode();
    }
    void FixedUpdate()
    {
        if (playerMovement.dir != Vector2.zero)
        {
            pos = manager.pos;
            if (pos != previousPos)
            {
                currentChunk = manager.currentChunk;
                if (OutsideChunk(pos))
                {
                    currentHash = currentChunk.ToString().GetHashCode();
                }
                if (!WithinBounds())
                {
                    GenerateNewChunks();
                }
                previousPos = pos;
            }
        }
    }
    /// <summary>
    /// Determines if the view bounds at the current position extend beyond the current chunk
    /// </summary>
    /// <returns></returns>
    bool WithinBounds()
    {
        if (pos.x >= chunkWidth-12|| pos.x <= 12 || pos.y >= chunkHeight-10 || pos.y <= 10)
            return false;
        return true;
    }
    /// <summary>
    /// Generates new chunks in different directions
    /// </summary>
    void GenerateNewChunks()
    {
        Vector2Int relGen;
        int relx = 0;
        int rely = 0;
        if (pos.x >= chunkWidth-12)
            relx = 1;
        else if (pos.x <= 12)
            relx = -1;
        if (pos.y >= chunkHeight-10)
            rely = 1;
        else if (pos.y <= 10)
            rely = -1;
        relGen = new Vector2Int(0, rely);
        GenDirections(relGen);
        relGen = new Vector2Int(relx, 0);
        GenDirections(relGen);
        relGen = new Vector2Int(relx, rely);
        GenDirections(relGen);
    }
    /// <summary>
    /// Determines if adjacent chunk is already generated
    /// </summary>
    /// <param name="relGen">The relative direction of new chunk given current</param>
    public void GenDirections(Vector2Int relGen)
    {
        Vector2Int chunkPos = relGen + currentChunk;
        if (!ChunkGenerated(chunkPos))
            GenerateNewChunk(chunkPos);
    }
    /// <summary>
    /// Generates map of chunk at given chunk pos
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    public void GenerateNewChunk(Vector2Int chunkPos)
    {
        if (!ChunkCreated(chunkPos))
        {
            int hash = chunkPos.ToString().GetHashCode();
            chunks.Add(hash, new Chunk(chunkPos));
            ((Chunk)chunks[hash]).GenerateChunk();
        }
        else
        {
            GetChunk(chunkPos).GenerateChunk();
        }
    }
    /// <summary>
    /// Returns true if Chunk script has already been created
    /// </summary>
    /// <param name="chunkRelPos">Chunk position</param>
    /// <returns></returns>
    public bool ChunkCreated(Vector2Int chunkRelPos)
    {
        int hash = chunkRelPos.ToString().GetHashCode();
        return chunks.Contains(hash);
    }
    /// <summary>
    /// Creates chunk script at given position
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    public void CreateChunk(Vector2Int chunkPos)
    {
        int hash = chunkPos.ToString().GetHashCode();
        chunks.Add(hash, new Chunk(chunkPos));
    }
    /// <summary>
    /// Returns true if Chunk has already been generated
    /// </summary>
    /// <param name="chunkRelPos"></param>
    /// <returns></returns>
    public bool ChunkGenerated(Vector2Int chunkRelPos)
    {
        int hash = chunkRelPos.ToString().GetHashCode();
        if (chunks.Contains(hash))
        {
            return ((Chunk)chunks[hash]).generated;
        }
        return false;
    }
    /// <summary>
    /// Returns chunk script for chunk at given position
    /// </summary>
    /// <param name="chunkRelPos">Chunk position</param>
    /// <returns></returns>
    public Chunk GetChunk(Vector2Int chunkRelPos)
    {
        int hash = chunkRelPos.ToString().GetHashCode();
        return (Chunk)chunks[hash];
    }
    /// <summary>
    /// Determines the Vector2 Chunk for the given position
    /// </summary>
    /// <param name="tilePos">World position</param>
    /// <returns></returns>
    public Vector2Int GetChunkPos(Vector2Int tilePos)
    {
        Vector2Int chunkPos = Vector2Int.zero;
        chunkPos.x = (tilePos.x < 0) ? (tilePos.x - chunkWidth) / chunkWidth : tilePos.x / chunkWidth;
        chunkPos.y = (tilePos.y < 0) ? (tilePos.y - chunkHeight) / chunkHeight : tilePos.y / chunkHeight;
        return chunkPos;
    }
    /// <summary>
    /// Determines the position relative to its chunk
    /// </summary>
    /// <param name="tilePos">World position</param>
    /// <returns></returns>
    public Vector2Int GetChunkTilePos(Vector2Int tilePos)
    {
        Vector2Int localTilePos = new Vector2Int(0, 0);
        if (tilePos.x < 0)
            localTilePos.x = tilePos.x % chunkWidth + chunkWidth;
        else
            localTilePos.x = tilePos.x % chunkWidth;
        if (tilePos.y < 0)
            localTilePos.y = tilePos.y % chunkHeight + chunkHeight;
        else
            localTilePos.y = tilePos.y % chunkHeight;
        return localTilePos;
    }
    /// <summary>
    /// Gets block ID at given position
    /// </summary>
    /// <param name="tilePos">World position</param>
    /// <param name="chunkPos">Chunk position</param>
    /// <returns></returns>
    public byte GetBlock(Vector2Int tilePos, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            Vector2Int chunkTilePos = GetChunkTilePos(tilePos);
            return GetChunk(chunkPos).GetBlock(chunkTilePos.x, chunkTilePos.y);
        }
        return 0;
    }
    /// <summary>
    /// Changes byte given chunk tile position and chunk pos
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="tile">Tile ID</param>
    /// <param name="chunkPos">Chunk position</param>
    public void UpdateByte(Vector2Int tilePos, byte tile, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            GetChunk(chunkPos).UpdateByte(tilePos.x, tilePos.y,tile);
        }
    }
    /// <summary>
    /// Returns true if player has gone outside of current chunk
    /// </summary>
    /// <param name="playerPos">World position</param>
    /// <returns></returns>
    public bool OutsideChunk(Vector3 playerPos)
    {
        return (playerPos.x < currentChunk.x * chunkWidth || playerPos.x > currentChunk.x * chunkWidth + chunkWidth || playerPos.y < currentChunk.y * chunkHeight || playerPos.y > currentChunk.y * chunkHeight + chunkHeight);
    }
    /// <summary>
    /// Unloads chunk at given position
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    public void UnloadChunk(Vector3 chunkPos)
    {
        GetChunk(new Vector2Int((int)chunkPos.x / chunkWidth, (int)chunkPos.y / chunkHeight)).UnloadChunk();
    }
    /// <summary>
    /// Loads chunk at given position
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    public void LoadChunk(Vector3 chunkPos)
    {
        GetChunk(new Vector2Int((int)chunkPos.x / chunkWidth, (int)chunkPos.y / chunkHeight)).LoadChunk();
    }
    /// <summary>
    /// Updates the color of the tile at given chunk tile position and chunk position
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="newTile">Tile with updated colors</param>
    /// <param name="chunkPos">Chunk position</param>
    public void UpdateColor(Vector2Int tilePos, Tile newTile, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            GetChunk(chunkPos).UpdateColor(tilePos.x, tilePos.y, newTile);
        }
    }
    /// <summary>
    /// Returns the tile at the given chunk tile position and chunk position
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="chunkPos">Chunk position</param>
    /// <returns></returns>
    public Tile GetTile(Vector3Int tilePos, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            return GetChunk(chunkPos).GetTile(new Vector3Int(tilePos.x, tilePos.y, tilePos.z));
        }
        return null;
    }
    /// <summary>
    /// Updates the collider for the given chunk tile position in the given chunk
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="tileCollider">New tile collider</param>
    /// <param name="chunkPos">Chunk position</param>
    public void UpdateCollider(Vector3Int tilePos, Tile.ColliderType tileCollider, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            GetChunk(chunkPos).UpdateCollider(tilePos.x, tilePos.y,tileCollider);
        }
    }
    /// <summary>
    /// Interacts with tile at given location in given chunk
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="chunkPos">Chunk position</param>
    public void Interact(Vector3Int tilePos,Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
            GetChunk(chunkPos).Interact(new Vector2Int(tilePos.x, tilePos.y));
    }
    void PresetTiles(Vector2Int startPos, PremadeSection section)
    {
        if (section.textmap != null)
            PresetMap(startPos, section.textmap, 0);
        if (section.floormap != null)
            PresetMap(startPos, section.floormap, 1);
    }
    void PresetMap(Vector2Int startPos, TextAsset textmap, int z)
    {
        string[] rows = textmap.text.Split('\n');
        for (int r = 0; r < rows.Length; r++)
        {
            string[] columns = rows[r].Split('|');
            for (int c = 0; c < columns.Length; c++)
            {
                if (Char.IsLetter(columns[c][0]))
                    continue;
                int relX = c - columns.Length / 2;
                int relY = rows.Length / 2-r;
                Vector2Int newPos = startPos + new Vector2Int(relX, relY);
                Vector2Int chunkPos = GetChunkPos(newPos);
                Vector2Int chunkTilePos = GetChunkTilePos(newPos);
                if (!ChunkCreated(chunkPos))
                    CreateChunk(chunkPos);
                GetChunk(chunkPos).AddPreset(new Vector3Int(chunkTilePos.x, chunkTilePos.y, z), (byte)Convert.ToInt32(columns[c]));
            }
        }
    }
}
