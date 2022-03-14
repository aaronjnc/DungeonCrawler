using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameInformation : ScriptableObject
{
    [Tooltip("Name of chunks folder")]
    private const string chunks = "world";
    [Tooltip("string representing main folder for save")]
    private string saveLocation;
    [Tooltip("string representing chunk save location")]
    private string chunkLocation;
    [Tooltip("Binary formatter used for serialization")]
    private BinaryFormatter formatter;
    [Tooltip("instance of GameInformation")]
    private static GameInformation _instance;
    public static GameInformation Instance
    {
        get
        {
            return _instance;
        }
    }
    private void OnEnable()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    /// <summary>
    /// Sets save location with given string
    /// </summary>
    /// <param name="location"></param>
    public void SetLocation(string location)
    {
        formatter = new BinaryFormatter();
        saveLocation = location;
        chunkLocation = Path.Combine(location, chunks);
        if (!Directory.Exists(chunkLocation))
        {
            Directory.CreateDirectory(chunkLocation);
        }
    }
    /// <summary>
    /// Saves all world data
    /// </summary>
    /// <param name="manager"></param>
    public void SaveAll()
    {
        SaveGameInfo();
        SaveWorld();
        GameObject player = GameObject.Find("Player");
        SavePlayer(player);
        SaveInventory(GameManager.Instance.inv);
    }
    /// <summary>
    /// Saves information contained within gamemanager
    /// </summary>
    /// <param name="manager"></param>
    public void SaveGameInfo()
    {
        string worldInfo = Path.Combine(saveLocation, "worldInfo.txt");
        WorldInfo info = new WorldInfo();
        FileStream fs = new FileStream(worldInfo, FileMode.Create);
        formatter.Serialize(fs, info);
        fs.Close();
    }
    /// <summary>
    /// Saves chunk information
    /// </summary>
    /// <param name="gen"></param>
    public void SaveWorld()
    {
        Hashtable chunks = ChunkGen.Instance.GetChunks();
        foreach (Chunk chunk in chunks.Values)
        {
            if (chunk.changed)
            {
                string chunkName = chunk.chunkPos.x + "_" + chunk.chunkPos.y + ".txt";
                string chunkPath = Path.Combine(chunkLocation, chunkName);
                FileStream fs = new FileStream(chunkPath, FileMode.Create);
                ChunkSave c = new ChunkSave(chunk);
                formatter.Serialize(fs, c);
                fs.Close();
            }
        }
    }
    /// <summary>
    /// Saves player information
    /// </summary>
    /// <param name="player"></param>
    public void SavePlayer(GameObject player)
    {
        string playerInfo = Path.Combine(saveLocation, "player.txt");
        FileStream fs = new FileStream(playerInfo, FileMode.Create);
        PlayerSave p = new PlayerSave(player);
        formatter.Serialize(fs, p);
        fs.Close();
    }
    /// <summary>
    /// saves inventory
    /// </summary>
    /// <param name="inv"></param>
    public void SaveInventory(Inventory inv)
    {
        string inventoryInfo = Path.Combine(saveLocation, "inventory.txt");
        FileStream fs = new FileStream(inventoryInfo, FileMode.Create);
        InventorySave i = new InventorySave(inv);
        formatter.Serialize(fs, i);
        fs.Close();
    }
    /// <summary>
    /// Loads inventory
    /// </summary>
    /// <returns></returns>
    public InventorySave LoadInventory()
    {
        string inventoryInfo = Path.Combine(saveLocation, "inventory.txt");
        FileStream fs = new FileStream(inventoryInfo, FileMode.Open);
        InventorySave i = (InventorySave)formatter.Deserialize(fs);
        fs.Close();
        return i;
    }
    /// <summary>
    /// loads player
    /// </summary>
    /// <returns></returns>
    public PlayerSave LoadPlayer()
    {
        string playerInfo = Path.Combine(saveLocation, "player.txt");
        FileStream fs = new FileStream(playerInfo, FileMode.Open);
        PlayerSave p = (PlayerSave)formatter.Deserialize(fs);
        fs.Close();
        return p;
    }
    /// <summary>
    /// loads world
    /// </summary>
    /// <returns></returns>
    public List<ChunkSave> LoadWorld()
    {
        List<ChunkSave> chunks = new List<ChunkSave>();
        foreach(string file in Directory.GetFiles(chunkLocation))
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            ChunkSave s = (ChunkSave)formatter.Deserialize(fs);
            fs.Close();
            chunks.Add(s);
        }
        return chunks;
    }
    /// <summary>
    /// load game information
    /// </summary>
    /// <returns></returns>
    public WorldInfo LoadGameInfo()
    {
        string worldInfo = Path.Combine(saveLocation, "worldInfo.txt");
        FileStream fs = new FileStream(worldInfo, FileMode.Open);
        WorldInfo w = (WorldInfo)formatter.Deserialize(fs);
        fs.Close();
        return w;
    }
}
