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
    string prevspriteName;
    public Vector3Int prevmapPos = Vector3Int.zero;
    Vector3Int mapPos = Vector3Int.zero;
    int mapz;
    ItemRotator currentRotator;
    public SwapRotators swapRotators;
    public Inventory inventory;
    Vector2Int currentChunk = Vector2Int.zero;
    Vector2Int prevChunk = Vector2Int.zero;
    Tile destroy;
    Tile place;
    Tile refillTile;
    void Awake()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        controls = new PlayerControls();
        mapz = manager.mapz;
        mapPos.z = mapz;
        controls.Interact.Press.performed += ReplaceTile;
        controls.Interact.Enable();
        destroy = new Tile();
        destroy.color = new Color(255, 0, 0);
        place = new Tile();
        place.color = new Color(255, 255, 255, 100);
        refillTile = new Tile();
        refillTile.color = new Color(255, 255, 255, 255);
    }
    public void Positioning(Vector3Int newPos, Vector2Int chunkPos)
    {
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
        if (prevmapPos != Vector3Int.zero)
        {
            ResetPrev();
        }
        if (manager.placing && GetTile(mapPos, currentChunk) == null)
        {
            UpdateTile(mapPos, manager.currentTileID, currentChunk);
            UpdateColor(mapPos, place, currentChunk);
            UpdateCollider(mapPos, Tile.ColliderType.None, currentChunk);
            prevmapPos = mapPos;
        }
        else if (!manager.placing && GetTile(mapPos, currentChunk) != null && manager.breakable(mapPos, currentChunk))
        {
            UpdateColor(mapPos, destroy, currentChunk);
            prevmapPos = mapPos;
        }
        else
        {
            prevmapPos = Vector3Int.zero;
        }
    }
    public void ResetPrev()
    {
        if (manager.placing)
        {
            UpdateTile(prevmapPos, 127, prevChunk);
        }
        else
        {
            UpdateColor(prevmapPos, refillTile, prevChunk);
        }
        prevmapPos = Vector3Int.zero;
    }
    void ReplaceTile(CallbackContext ctx)
    {
        if (manager.paused)
            return;
        if (manager.GetByte(mapPos, currentChunk) == 127)
            return;
        if (manager.blockplacing && !manager.inv.gameObject.activeInHierarchy)
        {
            if (!manager.placing)
            {
                if (GetTile(mapPos,currentChunk).color.b != 255)
                {
                    manager.inv.reduceDurability(new Vector2Int(swapRotators.current,swapRotators.chosen));
                    manager.inv.AddItem(GetBlock(mapPos,currentChunk));
                    UpdateTile(mapPos, 127, currentChunk);
                }
            }
            else if (manager.placing && GetTile(mapPos, currentChunk).color.a != 255)
            {
                manager.inv.reduceStack(new Vector2Int(swapRotators.current, swapRotators.chosen));
                UpdateTile(mapPos, manager.currentTileID, currentChunk);
                UpdateColor(mapPos, refillTile, currentChunk);
            }
            prevmapPos = Vector3Int.zero;
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
    void UpdateTile(Vector3Int tilePos, byte tile, Vector2Int chunkPos)
    {
        ChunkGen.currentWorld.UpdateByte(new Vector2Int(tilePos.x, tilePos.y),tile, chunkPos);
    }
    void UpdateColor(Vector3Int tilePos, Tile updateTile, Vector2Int chunkPos)
    {
        ChunkGen.currentWorld.UpdateColor(new Vector2Int(tilePos.x, tilePos.y), updateTile, chunkPos);
    }
    void UpdateCollider(Vector3Int tilePos, Tile.ColliderType tileCollider, Vector2Int chunkPos)
    {
        ChunkGen.currentWorld.UpdateCollider(tilePos, tileCollider, chunkPos);
    }
    Tile GetTile(Vector3Int tilePos, Vector2Int chunkPos)
    {
        return ChunkGen.currentWorld.GetTile(tilePos, chunkPos);
    }
    byte GetBlock(Vector3Int tilePos, Vector2Int chunkPos)
    {
        return ChunkGen.currentWorld.GetBlock(new Vector2Int(tilePos.x, tilePos.y), chunkPos);
    }
}
