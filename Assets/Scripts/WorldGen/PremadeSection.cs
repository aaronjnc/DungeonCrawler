using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PremadeSection : MonoBehaviour
{
    [Tooltip("Floor map text asset")]
    public TextAsset floorMap;
    [Tooltip("Wall map text asset")]
    public TextAsset wallMap;
    [Tooltip("Enemies in premade section")]
    public Dictionary<Vector2Int, byte> enemies = new Dictionary<Vector2Int, byte>();
    [Tooltip("Create section at start of game")]
    public bool CreateAtStart;
    [Tooltip("Number of sections in world")]
    public int worldCount;
    [Tooltip("Minimum start position")]
    public Vector2Int minStart;
    [Tooltip("Maximum start position")]
    public Vector2Int maxStart;
    [Tooltip("Premade section is entire chunk")]
    public bool entireChunk;
    [Tooltip("Biome of section")]
    [HideInInspector] public int biome;
}
