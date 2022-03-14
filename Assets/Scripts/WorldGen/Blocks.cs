using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Blocks : MonoBehaviour
{
    public Tile tile;
    public float weight;
    public float scale;
    public bool solid;
    public bool breakable;
    public bool custom;
    public byte index;
    public int maxperchunk;
    public bool interactable;
    public int durability;
    public List<InventoryItem> drops = new List<InventoryItem>();
    public List<float> chances = new List<float>();
    public enum Type
    {
        Wall,
        Empty,
        Floor,
    }
    public Type blockType;
}
