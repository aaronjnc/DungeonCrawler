using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteProcessor : AssetPostprocessor
{
    /// <summary>
    /// Sets sprite properties to be correct size and filter mode
    /// </summary>
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.spritePixelsPerUnit = 32;
        textureImporter.filterMode = FilterMode.Point;
    }
}
