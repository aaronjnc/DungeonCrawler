using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Blocks : MonoBehaviour
{
    [Tooltip("Tile used by block")]
    public Tile tile;
    [Tooltip("Spawn weight")]
    public float weight;
    [Tooltip("Spawn scale")]
    public float scale;
    [Tooltip("Block is solid")]
    public bool solid;
    [Tooltip("Block is breakable")]
    public bool breakable;
    [Tooltip("Block has custom spawning procedure")]
    public bool custom;
    [Tooltip("Index representing block")]
    public byte index;
    [Tooltip("Max blocks of this type per chunk")]
    public int maxperchunk;
    [Tooltip("Block is interactable")]
    public bool interactable;
    [Tooltip("Durability of block")]
    public int durability;
    [Tooltip("Items this block can drop")]
    public List<InventoryItem> drops = new List<InventoryItem>();
    [Tooltip("Chance of item drops")]
    public List<float> chances = new List<float>();
    public enum Type
    {
        Wall,
        Empty,
        Floor,
    }
    [Tooltip("Block type")]
    public Type blockType;
}
