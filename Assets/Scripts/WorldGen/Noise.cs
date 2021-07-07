using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    /// <summary>
    /// Generates perlin noise at position
    /// </summary>
    /// <param name="position">Position</param>
    /// <param name="offset">Distance to move original position by</param>
    /// <param name="scale">Scale of perlin</param>
    /// <returns></returns>
    public static float Get2DPerlin(Vector2Int position, float offset, float scale)
    {
        return Mathf.PerlinNoise((position.x + .1f) / ChunkGen.currentWorld.chunkWidth * scale + offset, (position.y + .1f) / ChunkGen.currentWorld.chunkHeight * scale + offset);
    }
}
