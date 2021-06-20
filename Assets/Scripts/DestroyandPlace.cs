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
    string prevtileName;
    string prevspriteName;
    Tilemap map;
    public Vector3Int prevmapPos = Vector3Int.zero;
    Vector3Int mapPos = Vector3Int.zero;
    int mapz;
    ItemRotator currentRotator;
    public SwapRotators swapRotators;
    public Inventory inventory;
    void Awake()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        map = manager.map;
        controls = new PlayerControls();
        mapz = manager.mapz;
        mapPos.z = mapz;
        controls.Interact.Press.performed += ReplaceTile;
        controls.Interact.Enable();
    }
    public void Positioning(Vector3Int newPos)
    {
        mapPos = newPos;
        mapPos.z = mapz;
        if (prevmapPos != Vector3Int.zero)
        {
            ResetPrev();
        }
        if (manager.placing && map.GetTile(mapPos) == null)
        {
            prevtileName = "Empty";
            map.SetTile(mapPos, manager.GetTile(manager.currentTileID));
            map.GetTile<Tile>(mapPos).color = new Color(255, 255, 255, 100);
            map.GetTile<Tile>(mapPos).colliderType = Tile.ColliderType.None;
            map.RefreshTile(mapPos);
            prevmapPos = mapPos;
        }
        else if (!manager.placing && map.GetTile(mapPos) != null && manager.breakable(mapPos))
        {
            prevtileName = map.GetTile(mapPos).name;
            prevspriteName = map.GetSprite(mapPos).name;
            map.GetTile<Tile>(mapPos).color = new Color(255, 0, 0);
            map.RefreshTile(mapPos);
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
            map.SetTile(prevmapPos, null);
        }
        else
        {
            map.GetTile<Tile>(prevmapPos).color = new Color(255, 255, 255, 255);
            map.RefreshTile(prevmapPos);
        }
        prevmapPos = Vector3Int.zero;
    }
    void ReplaceTile(CallbackContext ctx)
    {
        if (manager.blockplacing && !manager.inv.gameObject.activeInHierarchy)
        {
            if (!manager.placing && prevtileName != "Empty")
            {
                if (map.GetTile<Tile>(mapPos).color.b != 255)
                {
                    manager.inv.reduceDurability(new Vector2Int(swapRotators.current,swapRotators.chosen));
                    manager.inv.AddItem(prevtileName);
                    map.SetTile(mapPos, null);
                    map.RefreshTile(mapPos);
                    ChunkGen.currentWorld.UpdateByte(new Vector2Int(mapPos.x, mapPos.y), 0);
                    if (manager.spawnEnemies)
                        AstarPath.active.Scan(AstarPath.active.graphs[0]);
                }
            }
            else if (manager.placing && map.GetTile<Tile>(mapPos).color.a != 255)
            {
                manager.inv.reduceStack(new Vector2Int(swapRotators.current, swapRotators.chosen));
                map.SetTile(mapPos, manager.GetTile(manager.currentTileID));
                map.GetTile<Tile>(mapPos).color = new Color(255, 255, 255, 255);
                ChunkGen.currentWorld.UpdateByte(new Vector2Int(mapPos.x, mapPos.y), manager.currentTileID);
                if (manager.Solid(mapPos))
                    map.GetTile<Tile>(mapPos).colliderType = Tile.ColliderType.Grid;
                map.RefreshTile(mapPos);
                if (manager.spawnEnemies)
                    AstarPath.active.Scan(AstarPath.active.graphs[0]);
            }
            prevmapPos = Vector3Int.zero;
            prevtileName = "";
            prevspriteName = "";
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
