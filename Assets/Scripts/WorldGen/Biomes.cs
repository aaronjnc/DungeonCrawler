using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biomes : MonoBehaviour
{
    [Tooltip("Biome ID")]
    public int biomeID;
    [Tooltip("Name of biome")]
    public string biomeName;
    [Tooltip("Wall blocks in biome")]
    public List<Blocks> biomeBlocks = new List<Blocks>();
    [Tooltip("Biome chance")] [Range(0, 1)]
    public float chance;
    [Tooltip("Blocks used to fill in empty spaces in biome")]
    public List<Blocks> emptyBlocks = new List<Blocks>();
    [Tooltip("Enemies found in biome")]
    public List<GameObject> enemies = new List<GameObject>();
    [Tooltip("Floor blocks found in biome")]
    public List<Blocks> floorBlocks = new List<Blocks>();
    [Tooltip("Biome scale affecting spawn rate")]
    public float scale;
    [Tooltip("Biome weight, affects spawn rate")][Range(0, 1)]
    public float weight;
    [Tooltip("Special blocks found in biome")]
    public List<Blocks> specialBlocks = new List<Blocks>();
}
