﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager currentManager;
    [Tooltip("Eliminate lone tiles")] public bool revisiting = false;

    [Header("Modes:")]
    public bool testingmode = true;
    [Tooltip("Pregenerated mapsize (size x size)")] public int testingsize;
    public bool spawnEnemies;

    [Header("Abilities:")]
    [HideInInspector]
    public bool blockplacing = false;
    [HideInInspector]
    public bool placing = false;
    [HideInInspector]
    public byte currentTileID;
    [HideInInspector]
    public Vector3Int pos = Vector3Int.zero;
    [Header("Script Refs:")]
    public DestroyandPlace destroyandPlace;
    Sprite[] sprites;
    Sprite[] posts;
    List<string> spriteNames = new List<string>();
    [HideInInspector] public int mapz;
    [HideInInspector] public int floorz;
    [HideInInspector] public List<InventoryItem> items = new List<InventoryItem>();
    [HideInInspector] public Inventory inv;
    [HideInInspector] public bool fighting = false;
    [HideInInspector] public ItemReference currentItem;
    [HideInInspector] public bool invOpen = false;
    [HideInInspector] public List<Tile[]> biomeBlocks = new List<Tile[]>();
    public GameObject character;
    public GameObject invObject;
    [HideInInspector] public List<Vector3Int> markets = new List<Vector3Int>();
    [HideInInspector] public List<Vendor> vendors = new List<Vendor>();
    Dictionary<byte, Blocks> blocks = new Dictionary<byte, Blocks>();
    Dictionary<byte, InventoryItem> itemScripts = new Dictionary<byte, InventoryItem>();
    [HideInInspector] public Vector2Int currentChunk = Vector2Int.zero;
    public int gold = 0;
    [HideInInspector] public bool paused = false;
    [HideInInspector] public List<PremadeSection> sections = new List<PremadeSection>();
    [HideInInspector] public string fullText;
    public TextAsset[] textFiles;
    [HideInInspector] public bool loadFromFile = false;
    GameInformation gameInfo;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mapz = 0;
        floorz = 1;
        currentManager = this;
        currentItem = new ItemReference();
        foreach (GameObject block in Resources.LoadAll("Blocks"))
        {
            InventoryItem item;
            if (block.TryGetComponent<InventoryItem>(out item))
            {
                item.durability = item.baseDurability;
                item.currentStack = 1;
                itemScripts.Add(item.itemID, item);
            }
            Blocks blockComp = block.GetComponent<Blocks>();
            blocks.Add(blockComp.index, blockComp);
        }
        foreach(GameObject item in Resources.LoadAll("Items"))
        {
            InventoryItem invItem = item.GetComponent<InventoryItem>();
            invItem.durability = invItem.baseDurability;
            invItem.currentStack = 1;
            items.Add(invItem);
            itemScripts.Add(invItem.itemID, invItem);
        }
        foreach(GameObject item in Resources.LoadAll("PremadeSections"))
        {
            sections.Add(item.GetComponent<PremadeSection>());
        }
        sprites = Resources.LoadAll<Sprite>("Images");
        posts = Resources.LoadAll<Sprite>("Images/Posts");
        foreach(Sprite sprite in sprites)
        {
            spriteNames.Add(sprite.name);
        }
        foreach(Sprite post in posts)
        {
            spriteNames.Add(post.name);
        }
        if (testingmode)
        {
            GetComponent<WorldCreationTesting>().enabled = true;
            GetComponent<WorldCreationTesting>().size = testingsize;
        }
        //GetTile("Post").sprite = Resources.Load<Sprite>("Images/Post");
    }
    /// <summary>
    /// Returns the tile related to the given ID
    /// </summary>
    /// <param name="tileID"> Block ID</param>
    /// <returns></returns>
    public Tile GetTile(byte tileID)
    {
        return blocks[tileID].tile;
    }
    /// <summary>
    /// Resets the tile to what it previously was before highlighted
    /// </summary>
    public void ResetPreviousTile()
    {
        if (destroyandPlace.prevmapPos != Vector3Int.zero)
            destroyandPlace.ResetPrevious();
        destroyandPlace.enabled = false;
    }
    /// <summary>
    /// Returns the Script related to the Item name
    /// </summary>
    /// <param name="name">Name of the item wanted</param>
    /// <returns></returns>
    public InventoryItem GetItem(string name)
    {
        foreach (InventoryItem item in items)
        {
            if (item.name.Equals(name))
                return item;
        }
        return null;
    }
    /// <summary>
    /// Returns the script related to the Item ID
    /// </summary>
    /// <param name="ID">ID of the item wanted</param>
    /// <returns></returns>
    public InventoryItem GetItem(byte ID)
    {
        return itemScripts[ID];
    }
    /// <summary>
    /// Determines if the block can be broken
    /// </summary>
    /// <param name="pos">World position</param>
    /// <param name="currentChunk">Chunk position</param>
    /// <returns></returns>
    public bool IsBreakable(Vector3Int pos, Vector2Int currentChunk)
    {
        return blocks[GetByte(pos, currentChunk)].breakable;
    }
    /// <summary>
    /// Returns the block script related to the ID
    /// </summary>
    /// <param name="ID">ID of the desired block</param>
    /// <returns></returns>
    public Blocks GetBlock(byte ID)
    {
        if (BlockExists(ID))
            return blocks[ID];
        return null;
    }
    /// <summary>
    /// Returns the block ID of the block at the given location
    /// </summary>
    /// <param name="tilePos">World position</param>
    /// <param name="currentChunk">Chunk position</param>
    /// <returns></returns>
    public byte GetByte(Vector3Int tilePos, Vector2Int currentChunk)
    {
        return ChunkGen.currentWorld.GetBlock(new Vector2Int(tilePos.x, tilePos.y), currentChunk);
    }
    /// <summary>
    /// Returns true if there is a block with given ID
    /// </summary>
    /// <param name="ID">ID of block</param>
    /// <returns></returns>
    public bool BlockExists(byte ID)
    {
        return blocks.ContainsKey(ID);
    }
    /// <summary>
    /// Pauses the game and turns off Updates
    /// </summary>
    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
    }
    /// <summary>
    /// Resumes the game and turns on Updates
    /// </summary>
    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
    }
    public void BuildNavMesh()
    {
        foreach(NavMeshSurface2d surface in NavMeshSurface2d.activeSurfaces)
        {
            surface.BuildNavMesh();
        }
    }
    public void assignTextFile(string textName)
    {
        foreach(TextAsset text in textFiles)
        {
            if (text.name.Equals(textName))
            {
                fullText = text.text;
            }
        }
    }
    public void loadWorld(GameInformation info)
    {
        loadFromFile = true;
        gameInfo = info;
    }
    public GameInformation GetGameInformation()
    {
        return gameInfo;
    }
}
