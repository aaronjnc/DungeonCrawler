using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
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
    public NavMeshSurface2d surface;
    public int maxenemies;
    public Transform enemyParent;
    [HideInInspector]
    void Awake()
    {
        mapz = 0;
        floorz = 1;
        currentWorld = this;
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
        PresetTile(new Vector2Int(1, 1), 7);
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
    bool WithinBounds()
    {
        if (pos.x >= chunkWidth-12|| pos.x <= 12 || pos.y >= chunkHeight-10 || pos.y <= 10)
            return false;
        return true;
    }
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
    public Vector2Int GetChunkPos(Vector2Int tilePos)
    {
        Vector2Int chunkPos = Vector2Int.zero;
        chunkPos.x = (tilePos.x < 0) ? (tilePos.x - chunkWidth) / chunkWidth : tilePos.x / chunkWidth;
        chunkPos.y = (tilePos.y < 0) ? (tilePos.y - chunkHeight) / chunkHeight : tilePos.y / chunkHeight;
        return chunkPos;
    }
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
    public byte GetBlock(Vector2Int tilePos, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            Vector2Int chunkTilePos = GetChunkTilePos(tilePos);
            return GetChunk(chunkPos).GetBlock(chunkTilePos.x, chunkTilePos.y);
        }
        return 0;
    }
    public void UpdateByte(Vector2Int tilePos, byte tile, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            GetChunk(chunkPos).UpdateByte(tilePos.x, tilePos.y,tile);
        }
    }
    public bool OutsideChunk(Vector3 playerPos)
    {
        return (playerPos.x < currentChunk.x * chunkWidth || playerPos.x > currentChunk.x * chunkWidth + chunkWidth || playerPos.y < currentChunk.y * chunkHeight || playerPos.y > currentChunk.y * chunkHeight + chunkHeight);
    }
    public Tilemap GetMap(Vector2Int tilePos)
    {
        return GetChunk(tilePos).map;
    }
    public Tilemap GetMap(Vector3 playerPos)
    {
        return GetChunk(GetChunkPos(new Vector2Int((int)playerPos.x,(int)playerPos.y))).map;
    }
    public void UnloadChunk(Vector3 chunkPos)
    {
        GetChunk(new Vector2Int((int)chunkPos.x / chunkWidth, (int)chunkPos.y / chunkHeight)).UnloadChunk();
    }
    public void LoadChunk(Vector3 chunkPos)
    {
        GetChunk(new Vector2Int((int)chunkPos.x / chunkWidth, (int)chunkPos.y / chunkHeight)).LoadChunk();
    }
    public void UpdateColor(Vector2Int tilePos, Tile newTile, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            GetChunk(chunkPos).UpdateColor(tilePos.x, tilePos.y, newTile);
        }
    }
    public Tile GetTile(Vector3Int tilePos, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            return GetChunk(chunkPos).GetTile(new Vector3Int(tilePos.x, tilePos.y, tilePos.z));
        }
        return null;
    }
    public void UpdateCollider(Vector3Int tilePos, Tile.ColliderType tileCollider, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            GetChunk(chunkPos).UpdateCollider(tilePos.x, tilePos.y,tileCollider);
        }
    }
    public void Interact(Vector3Int tilePos,Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
            GetChunk(chunkPos).Interact(new Vector2Int(tilePos.x, tilePos.y));
    }
}
