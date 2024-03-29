﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using static System.Collections.Generic.Dictionary<byte, InventoryItem>;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Testing Modes:")]
    [Tooltip("Test world generation")]
    public bool testingmode = true;
    [Tooltip("Pregenerated mapsize (size x size)")] 
    public int testingsize;
    [Tooltip("Spawn enemies into the game")]
    public bool spawnMonsters;
    [Tooltip("Breaking blocks")]
    [HideInInspector] public bool blockBreaking = false;
    [Tooltip("Reference to block breaking script")]
    [HideInInspector] public BlockBreaking blockBreakingScript;
    [Tooltip("Map z position")]
    [HideInInspector] public int mapz;
    [Tooltip("Foor z position")]
    [HideInInspector] public int floorz;
    [Tooltip("Item player is currently holding")]
    [HideInInspector] public ItemSlot currentItem;
    [Tooltip("Inventory is open")]
    [HideInInspector] public bool invOpen = false;
    [Tooltip("Dictionary of all blocks")]
    private Dictionary<byte, Blocks> blocks = new Dictionary<byte, Blocks>();
    [Tooltip("Dictionary of all items")]
    private Dictionary<byte, InventoryItem> itemScripts = new Dictionary<byte, InventoryItem>();
    [Tooltip("Game is paused")]
    [HideInInspector] public bool paused = false;
    [Tooltip("All premade sections")]
    [HideInInspector] public List<PremadeSection> sections = new List<PremadeSection>();
    private Dictionary<byte, GameObject> monsters = new Dictionary<byte, GameObject>();
    [Tooltip("String of full text file for dialog")]
    [HideInInspector] public string fullText;
    [Tooltip("Load world from file")]
    [HideInInspector] public bool loadFromFile = false;
    [Tooltip("Name of world")]
    [HideInInspector] public string worldName;
    [Tooltip("Start time of world")]
    [HideInInspector] public DateTime startTime;
    [Tooltip("Hours in world")]
    [HideInInspector] public double hours;
    [Tooltip("Stall items")]
    private List<ItemSlot> stallItems = new List<ItemSlot>();
    [Tooltip("Stored inventory array")]
    private ItemSlot[,] inventory = new ItemSlot[5, 7];
    [Tooltip("Player money")]
    private int playerMoney;
    [Tooltip("Transitioning from dialog to world")]
    [HideInInspector] public bool reopen = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        base.Awake();
        GameInformation.CreateInstance<GameInformation>();
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        mapz = 0;
        floorz = 1;
        currentItem = new ItemSlot();
        foreach (GameObject block in Resources.LoadAll("Blocks"))
        {
            Blocks blockComp = block.GetComponent<Blocks>();
            blocks.Add(blockComp.index, blockComp);
        }
        foreach(GameObject item in Resources.LoadAll("Items"))
        {
            InventoryItem invItem = item.GetComponent<InventoryItem>();
            itemScripts.Add(invItem.itemID, invItem);
        }
        foreach(GameObject premade in Resources.LoadAll("PremadeSections"))
        {
            sections.Add(premade.GetComponent<PremadeSection>());
        }
        foreach(GameObject monster in Resources.LoadAll("Monsters"))
        {
            monsters.Add(monster.GetComponent<MonsterInfo>().id, monster);
        }
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
    /// Used to disable destroying blocks
    /// </summary>
    public void DisableBlockBreaking()
    {
        if (blockBreakingScript.prevmapPos != Vector3Int.zero)
            blockBreakingScript.ResetPrevious();
        blockBreakingScript.enabled = false;
    }
    /// <summary>
    /// Returns the Script related to the Item name
    /// </summary>
    /// <param name="name">Name of the item wanted</param>
    /// <returns></returns>
    public InventoryItem GetItem(string name)
    {
        foreach (InventoryItem item in itemScripts.Values)
        {
            if (item.itemName.Equals(name))
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
        return ChunkGen.Instance.GetBlock(new Vector2Int(tilePos.x, tilePos.y), currentChunk);
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
    /// <summary>
    /// Assigns given text file to the fullText property
    /// </summary>
    /// <param name="text">Text file</param>
    public void assignTextFile(TextAsset text)
    {
        fullText = text.text;
    }
    /// <summary>
    /// Sets load from file to true then loads game world scene
    /// </summary>
    public void LoadFromFile()
    {
        loadFromFile = true;
        SceneLoader.LoadScene(1);
    }
    /// <summary>
    /// Called when scene changes
    /// </summary>
    /// <param name="scene">New scene</param>
    /// <param name="mode">Load type</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        ResumeGame();
        if (!scene.name.Equals("World") && ChunkGen.Instance != null)
        {
            ChunkGen.Instance.enabled = false;
        }
    }
    /// <summary>
    /// Sets game manager values upon loading main game world
    /// </summary>
    public void SetValues()
    {
        blockBreakingScript = GameObject.Find("Grid").GetComponent<BlockBreaking>();
        ChunkGen.Instance.enabled = true;
        ChunkGen.Instance.StartUp();
        startTime = DateTime.Now;
    }
    /// <summary>
    /// Returns list of item scripts
    /// </summary>
    /// <returns>List of inventory items</returns>
    public ValueCollection GetItemScripts()
    {
        return itemScripts.Values;
    }
    /// <summary>
    /// Adds items to stallitems
    /// </summary>
    /// <param name="items"></param>
    public void AddStallItems(List<ItemSlot> items)
    {
        foreach (ItemSlot item in items)
        {
            stallItems.Add(item);
        }
    }
    /// <summary>
    /// Returns list of stall items
    /// </summary>
    /// <returns></returns>
    public List<ItemSlot> GetStallItems()
    {
        return stallItems;
    }
    /// <summary>
    /// Stores the inventory in the game manager
    /// </summary>
    /// <param name="itemSlots">Slots to store</param>
    public void StoreInventory(ItemSlot[,] itemSlots)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                inventory[i, j] = new ItemSlot();
                inventory[i, j].AddExisting(itemSlots[i, j]);
            }
        }
    }
    /// <summary>
    /// Returns the stored inventory
    /// </summary>
    /// <returns></returns>
    public ItemSlot[,] GetInventory()
    {
        return inventory;
    }
    /// <summary>
    /// Sets current amount of money
    /// </summary>
    /// <param name="money"></param>
    public void SetMoney(int money)
    {
        playerMoney = money;
    }
    /// <summary>
    /// returns current amount of money
    /// </summary>
    /// <returns></returns>
    public int GetMoney()
    {
        return playerMoney;
    }
    /// <summary>
    /// Returns monster prefab gameobjects
    /// </summary>
    /// <param name="monsterID"></param>
    /// <returns></returns>
    public GameObject GetMonsterObject(byte monsterID)
    {
        return monsters[monsterID];
    }
    public List<MonsterInfo> GetMonsters()
    {
        List<MonsterInfo> monsterList = new List<MonsterInfo>();
        foreach (GameObject monster in monsters.Values)
        {
            monsterList.Add(monster.GetComponent<MonsterInfo>());
        }
        return monsterList;
    }
}
