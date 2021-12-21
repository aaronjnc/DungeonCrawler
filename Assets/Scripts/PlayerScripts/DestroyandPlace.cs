using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;

public class DestroyandPlace : MonoBehaviour
{
    GameManager manager;
    PlayerControls controls;
    public string spriteName;
    public Vector3Int prevmapPos = Vector3Int.zero;
    Vector3Int mapPos = Vector3Int.zero;
    int mapz;
    public SwapRotators swapRotators;
    public Inventory inventory;
    Vector2Int currentChunk = Vector2Int.zero;
    Vector2Int prevChunk = Vector2Int.zero;
    Tile destroy;
    Tile refillTile;
    bool breaking;
    float blockHealth;
    Vector3Int destroyPos;
    Vector2Int destroyChunkPos;
    int damage;
    void Awake()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        controls = new PlayerControls();
        mapz = manager.mapz;
        mapPos.z = mapz;
        controls.Interact.Press.performed += ReplaceTile;
        controls.Interact.Press.canceled += StopBreaking;
        controls.Interact.Enable();
        destroy = new Tile();
        destroy.color = new Color(255, 0, 0);
        refillTile = new Tile();
        refillTile.color = new Color(255, 255, 255, 255);
    }
    /// <summary>
    /// Determines tile location and then calls methods to set position
    /// </summary>
    /// <param name="newPos">Chunk tile position</param>
    /// <param name="chunkPos">Chunk position</param>
    public void Positioning(Vector3Int newPos, Vector2Int chunkPos)
    {
        if (!breaking && prevmapPos != Vector3Int.zero && ChunkGen.currentWorld.GetBlock(new Vector2Int(prevmapPos.x, prevmapPos.y), currentChunk) != 127)
        {
            ResetPrevious();
        }
        Vector2Int chunkMod = new Vector2Int(0, 0);
        if (newPos.x < 0)
        {
            newPos.x = 63;
            chunkMod.x = -1;
        }
        else if (newPos.x > 63)
        {
            newPos.x = 0;
            chunkMod.x = 1;
        }
        if (newPos.y < 0)
        {
            newPos.y = 63;
            chunkMod.y = -1;
        }
        else if (newPos.y > 63)
        {
            newPos.y = 0;
            chunkMod.y = 1;
        }
        prevChunk = currentChunk;
        currentChunk = chunkPos + chunkMod;
        mapPos = newPos;
        mapPos.z = mapz;
        if (breaking && mapPos != destroyPos)
            ChangeBreaking(mapPos, currentChunk, GetBlock(mapPos, currentChunk));
        if (GetTile(mapPos, currentChunk) != null && manager.IsBreakable(mapPos, currentChunk))
        {
            UpdateColor(mapPos, destroy, currentChunk);
            prevmapPos = mapPos;
        }
        else
        {
            prevmapPos = Vector3Int.zero;
        }
    }
    private void FixedUpdate()
    {
        if (breaking)
        {
            blockHealth -= damage*Time.deltaTime;
            if (blockHealth <= 0)
                DestroyBlock(destroyPos, destroyChunkPos,GetBlock(destroyPos,destroyChunkPos));
        }
    }
    /// <summary>
    /// Resets previously chosen tile
    /// </summary>
    public void ResetPrevious()
    {
        UpdateColor(prevmapPos, refillTile, prevChunk);
        prevmapPos = Vector3Int.zero;
    }
    /// <summary>
    /// Replaces tile on mouse click
    /// </summary>
    /// <param name="ctx"></param>
    void ReplaceTile(CallbackContext ctx)
    {
        if (manager.paused)
            return;
        if (manager.GetByte(mapPos, currentChunk) == 127)
            return;
        if (manager.blockBreaking && !manager.inv.gameObject.activeInHierarchy)
        {
            if (GetTile(mapPos,currentChunk).color.b != 255)
            {
                ChangeBreaking(mapPos, currentChunk, GetBlock(mapPos, currentChunk));
            }
        }
    }
    void ChangeBreaking(Vector3Int newPos, Vector2Int newChunk, byte newID)
    {
        blockHealth = manager.GetBlock(newID).durability;
        destroyPos = newPos;
        destroyChunkPos = newChunk;
        damage = swapRotators.rotators[swapRotators.current].GetComponent<ItemRotator>().getChosen().getDamage();
        breaking = true;
    }
    void DestroyBlock(Vector3Int newPos, Vector2Int newChunk, byte blockID)
    {
        breaking = false;
        manager.inv.ReduceToolDurability(swapRotators.chosen);
        AddItem(blockID);
        UpdateTile(newPos, 127, newChunk);
    }
    private void AddItem(byte blockId)
    {
        Blocks block = manager.GetBlock(blockId);
        float randomVal = UnityEngine.Random.value;
        for (int i = 0; i < block.drops.Count; i++)
        {
            if (randomVal < block.chances[i])
            {
                manager.inv.AddItem(block.drops[i].itemID);
                return;
            }
        }
    }
    void StopBreaking(CallbackContext ctx)
    {
        breaking = false;
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
    /// <summary>
    /// Updates tile at given position
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="tile">Tile ID</param>
    /// <param name="chunkPos">Chunk position</param>
    void UpdateTile(Vector3Int tilePos, byte tile, Vector2Int chunkPos)
    {
        ChunkGen.currentWorld.UpdateByte(new Vector2Int(tilePos.x, tilePos.y),tile, chunkPos);
    }
    /// <summary>
    /// Updates color of tile at given position
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="updateTile">Tile with new color scheme</param>
    /// <param name="chunkPos">Chunk position</param>
    void UpdateColor(Vector3Int tilePos, Tile updateTile, Vector2Int chunkPos)
    {
        ChunkGen.currentWorld.UpdateColor(new Vector2Int(tilePos.x, tilePos.y), updateTile, chunkPos);
    }/// <summary>
    /// Updates collider of tile at given position
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="tileCollider">New Tilecollider</param>
    /// <param name="chunkPos">Chunk position</param>
    void UpdateCollider(Vector3Int tilePos, Tile.ColliderType tileCollider, Vector2Int chunkPos)
    {
        ChunkGen.currentWorld.UpdateCollider(tilePos, tileCollider, chunkPos);
    }
    /// <summary>
    /// Returns tile at given position
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="chunkPos">Chunk position</param>
    /// <returns></returns>
    Tile GetTile(Vector3Int tilePos, Vector2Int chunkPos)
    {
        return ChunkGen.currentWorld.GetTile(tilePos, chunkPos);
    }
    /// <summary>
    /// Returns the byte of block at given position
    /// </summary>
    /// <param name="tilePos">Chunk tile position</param>
    /// <param name="chunkPos">Chunk position</param>
    /// <returns></returns>
    byte GetBlock(Vector3Int tilePos, Vector2Int chunkPos)
    {
        return ChunkGen.currentWorld.GetBlock(new Vector2Int(tilePos.x, tilePos.y), chunkPos);
    }

    private void OnDestroy()
    {
        controls.Disable();
    }
}
