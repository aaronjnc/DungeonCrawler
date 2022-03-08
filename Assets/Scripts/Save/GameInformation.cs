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
    /// <summary>
    /// Sets up game information with passed in manager
    /// </summary>
    /// <param name="manager"></param>
    public GameInformation()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        _instance = this;
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
        SaveInventory(player.GetComponent<Inventory>());
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
        foreach (Chunk chunk in chunks)
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
    public void LoadInventory()
    {

    }
    public void LoadPlayer()
    {

    }
    public void LoadWorld()
    {

    }
    public void LoadGameInfo()
    {

    }
}
