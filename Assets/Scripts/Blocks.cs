using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Blocks : MonoBehaviour
{
    public Tile tile;
    public bool spawnable;
    public float chance;
    public bool solid;
    public bool onlyone;
    public bool breakable;
    public int biome;
    public bool custom;
    public bool prefab;
    [HideInInspector] public string blockName;
    public enum Type
    {
        Wall,
        Empty,
    }
    public Type blockType;
    public void SetSolid()
    {
        if (solid)
            tile.colliderType = Tile.ColliderType.Grid;
        else
            tile.colliderType = Tile.ColliderType.None;
    }
}
