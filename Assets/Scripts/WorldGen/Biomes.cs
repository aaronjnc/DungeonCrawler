﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biomes : MonoBehaviour
{
    public int biomeID;
    public string biomeName;
    public List<Blocks> biomeBlocks = new List<Blocks>();
    public float chance;
    public List<Blocks> emptyBlocks = new List<Blocks>();
    public List<GameObject> enemies = new List<GameObject>();
    public Blocks baseFloor;
    public float scale;
    public float weight;
}