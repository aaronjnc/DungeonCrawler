using System.Collections;
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
        
    }
}
