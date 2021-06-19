using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class CreateWorld : MonoBehaviour
{
    GameManager manager;
    public int chunksize = 16;
    Tilemap map;
    Tilemap floor;
    Vector3Int pos = Vector3Int.zero;
    public float scale = 20f;
    int mapz;
    int floorz;
    int playerz;
    bool revisitTiles;
    List<Vector3Int> revisitSpots = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> marketpos = new List<Vector3Int>();
    public GameObject troll;
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        map = manager.map;
        floor = manager.floor;
        mapz = manager.mapz;
        floorz = manager.floorz;
        map.SetTile(new Vector3Int(1, 1, mapz), manager.GetTile("Market"));
        marketpos.Add(new Vector3Int(1, 1, mapz));
        for (int x = -1;x<=1;x++)
        {
            for (int y = -1;y<=1;y++)
            {
                map.SetTile(new Vector3Int(x, y, mapz), manager.GetTile("Empty"));
                floor.SetTile(new Vector3Int(x, y, floorz), manager.GetTile("Floor"));
            }
        }
        map.SetTile(new Vector3Int(1, 1, mapz), manager.GetTile("Market"));
        manager.markets.Add(new Vector3Int(1, 1, mapz));
        manager.vendors.Add(new Vendor());
        map.RefreshTile(new Vector3Int(1, 1, mapz));
        Spawn(new Vector3Int(0,0,mapz));
        if (manager.testingmode)
        {
            GetComponent<WorldCreationTesting>().enabled = true;
        }
        revisitTiles = manager.revisiting;
    }
    public void Spawn(Vector3Int pos)
    {
        for (int x = pos.x-9;x<=pos.x+9;x++)
        {
            for (int y = pos.y-5;y<=pos.y+5;y++)
            {
                Vector3Int spot = new Vector3Int(x, y, mapz);
                if (map.GetTile(spot) != null)
                    continue;
                floor.SetTile(new Vector3Int(x, y, floorz), manager.GetTile("Floor"));
                if (Random.value > Adjacent(spot))
                {
                    map.SetTile(spot, manager.GetTile("Empty"));
                    if (Random.value < Markets(pos))
                    {
                        map.SetTile(spot, manager.GetTile("Market"));
                        map.RefreshTile(spot);
                        marketpos.Add(spot);
                        manager.markets.Add(spot);
                        manager.vendors.Add(new Vendor());
                    }    
                    else if (Random.value < .03 && manager.spawnEnemies)
                    {
                        GameObject newtroll = Instantiate(troll);
                        newtroll.transform.position = new Vector3(x, y, -1);
                    }
                }
                else
                    map.SetTile(spot, manager.GetTile("Wall"));
                if (revisitTiles && !map.GetTile(spot).name.Equals("Market"))
                {
                    if (Revisit(spot))
                    {
                        revisitSpots.Add(spot);
                    }
                }
            }
        }
        if (revisitTiles)
        {
            foreach (Vector3Int revisitSpot in revisitSpots)
            {
                if (Revisit(revisitSpot))
                {
                    if (map.GetTile(revisitSpot).name.Equals("Wall"))
                    {
                        map.SetTile(revisitSpot, manager.GetTile("Empty"));
                    }
                    else
                    {
                        map.SetTile(revisitSpot, manager.GetTile("Wall"));
                    }
                    map.RefreshTile(revisitSpot);
                }
            }
            revisitSpots.Clear();
        }
        if (manager.spawnEnemies)
            AstarPath.active.Scan(AstarPath.active.graphs[0]);
    }
    bool Revisit(Vector3Int pos)
    {
        string tileName = map.GetTile(pos).name;
        int surrounding = 0;
        List<string> tiles = new List<string>();
        for (int x = pos.x-1;x<=pos.x+1;x++)
        {
            if (x == pos.x || map.GetTile(new Vector3Int(x, pos.y, mapz)) == null)
                continue;
            tiles.Add(map.GetTile(new Vector3Int(x, pos.y, mapz)).name);
        }
        for (int y = pos.y - 1; y <= pos.y + 1; y++)
        {
            if (y == pos.y || map.GetTile(new Vector3Int(pos.x, y, pos.z)) == null)
                continue;
            tiles.Add(map.GetTile(new Vector3Int(pos.x, y, pos.z)).name);
        }
        foreach(string tile in tiles)
        {
            if (tile.Equals(tileName))
                surrounding++;
        }
        if (surrounding==0)
            return true;
        return false;
    }
    float Adjacent(Vector3Int pos)
    {
        float chance = .2f;
        for(int x = pos.x-1;x<=pos.x+1;x++)
        {
            for (int y = pos.y-1;y<=pos.y+1;y++)
            {
                if ((y == pos.y && x == pos.x)|| map.GetTile(new Vector3Int(x, y, mapz)) == null)
                    continue;
                if (map.GetTile(new Vector3Int(x, y, mapz)).name == "Empty")
                    chance += .1f;
            }
        }
        return chance;
    }
    float Markets(Vector3Int pos)
    {
        float chance = .01f;
        int count = 0;
        foreach(Vector3Int market in marketpos)
        {
            int val = (int)Mathf.Abs(Vector3Int.Distance(pos, market)) / 10;
            switch (val)
            {
                case 0:
                    chance = 0;
                    count++;
                    break;
                case 1:
                    chance /= 5;
                    count++;
                    break;
                case 2:
                    chance /= 4;
                    count++;
                    break;
                case 3:
                    chance /= 2;
                    count++;
                    break;
            }
        }
        if (count == 0)
            chance += .02f;
        return chance;
    }
}
