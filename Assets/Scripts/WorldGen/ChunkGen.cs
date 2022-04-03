using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ChunkGen : Singleton<ChunkGen>
{
    [Tooltip("Tilemap prefab")]
    public GameObject map;
    [Tooltip("Grid used for tilemaps")]
    [HideInInspector] public Transform grid;
    [Tooltip("Wall z position")]
    [HideInInspector] public int mapz;
    [Tooltip("Floor z position")]
    [HideInInspector] public int floorz;
    [Tooltip("Player position")]
    private Vector3Int pos = Vector3Int.zero;
    [Tooltip("Previous player position")]
    private Vector3Int previousPos = Vector3Int.zero;
    [Tooltip("Player chunk")]
    [HideInInspector] private Vector2Int currentChunk = Vector2Int.zero;
    [Tooltip("Dictionary containg all created chunks")]
    private Dictionary<Vector2Int, Chunk> chunks;
    [Tooltip("Array of biome scripts")]
    public Biomes[] biomes;
    [Tooltip("Player movement script")]
    [HideInInspector] public FreePlayerMove playerMovement;
    [Tooltip("Width of chunk")]
    public int chunkWidth;
    [Tooltip("Height of chunk")]
    public int chunkHeight;
    [Tooltip("World seed")]
    public int seed;
    [Tooltip("Generate a random world seed")]
    [SerializeField] private bool randomSeed;
    [Tooltip("Biome seed")]
    public int biomeseed;
    [Tooltip("Generate a random biome seed")]
    [SerializeField] public bool randomBiomeSeed;
    [Tooltip("Wall fill percent")]
    [Range(0, 100)] public int randomFillPercent;
    [Tooltip("Number of wall smooths to perform")]
    public int smooths;
    [Tooltip("Number of biome smooths to perform")]
    public int biomesmooths;
    [Tooltip("Enemy spawn chance")]
    public float monsterChance;
    [Tooltip("Maximum number of enemeis")]
    public int maxMonsters;
    [Tooltip("Enemy parent transform")]
    [HideInInspector] public Transform monsterParent;
    [Tooltip("Chance of special tile")]
    public int specialTileChance;
    private System.Random rand;
    private int generated = 0;
    public void Awake()
    {
        base.Awake();
        this.enabled = false;
        chunks = new Dictionary<Vector2Int, Chunk>();
    }
    /// <summary>
    /// Sets up world
    /// </summary>
    public void StartUp()
    {
        grid = GameObject.Find("Grid").transform;
        playerMovement = GameObject.Find("Player").GetComponent<FreePlayerMove>();
        monsterParent = GameObject.Find("Enemies").transform;
        mapz = 0;
        floorz = 1;
        chunks = new Dictionary<Vector2Int, Chunk>();
        if (GameManager.Instance.loadFromFile)
        {
            GetSeed();
            rand = new System.Random(seed);
            foreach (PremadeSection sections in GameManager.Instance.sections)
            {
                if (sections.CreateAtStart)
                {
                    PresetChunk(sections);
                }
            }
            LoadFromFile();
        }
        else
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
            if (randomSeed)
                seed = UnityEngine.Random.Range(0, int.MaxValue);
            if (randomBiomeSeed)
                biomeseed = UnityEngine.Random.Range(0, 1000000);
            rand = new System.Random(seed);
            foreach (PremadeSection sections in GameManager.Instance.sections)
            {
                if (sections.CreateAtStart)
                {
                    PresetChunk(sections);
                }
            }
            for (int x = -1; x <= 0; x++)
            {
                for (int y = -1; y <= 0; y++)
                {
                    Vector2Int chunkPos = new Vector2Int(x, y);
                    GenerateChunk(chunkPos);
                }
            }
            currentChunk = new Vector2Int(0, 0);
            if (GameManager.Instance.testingmode)
            {
                GetComponent<WorldCreationTesting>().size = GameManager.Instance.testingsize;
                GetComponent<WorldCreationTesting>().enabled = true;
            }
        }
    }
    public void Regenerate()
    {
        List<Vector2Int> keys = new List<Vector2Int>();
        foreach (Vector2Int key in chunks.Keys) 
        {
            keys.Add(key);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            chunks[keys[i]].DestroyMap();
        }
        GetComponent<WorldCreationTesting>().enabled = false;
        StartUp();
    }
    void FixedUpdate()
    {
        if (playerMovement.dir != Vector2.zero)
        {
            pos = FreePlayerMove.Instance.pos;
            if (pos != previousPos)
            {
                currentChunk = FreePlayerMove.Instance.currentChunk;
                if (!WithinBounds())
                {
                    GenerateSurroundingChunks();
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
    public void GenerateSurroundingChunks()
    {
        pos = FreePlayerMove.Instance.pos;
        currentChunk = FreePlayerMove.Instance.currentChunk;
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
        relGen = new Vector2Int(0, 0);
        GenerateDirections(relGen);
        relGen = new Vector2Int(0, rely);
        GenerateDirections(relGen);
        relGen = new Vector2Int(relx, 0);
        GenerateDirections(relGen);
        relGen = new Vector2Int(relx, rely);
        GenerateDirections(relGen);
    }
    /// <summary>
    /// Determines if adjacent chunk is already generated
    /// </summary>
    /// <param name="relGen">The relative direction of new chunk given current</param>
    public void GenerateDirections(Vector2Int relGen)
    {
        Vector2Int chunkPos = relGen + currentChunk;
        if (!ChunkGenerated(chunkPos))
            GenerateChunk(chunkPos);
    }
    /// <summary>
    /// Generates map of chunk at given chunk pos
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    public void GenerateChunk(Vector2Int chunkPos)
    {
        if (!ChunkCreated(chunkPos))
        {
            CreateChunk(chunkPos);
            chunks[chunkPos].GenerateChunk(generated);
            generated++;
        }
        else if (!ChunkGenerated(chunkPos)) 
        {
            GetChunk(chunkPos).GenerateChunk(generated);
            generated++;
        }
    }
    /// <summary>
    /// determine biome type
    /// </summary>
    /// <param name="chunkPos">chunk to determine biome of</param>
    /// <returns></returns>
    private int DetermineBiome(Vector2Int chunkPos)
    {
        float highest = 0;
        int index = 0;
        foreach (Biomes biomeScript in biomes)
        {
            float val = Noise.Get2DPerlinChunk(chunkPos, biomeseed, biomeScript.scale);
            if (val > highest)
            {
                index = biomeScript.biomeID;
                highest = val;
            }
        }
        return index;
    }
    /// <summary>
    /// Creates biome script and adds it to chunk dictionary
    /// </summary>
    /// <param name="chunkPos">biome position</param>
    /// <param name="biomeIdx">index of biome</param>
    private void GenerateBiome(Vector2Int chunkPos, int biomeIdx)
    {
        switch(biomeIdx)
        {
            default:
            case 0:
                chunks.Add(chunkPos, new CommonBiome(chunkPos));
                break;
            case 1:
                chunks.Add(chunkPos, new WaterBiome(chunkPos));
                break;
        }
    }
    /// <summary>
    /// Returns true if Chunk script has already been created
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    /// <returns></returns>
    public bool ChunkCreated(Vector2Int chunkPos)
    {
        return chunks.ContainsKey(chunkPos);
    }
    /// <summary>
    /// Creates chunk script at given position
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    public void CreateChunk(Vector2Int chunkPos)
    {
        if ((Mathf.Abs(chunkPos.x) % 2 == 0 && Mathf.Abs(chunkPos.y) % 2 == 0) || (Mathf.Abs(chunkPos.x) % 2 == 1 && Mathf.Abs(chunkPos.y % 2) == 1))
        {
            int id = DetermineBiome(chunkPos);
            GenerateBiome(chunkPos, id);
        }
        else
        {
            chunks.Add(chunkPos, new MixedBiome(chunkPos));
        }
    }
    /// <summary>
    /// Returns true if Chunk has already been generated
    /// </summary>
    /// <param name="chunkPos"></param>
    /// <returns></returns>
    public bool ChunkGenerated(Vector2Int chunkPos)
    {
        if (chunks.ContainsKey(chunkPos))
        {
            return chunks[chunkPos].generated;
        }
        return false;
    }
    /// <summary>
    /// Returns chunk script for chunk at given position
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    /// <returns></returns>
    public Chunk GetChunk(Vector2Int chunkPos)
    {
        if (!ChunkCreated(chunkPos))
            return null;
        return chunks[chunkPos];
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
            return GetChunk(chunkPos).GetBlockID(chunkTilePos.x, chunkTilePos.y);
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
    /// Unloads chunk at given position
    /// </summary>
    /// <param name="chunkPos">Chunk position</param>
    public void UnloadChunk(Vector3 chunkPos)
    {
        if (!GameManager.Instance.testingmode)
        {
            Vector2Int newChunkPos = new Vector2Int((int)chunkPos.x / chunkWidth, (int)chunkPos.y / chunkHeight);
            if (newChunkPos != currentChunk)
                GetChunk(newChunkPos).UnloadChunk();
        }
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
    public void UpdateTileColor(Vector2Int tilePos, Tile newTile, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            GetChunk(chunkPos).UpdateTileColor(tilePos.x, tilePos.y, newTile);
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
    public void UpdateTileCollider(Vector3Int tilePos, Tile.ColliderType tileCollider, Vector2Int chunkPos)
    {
        if (ChunkGenerated(chunkPos))
        {
            GetChunk(chunkPos).UpdateTileCollider(tilePos.x, tilePos.y,tileCollider);
        }
    }
    /// <summary>
    /// adds preset section
    /// </summary>
    /// <param name="startPos">start position of section</param>
    /// <param name="section">script holding preset section information</param>
    void PresetChunk(PremadeSection section)
    {
        Vector2Int startPos = Vector2Int.zero;
        startPos.x = rand.Next(section.minStart.x, section.maxStart.x);
        startPos.y = rand.Next(section.minStart.y, section.maxStart.y);
        if (section.entireChunk)
        {
            GenerateBiome(startPos, section.biome);
            GetChunk(startPos).specialChunk = true;
        }
        PresetTiles(startPos, section.wallMap, 0, section.entireChunk);
        PresetTiles(startPos, section.floorMap, 1, section.entireChunk);
        PresetEnemies(startPos, section.enemies, section.entireChunk);
    }
    /// <summary>
    /// Adds preset sections to map
    /// </summary>
    /// <param name="startPos">Start pos to add object</param>
    /// <param name="map">TextAsset of map</param>
    /// <param name="z">Z position of map</param>
    void PresetTiles(Vector2Int startPos, TextAsset map, int z, bool entireChunk)
    {
        string textmap = map.text;
        string[] lines = textmap.Split('\n');
        for (int col = 0; col < lines.Length; col++)
        {
            string[] bytes = lines[col].Split('|');
            for (int row = 0; row < bytes.Length; row++)
            {
                byte blockID = Convert.ToByte(bytes[row]);
                Vector2Int chunkPos;
                Vector2Int chunkTilePos;
                if (!entireChunk)
                {
                    Vector2Int newPos = startPos + new Vector2Int(lines.Length - col - 1, bytes.Length - row - 1);
                    chunkPos = GetChunkPos(newPos);
                    chunkTilePos = GetChunkTilePos(newPos);
                }
                else
                {
                    chunkPos = startPos;
                    chunkTilePos = new Vector2Int(lines.Length - col - 1, bytes.Length - row - 1);
                }
                if (!ChunkCreated(chunkPos))
                    CreateChunk(chunkPos);
                GetChunk(chunkPos).AddPresetTile(new Vector3Int(chunkTilePos.x, chunkTilePos.y, z), blockID);
            }
        }
    }
    /// <summary>
    /// Adds preset enemies to map
    /// </summary>
    /// <param name="startPos">Start pos to add object</param>
    /// <param name="map">TextAsset containing enemies</param>
    void PresetEnemies(Vector2Int startPos, TextAsset map, bool entireChunk)
    {
        string text = map.text;
        string[] lines = text.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] lineData = lines[i].Split('|');
            byte enemy = Convert.ToByte(lineData[1]);
            string[] pos = lineData[0].Split(' ');
            Vector2Int chunkPos;
            Vector2Int chunkTilePos;
            if (!entireChunk)
            {
                Vector2Int newPos = startPos + new Vector2Int(Int32.Parse(pos[0]), Int32.Parse(pos[1]));
                chunkPos = GetChunkPos(newPos);
                chunkTilePos = GetChunkTilePos(newPos);
            }
            else
            {
                chunkPos = startPos;
                chunkTilePos = new Vector2Int(Int32.Parse(pos[0]), Int32.Parse(pos[1]));
            }
            if (!ChunkCreated(chunkPos))
                CreateChunk(chunkPos);
            GetChunk(chunkPos).AddPresetEnemy(chunkTilePos, enemy);
        }
    }
    /// <summary>
    /// Return dictionary of chunks
    /// </summary>
    /// <returns></returns>
    public Dictionary<Vector2Int, Chunk> GetChunks()
    {
        return chunks;
    }
    /// <summary>
    /// loads world from file
    /// </summary>
    void LoadFromFile()
    {
        WorldInfo w = GameInformation.Instance.LoadGameInfo();
        List<ChunkSave> cs = GameInformation.Instance.LoadWorld();
        foreach (ChunkSave c in cs)
        {
            Vector2Int chunkPos = c.GetChunkPos();
            if (!ChunkCreated(chunkPos))
                CreateChunk(chunkPos);
            GetChunk(chunkPos).LoadFromFile(c);
            GetChunk(chunkPos).GenerateChunk(generated);
            generated++;
        }
    }
    private void GetSeed()
    {
        WorldInfo w = GameInformation.Instance.LoadGameInfo();
        List<ChunkSave> cs = GameInformation.Instance.LoadWorld();
        seed = w.GetWorldSeed();
        biomeseed = w.GetBiomeSeed();
    }
    /// <summary>
    /// Clear the chunks dictionary when script is disabled
    /// </summary>
    private void OnDisable()
    {
        if (chunks != null)
            chunks.Clear();
    }
}
