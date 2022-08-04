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
        return Mathf.PerlinNoise((position.x + .1f) / ChunkGen.Instance.chunkWidth * scale + offset, (position.y + .1f) / ChunkGen.Instance.chunkHeight * scale + offset);
    }
    /// <summary>
    /// Generate perlin noise for biome generation
    /// </summary>
    /// <param name="position"></param>
    /// <param name="offset"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static float Get2DPerlinChunk(Vector2Int position, float offset, float scale)
    {
        return Mathf.PerlinNoise((position.x + .1f) * scale + offset, (position.y + .1f) * scale + offset);
    }
}
