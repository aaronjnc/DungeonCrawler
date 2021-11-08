﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameInformation
{
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
    public byte[,,] inventory = new byte[5,5,7];
    public byte[,,] stackSize = new byte[5, 5, 7];
    public byte[,,] durability = new byte[5,5,7];
    public int rotator;
    public int currentChoice;
    public int[,,] chosenPos = new int[5,5,2];
    public GameInformation(GameObject manager)
    {
        SaveWorld(manager.GetComponent<ChunkGen>());
        SaveManager(manager.GetComponent<GameManager>());
        SavePlayer(GameObject.Find("Player"));
    }
    void SaveManager(GameManager manager)
    {

    }
    void SaveWorld(ChunkGen gen)
    {
        worldMap = gen.getWorldMap();
        seed = gen.seed;
        biomeSeed = gen.biomeseed;
    }
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

    void SaveInventory(Inventory inv)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                chosenPos[i, j, 0] = inv.chosenPos[i, j].x;
                chosenPos[i, j, 1] = inv.chosenPos[i, j].y;
                if (inv.chosenItems[i,j].empty)
                {
                    chosenPos[i, j, 0] = int.MaxValue;
                }
                for (int k = 0; k < 7; k++)
                {
                    ItemReference iRef = inv.invItems[i, j, k];
                    if (!iRef.empty)
                    {
                        inventory[i, j, k] = iRef.itemID;
                        stackSize[i, j, k] = iRef.currentStack;
                        durability[i, j, k] = iRef.durability;
                    } else
                    {
                        inventory[i, j, k] = 127;
                    }
                }
            }
        }
        rotator = inv.swapRotators.current;
        currentChoice = inv.swapRotators.chosen;
    }
}
