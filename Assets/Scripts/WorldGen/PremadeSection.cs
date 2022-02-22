using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PremadeSection : MonoBehaviour
{
    public byte[,] floormap = new byte[64,64];
    public byte[,] textmap = new byte[64, 64];
    public Dictionary<Vector2Int, int> enemies = new Dictionary<Vector2Int, int>();
    public bool CreateAtStart;
    public int worldCount;
    public Vector2Int minStart;
    public Vector2Int maxStart;
}
