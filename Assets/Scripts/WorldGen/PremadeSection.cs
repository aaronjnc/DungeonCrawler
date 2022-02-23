using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PremadeSection : MonoBehaviour
{
    public byte[,] floorMap;
    public byte[,] wallMap;
    public Dictionary<Vector2Int, byte> enemies = new Dictionary<Vector2Int, byte>();
    public bool CreateAtStart;
    public int worldCount;
    public Vector2Int minStart;
    public Vector2Int maxStart;
}
