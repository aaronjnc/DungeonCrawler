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
    private const string chunks = "world";
    private string saveLocation;
    private string chunkLocation;
    private BinaryFormatter formatter;
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
    public void SaveAll(GameManager manager)
    {
        SaveGameInfo(manager);
        SaveWorld(manager.gen);
        GameObject player = GameObject.Find("Player");
        SavePlayer(player);
        SaveInventory(manager.inv);
    }
    /// <summary>
    /// Saves information contained within gamemanager
    /// </summary>
    /// <param name="manager"></param>
    public void SaveGameInfo(GameManager manager)
    {
        string worldInfo = Path.Combine(saveLocation, "worldInfo.txt");
        WorldInfo info = new WorldInfo(manager.gen, manager);
        FileStream fs = new FileStream(worldInfo, FileMode.Create);
        formatter.Serialize(fs, info);
        fs.Close();
    }
    /// <summary>
    /// Saves chunk information
    /// </summary>
    /// <param name="gen"></param>
    public void SaveWorld(ChunkGen gen)
    {
        Hashtable chunks = gen.GetChunks();
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
    public void SavePlayer(GameObject player)
    {
        string playerInfo = Path.Combine(saveLocation, "player.txt");
        FileStream fs = new FileStream(playerInfo, FileMode.Create);
        PlayerSave p = new PlayerSave(player);
        formatter.Serialize(fs, p);
        fs.Close();
    }
    public void SaveInventory(Inventory inv)
    {
        string inventoryInfo = Path.Combine(saveLocation, "inventory.txt");
        FileStream fs = new FileStream(inventoryInfo, FileMode.Create);
        InventorySave i = new InventorySave(inv);
        formatter.Serialize(fs, i);
        fs.Close();
    }
    public InventorySave LoadInventory()
    {
        string inventoryInfo = Path.Combine(saveLocation, "inventory.txt");
        FileStream fs = new FileStream(inventoryInfo, FileMode.Open);
        InventorySave i = (InventorySave)formatter.Deserialize(fs);
        fs.Close();
        return i;
    }
    public PlayerSave LoadPlayer()
    {
        string playerInfo = Path.Combine(saveLocation, "player.txt");
        FileStream fs = new FileStream(playerInfo, FileMode.Open);
        PlayerSave p = (PlayerSave)formatter.Deserialize(fs);
        fs.Close();
        return p;
    }
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
    public WorldInfo LoadGameInfo()
    {
        string worldInfo = Path.Combine(saveLocation, "worldInfo.txt");
        FileStream fs = new FileStream(worldInfo, FileMode.Open);
        WorldInfo w = (WorldInfo)formatter.Deserialize(fs);
        fs.Close();
        return w;
    }
}
