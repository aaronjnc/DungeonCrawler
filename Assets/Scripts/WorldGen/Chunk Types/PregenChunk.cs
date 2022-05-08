using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregenChunk : Chunk
{
    TextAsset floorMap;
    TextAsset wallMap;
    TextAsset enemyMap;
    public PregenChunk(Vector2Int pos) : base(pos)
    {
    }
    public PregenChunk(Vector2Int pos, PremadeSection section) : base(pos)
    {
        biomeId = (byte)section.biome;
        floorMap = section.floorMap;
        wallMap = section.wallMap;
        enemyMap = section.enemies;
    }

    public override float chance => throw new System.NotImplementedException();

    public override void GenerateChunk(int genNum)
    {
        numGen = genNum;
        FillBiomeMap();
        CreateTileMaps();
        FillBlocks(floorMap, 0);
        FillBlocks(wallMap, 1);
        SpawnEnemies(enemyMap);
        AddChangedBlocks();
        DrawTileMap();
        generated = true;
    }

    private void FillBlocks(TextAsset map, int z)
    {
        string textmap = map.text;
        string[] lines = textmap.Split('\n');
        for (int col = 0; col < lines.Length; col++)
        {
            string[] bytes = lines[col].Split('|');
            for (int row = 0; row < bytes.Length; row++)
            {
                byte blockID = Convert.ToByte(bytes[row]);
                Vector2Int newPos = new Vector2Int(lines.Length - col - 1, bytes.Length - row - 1);
                blocks[newPos.x, newPos.y] = blockID;
            }
        }
    }

    private void SpawnEnemies(TextAsset map)
    {
        if (GameManager.Instance.loadFromFile)
            return;
        string text = map.text;
        string[] lines = text.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] lineData = lines[i].Split('|');
            byte enemyID = Convert.ToByte(lineData[1]);
            string[] pos = lineData[0].Split(' ');
            Vector2Int newPos = new Vector2Int(Int32.Parse(pos[0]), Int32.Parse(pos[1]));
            GameObject enemy = GameObject.Instantiate(GameManager.Instance.GetMonsterObject(enemyID), monsterParent) as GameObject;
            enemy.transform.position = GetTileWorldPos(newPos.x, newPos.y, -1);
            enemy.GetComponent<MonsterInfo>().chunk = chunkPos;
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, monsterParent.position.z);
            monsters.Add(enemy.GetHashCode(), enemy);
        }
    }

    protected override void FillBiomeMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                biomes[i, j] = biomeId;
            }
        }
    }
}
