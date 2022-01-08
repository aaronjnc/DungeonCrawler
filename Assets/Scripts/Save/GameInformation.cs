using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GameInformation
{
    public double playHours = 0;
    public string[][] worldMap;
    public string[][] enemies;
    public float[] playerPos;
    public float[] playerRot;
    public float[] playerSize;
    public int[] currentChunk;
    public float health;
    public float magic;
    public int[] enabledSpells;
    public int seed;
    public int biomeSeed;
    public byte[,] inventory = new byte[5,7];
    public byte[,] stackSize = new byte[5, 7];
    public byte[,] durability = new byte[5,7];
    public int rotator;
    public int currentChoice;
    public int[,] chosenItems = new int[7, 2];
    /// <summary>
    /// Sets up game information with passed in manager
    /// </summary>
    /// <param name="manager"></param>
    public GameInformation(GameObject manager)
    {
        SaveWorld(manager.GetComponent<ChunkGen>());
        SaveManager(manager.GetComponent<GameManager>());
        SavePlayer(GameObject.Find("Player"));
    }
    /// <summary>
    /// Saves information contained within gamemanager
    /// </summary>
    /// <param name="manager"></param>
    void SaveManager(GameManager manager)
    {
        TimeSpan playTime = System.DateTime.Now.Subtract(manager.startTime);
        playHours = manager.hours + playTime.TotalHours;
    }
    /// <summary>
    /// Saves chunk information
    /// </summary>
    /// <param name="gen"></param>
    void SaveWorld(ChunkGen gen)
    {
        worldMap = gen.getWorldMap();
        seed = gen.seed;
        biomeSeed = gen.biomeseed;
    }
    /// <summary>
    /// Saves player information
    /// </summary>
    /// <param name="player"></param>
    void SavePlayer(GameObject player)
    {
        Transform p = player.transform;
        playerPos = new float[] { p.position.x, p.position.y, p.position.z };
        playerRot = new float[] { p.eulerAngles.x, p.eulerAngles.y, p.eulerAngles.z };
        playerSize = new float[] { p.lossyScale.x, p.lossyScale.y, p.lossyScale.z };
        Vector2Int chunk = player.GetComponent<FreePlayerMove>().currentChunk;
        currentChunk = new int[] { chunk.x, chunk.y };
        health = player.GetComponent<PlayerFight>().health;
        magic = player.GetComponent<Magic>().magic;
        enabledSpells = player.GetComponent<Magic>().enabledSpells;
        SaveInventory(player.GetComponent<FreePlayerMove>().canvas.GetComponent<Inventory>());
    }
    /// <summary>
    /// Saves inventory information
    /// </summary>
    /// <param name="inv"></param>
    void SaveInventory(Inventory inv)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                ItemSlot iRef = inv.getItemSlot(i, j);
                if (!iRef.isEmpty())
                {
                    inventory[i, j] = iRef.getItemId();
                    stackSize[i, j] = iRef.getCurrentCount();
                    durability[i,j] = iRef.getDurability();
                } 
                else
                {
                    inventory[i, j] = 127;
                }
            }
        }
        for (int i = 0; i < 7; i++)
        {
            chosenItems[i, 0] = inv.chosenItems[i].x;
            chosenItems[i, 1] = inv.chosenItems[i].y;
        }
        currentChoice = inv.itemRotator.current;
    }
}
