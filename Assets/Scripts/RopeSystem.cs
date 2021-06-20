using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;

public class RopeSystem : MonoBehaviour
{

    //use array for spriteTiles
    //use list for line Tiles

    GameManager manager;
    PlayerControls controls;
    [HideInInspector] public Vector3Int startpos;
    Tilemap map;
    [HideInInspector] public bool placing = false;
    [HideInInspector] public int length = 0;
    [HideInInspector] public Vector3Int[] ropes = new Vector3Int[5];
    [HideInInspector] public Vector2 dir = new Vector2();
    Vector3Int firstspot;
    [HideInInspector] public List<string> lineTiles = new List<string>();
    [HideInInspector] public string[] spriteTiles = new string[7];
    Vector3Int pos = Vector3Int.zero;
    int mapz;
    FreePlayerMove player;
    public byte postID;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FreePlayerMove>();
        ResetRopes();
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        map = manager.map;
        controls = new PlayerControls();
        controls.Interact.Press.performed += Place;
        controls.Interact.Press.Enable();
        controls.Interact.Cancel.performed += Cancel;
        controls.Interact.Cancel.Enable();
        mapz = manager.mapz;
    }
    void Place(CallbackContext ctx)
    {
        startpos = player.pos;
        startpos.z = manager.mapz;
        if (placing)
        {
            placing = false;
            Finish();
            length = 0;
        } 
        else
        {
            lineTiles.Clear();
            firstspot = startpos;
            lineTiles.Add(map.GetTile(firstspot).name);
            if (lineTiles[0] != "Empty")
                spriteTiles[0]=map.GetSprite(firstspot).name;
            map.SetTile(startpos, manager.GetSprite(Vector2.zero,spriteTiles[0],1,startpos,lineTiles[0]));
            map.RefreshTile(startpos);
            placing = true;
        }
    }
    void Cancel(CallbackContext ctx)
    {
        if (placing)
            ClearRopes();
        placing = false;
    }
    public void Roping(Vector3Int playerpos)
    {
        pos = playerpos;
        if (placing)
            RopePlacement();
    }
    void RopePlacement()
    {
        if (pos.x == startpos.x ^ pos.y == startpos.y)
        {
            Tile ropeTile = manager.GetTile(postID);
            Vector3Int mapSpot = new Vector3Int(pos.x, pos.y, mapz);
            Sprite ropeSprite = null;
            int val = 0;
            int postPos = 0;
            Vector3Int ropeDir = Vector3Int.zero;
            bool xDir = pos.x == startpos.x && pos.y != startpos.y;
            bool yDir = pos.y == startpos.y && pos.x != startpos.x;
            if (xDir)
            {
                ropeSprite = Resources.Load<Sprite>("Images/RopeUp");
                ropeDir.y = 1;
                val = pos.y;
                postPos = startpos.y;
            }
            else if (yDir)
            {
                ropeSprite = Resources.Load<Sprite>("Images/Rope");
                ropeDir.x = 1;
                val = pos.x;
                postPos = startpos.x;
            }
            else
            {
                ClearRopes();
                map.SetTile(startpos, manager.GetSprite(Vector2.zero, spriteTiles[0], 1, startpos, lineTiles[0]));
            }
            if (xDir || yDir)
            {
                ropeTile.sprite = ropeSprite;
                int moveDir = -1;
                if (val > postPos)
                    moveDir = 1;
                ropeDir *= moveDir;
                int distance = Mathf.Abs(val - postPos);
                dir = new Vector2(ropeDir.x, ropeDir.y);
                if (distance <= 5 && distance > 0)
                {
                    if (ropes[distance - 1].z != 1)
                    {
                        lineTiles.RemoveAt(lineTiles.Count - 1);
                        spriteTiles[lineTiles.Count - 1] = "";
                        map.SetTile(ropes[distance - 1], null);
                        map.RefreshTile(ropes[distance - 1]);
                        ropes[distance - 1] = new Vector3Int(0, 0, 1);
                    }
                    else
                    {
                        ropes[distance - 1] = mapSpot - ropeDir;
                        lineTiles.Add(map.GetTile(ropes[distance - 1]).name);
                        if (lineTiles[lineTiles.Count - 1] != "Empty")
                            spriteTiles[lineTiles.Count - 1] = map.GetSprite(ropes[distance - 1]).name;
                        map.SetTile(ropes[distance - 1], ropeTile);
                        map.RefreshTile(ropes[distance - 1]);
                    }
                }
                if (distance <= 5)
                {
                    map.SetTile(startpos, manager.GetSprite(new Vector2(ropeDir.x, ropeDir.y), spriteTiles[0], 1, startpos, lineTiles[0]));
                    map.RefreshTile(startpos);
                }
            }
        }
    }
    void ClearRopes()
    {
        /*for (int i = 0; i < ropes.Length; i++)
        {
            if (ropes[i].z == 1)
                continue;
            Tile newTile = manager.GetTile(lineTiles[i + 1]);
            if (newTile != null)
                newTile.sprite = manager.GetSprite(spriteTiles[i + 1]);
            map.SetTile(ropes[i], newTile);
            map.RefreshTile(ropes[i]);
            ropes[i] = new Vector3Int(0, 0, 1);
            lineTiles.RemoveAt(i + 1);
            spriteTiles[i + 1] = "";
        }*/
    }
    void Finish()
    {
        int last = 0;
        for(int i = 0; i < ropes.Length;i++)
        {
            if (i-1 < lineTiles.Count && i-1 >= 0)
            {
                Tile post = manager.GetTile(postID);
                /*if (lineTiles[i-1] == "Rope" || lineTiles[i-1] == "RopeUp")
                {
                    post.sprite = Resources.Load<Sprite>("Images/Posts/UpRightDownLeft");
                    map.SetTile(ropes[i], post);
                    map.RefreshTile(ropes[i]);
                }*/
            }
            if (ropes[i].z != 1)
            {
                last++;
            }
            else
                break;
        }
        Vector3Int postpos = Vector3Int.zero; 
        if (last == 0)
        {
            postpos = firstspot;
        }
        else
        {
            postpos = ropes[last-1];
        }
        postpos = new Vector3Int((int)(postpos.x + dir.x), (int)(postpos.y + dir.y), manager.mapz);
        int mod = 1;
        lineTiles.Add(map.GetTile(postpos).name);
        if (lineTiles[lineTiles.Count - 1] != "Empty")
        {
            spriteTiles[lineTiles.Count - 1] = map.GetSprite(postpos).name;
        }
        if (lineTiles[lineTiles.Count-1] != "Wall")
        {
            map.SetTile(postpos, manager.GetSprite(dir,spriteTiles[lineTiles.Count-1],mod*-1,postpos,lineTiles[lineTiles.Count-1]));
            map.RefreshTile(postpos);
        }
        ResetRopes();
    }
    public void ResetRopes()
    {
        for (int i = 0; i < 5; i++)
        {
            ropes[i] = Vector3Int.zero;
            ropes[i].z = 1;
        }
    }
    private void OnDisable()
    {
        if (controls != null)
            controls.Disable();
    }
    private void OnEnable()
    {
        if (controls != null)
            controls.Enable();
    }
}
