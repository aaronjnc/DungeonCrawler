using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Tooltip("Eliminate lone tiles")] public bool revisiting = false;

    [Header("Modes:")]
    public bool testingmode = true;
    [Tooltip("Pregenerated mapsize (size x size)")]public int testingsize = 20;
    public bool spawnEnemies;

    [Header("Abilities:")]
    public bool ropeplacing = false;
    [HideInInspector]
    public bool blockplacing = false;
    [HideInInspector]
    public bool placing = false;
    [HideInInspector]
    public string tileName;
    [Header("Maps:")]
    [Tooltip("Map containing background floor design")]public Tilemap floor;
    [Tooltip("Map containg majority of tiles")]public Tilemap map;
    [HideInInspector]
    public List<Blocks> blocks = new List<Blocks>();
    [HideInInspector]
    public Vector3Int pos = Vector3Int.zero;
    [Header("Script Refs:")]
    public DestroyandPlace destroyandPlace;
    public RopeSystem ropesystem;
    List<string> blockNames = new List<string>();
    Sprite[] sprites;
    Sprite[] posts;
    List<string> spriteNames = new List<string>();
    [HideInInspector] public int mapz = -1;
    [HideInInspector] public int floorz = 0;
    [HideInInspector] public List<InventoryItem> items = new List<InventoryItem>();
    [HideInInspector] public Inventory inv;
    [HideInInspector] public bool fighting = false;
    [HideInInspector] public ItemReference currentItem;
    [HideInInspector] public bool invOpen = false;
    [HideInInspector] public List<Tile[]> biomeBlocks = new List<Tile[]>();
    public GameObject character;
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public GameObject invObject;
    public GameObject Astar;
    [HideInInspector] public List<Vector3Int> markets = new List<Vector3Int>();
    [HideInInspector] public List<Vendor> vendors = new List<Vendor>();
    public int gold = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (!spawnEnemies)
            Astar.SetActive(false);
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        currentItem = new ItemReference();
        foreach (GameObject block in Resources.LoadAll("Blocks"))
        {
            blocks.Add(block.GetComponent<Blocks>());
            blocks[blocks.Count - 1].SetSolid();
            blockNames.Add(block.name);
        }
        foreach(GameObject item in Resources.LoadAll("Items"))
        {
            InventoryItem invItem = item.GetComponent<InventoryItem>();
            invItem.durability = invItem.baseDurability;
            invItem.currentStack = 1;
            invItem.damage = 0;
            items.Add(invItem);
        }
        sprites = Resources.LoadAll<Sprite>("Images");
        posts = Resources.LoadAll<Sprite>("Images/Posts");
        foreach(Sprite sprite in sprites)
        {
            spriteNames.Add(sprite.name);
        }
        foreach(Sprite post in posts)
        {
            spriteNames.Add(post.name);
        }
        GetTile("Post").sprite = Resources.Load<Sprite>("Images/Post");
    }
    public Tile GetTile(string tileName)
    {
        if (blockNames.Contains(tileName))
            return blocks[blockNames.IndexOf(tileName)].tile;
        return null;
    }
    public bool Solid(Vector3Int pos)
    {
        pos.z = mapz;
        string tileName = map.GetTile(pos).name;
        if (blockNames.Contains(tileName))
            return blocks[blockNames.IndexOf(tileName)].solid;
        return false;
    }
    public Sprite GetSprite(string spriteName)
    {
        if (spriteNames.Contains(spriteName))
        {
            int index = spriteNames.IndexOf(spriteName);
            if (index >= sprites.Length)
                return posts[index - sprites.Length];
            else
                return sprites[index];
        }
        return null;
    }
    public Tile GetSprite(Vector2 dir, string spriteName, int mod, Vector3Int spot, string tileName)
    {
        Tile tile = GetTile(tileName);
        string postName = "";
        if (tile.name == "Post")
        {
            postName = spriteName;
        }
        Tile postTile = GetTile("Post");
        if (dir == Vector2.zero)
        {
            return tile;
        }
        string upName = map.GetTile(new Vector3Int(spot.x, spot.y+1, mapz)).name;
        string rightName = map.GetTile(new Vector3Int(spot.x + 1, spot.y, mapz)).name; 
        string downName = map.GetTile(new Vector3Int(spot.x, spot.y - 1, mapz)).name;
        string leftName = map.GetTile(new Vector3Int(spot.x - 1, spot.y, mapz)).name;
        bool UpandDown = (upName.Equals("Rope") || upName.Equals("Post")) && (downName.Equals("Post") || downName.Equals("Rope"));
        bool LeftandRight = (rightName.Equals("Rope") || rightName.Equals("Post")) && (leftName.Equals("Post") || leftName.Equals("Rope"));
        string position = "Images/Posts/";
        if (postName.Contains("Up") || dir.y*mod == 1 || (dir.y == 0 && UpandDown))
        {
            position += "Up";
        }
        if (postName.Contains("Right") || dir.x * mod == 1 || (dir.x == 0 && LeftandRight))
        {
            position += "Right";
        }
        if (postName.Contains("Down") || dir.y * mod == -1 || (dir.y == 0 && UpandDown))
        {
            position += "Down";
        }
        if (postName.Contains("Left") || dir.x * mod == -1 || (dir.x == 0 && LeftandRight))
        {
            position += "Left";
        }
        postTile.sprite = Resources.Load<Sprite>(position);
        return postTile;
    }
    public void ResetPrev()
    {
        if (destroyandPlace.prevmapPos != Vector3Int.zero)
            destroyandPlace.ResetPrev();
        destroyandPlace.enabled = false;
    }
    public void ResetRopes()
    {
        ropeplacing = false;
        ropesystem.enabled = false;
    }
    public void SetSolid(bool solid)
    {
        blocks[blockNames.IndexOf("Remove")].solid = solid;
    }
    public Sprite InventorySprite(string name)
    {
        foreach(InventoryItem item in items)
        {
            if (item.name == name)
                return item.itemSprite;
        }
        return null;
    }
    public InventoryItem GetItem(string name)
    {
        foreach (InventoryItem item in items)
        {
            if (item.name.Equals(name))
                return item;
        }
        return null;
    }
    public List<InventoryItem> GetLevel(int level)
    {
        List<InventoryItem> levelItems = new List<InventoryItem>();
        for (int i = 0; i < items.Count;i++)
        {
            if (items[i].level == level)
                levelItems.Add(items[i]);
        }
        return levelItems;
    }
    public int GetID(string spriteName)
    {
        for (int i = 0; i < items.Count;i++)
        {
            if (items[i].itemSprite.name.Equals(spriteName))
                return i;
        }
        return 0;
    }
    public bool breakable(Vector3Int pos)
    {
        pos.z = mapz;
        string tileName = map.GetTile(pos).name;
        if (blockNames.Contains(tileName))
        {
            return blocks[blockNames.IndexOf(tileName)].breakable;
        }
        return false;
    }
}
