using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkGen : MonoBehaviour
{
    public static ChunkGen currentWorld;
    public GameManager manager;
    [HideInInspector]
    public Tilemap map;
    [HideInInspector]
    public Tilemap floor;
    [HideInInspector]
    public int mapz;
    [HideInInspector]
    public int floorz;
    Vector3Int pos = Vector3Int.zero;
    Vector3Int previousPos = Vector3Int.zero;
    Vector2Int currentChunk = Vector2Int.zero;
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
    void Awake()
    {
        currentWorld = this;
        map = manager.map;
        floor = manager.floor;
        mapz = manager.mapz;
        floorz = manager.floorz;
        if (randomSeed)
            seed = Random.Range(0, int.MaxValue);
        if (randomBiomeSeed)
            biomeseed = Random.Range(0, 1000000);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <=1;y++)
            {
                PresetTile(new Vector2Int(x, y), 127);
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
    }
    void FixedUpdate()
    {
        if (playerMovement.dir != Vector2.zero)
        {
            pos = manager.pos;
            if (pos != previousPos)
            {
                currentChunk = GetChunkPos(new Vector2Int(pos.x,pos.y));
                currentHash = currentChunk.ToString().GetHashCode();
                if (!WithinBounds())
                    GenerateNewChunks();
                previousPos = pos;
            }
        }
    }
    bool WithinBounds()
    {
        Vector2Int topRight = ((Chunk)chunks[currentHash]).topRight();
        Vector2Int bottomLeft = ((Chunk)chunks[currentHash]).bottomLeft();
        Vector2Int relGen = new Vector2Int(0, 0);
        if (pos.x + 10 > topRight.x || pos.x - 10 < bottomLeft.x || pos.y + 10 > topRight.y || pos.y - 10 < bottomLeft.y)
            return false;
        return true;
    }
    void GenerateNewChunks()
    {
        Vector2Int topRight = ((Chunk)chunks[currentHash]).topRight();
        Vector2Int bottomLeft = ((Chunk)chunks[currentHash]).bottomLeft();
        Vector2Int relGen = new Vector2Int(0, 0);
        int relx = 0;
        int rely = 0;
        if (pos.x + 10 > topRight.x)
            relx = 1;
        else if (pos.x - 10 < bottomLeft.x)
            relx = -1;
        if (pos.y + 10 > topRight.y)
            rely = 1;
        else if (pos.y - 10 < bottomLeft.y)
            rely = -1;
        relGen = new Vector2Int(0, rely);
        GenDirections(relGen);
        relGen = new Vector2Int(relx, 0);
        GenDirections(relGen);
        relGen = new Vector2Int(relx, rely);
        GenDirections(relGen);
    }
    public void GenDirections(Vector2Int relGen)
    {
        Vector2Int chunkPos = relGen + currentChunk;
        if (!ChunkGenerated(chunkPos))
            GenerateNewChunk(chunkPos);
    }
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
    public bool ChunkCreated(Vector2Int chunkRelPos)
    {
        int hash = chunkRelPos.ToString().GetHashCode();
        return chunks.Contains(hash);
    }
    public bool ChunkGenerated(Vector2Int chunkRelPos)
    {
        int hash = chunkRelPos.ToString().GetHashCode();
        if (chunks.Contains(hash))
        {
            return ((Chunk)chunks[hash]).generated;
        }
        return false;
    }
    public Chunk GetChunk(Vector2Int chunkRelPos)
    {
        int hash = chunkRelPos.ToString().GetHashCode();
        return (Chunk)chunks[hash];
    }
    public void PresetTile(Vector2Int tilePos, byte tile)
    {
        Vector2Int chunkPos = GetChunkPos(tilePos);
        Vector2Int chunkTilePos = GetChunkTilePos(tilePos);
        if (ChunkCreated(chunkPos))
        {
            GetChunk(chunkPos).AddPreset(chunkTilePos, tile);
        }
        else
        {
            int hash = chunkPos.ToString().GetHashCode();
            chunks.Add(hash, new Chunk(chunkPos));
            ((Chunk)chunks[hash]).AddPreset(chunkTilePos, tile);
        }
    }
    Vector2Int GetChunkPos(Vector2Int tilePos)
    {
        Vector2Int chunkPos = Vector2Int.zero;
        chunkPos.x = (tilePos.x < 0) ? (tilePos.x - chunkWidth) / chunkWidth : tilePos.x / chunkWidth;
        chunkPos.y = (tilePos.y < 0) ? (tilePos.y - chunkHeight) / chunkHeight : tilePos.y / chunkHeight;
        return chunkPos;
    }
    Vector2Int GetChunkTilePos(Vector2Int tilePos)
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
    public byte GetBlock(Vector2Int tilePos)
    {
        Vector2Int chunkPos = GetChunkPos(tilePos);
        if (ChunkGenerated(chunkPos))
        {
            Vector2Int chunkTilePos = GetChunkTilePos(tilePos);
            return GetChunk(chunkPos).GetBlock(chunkTilePos.x, chunkTilePos.y);
        }
        return 0;
    }
    public void UpdateByte(Vector2Int tilePos, byte tile)
    {
        Vector2Int chunkPos = GetChunkPos(tilePos);
        if (ChunkGenerated(chunkPos))
        {
            Vector2Int chunkTilePos = GetChunkTilePos(tilePos);
            GetChunk(chunkPos).UpdateByte(chunkTilePos.x, chunkTilePos.y, tile);
        }
    }
}
