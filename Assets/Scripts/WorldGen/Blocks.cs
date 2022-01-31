﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Blocks : MonoBehaviour
{
    public Tile tile;
    public bool spawnable;
    public float weight;
    public float scale;
    public bool solid;
    public bool onlyone;
    public bool breakable;
    public int biome;
    public bool custom;
    public bool prefab;
    public byte index;
    public int maxperchunk;
    public bool interactable;
    public int durability;
    public List<InventoryItem> drops = new List<InventoryItem>();
    public List<float> chances = new List<float>();
    [HideInInspector] public string blockName;
    public enum Type
    {
        Wall,
        Empty,
        Floor,
    }
    public Type blockType;
}
