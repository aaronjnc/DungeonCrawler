using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapProcessor : AssetPostprocessor
{
    const string MAP_POSITION = "Assets/Resources/PremadeSections/";
    void OnPreprocessAsset()
    {
        if (!assetImporter.assetPath.Contains("Map.prefab"))
        {
            return;
        }
        string mapName = assetImporter.assetPath.Replace(MAP_POSITION, "");
        GameObject prefab = Resources.Load("MapCreation/PremadeSectionPrefab") as GameObject;
        Debug.Log(prefab.name);
    }
    private void AnalyzeMap(GameObject newMap, GameObject prefab)
    {
        //GameObject floor = newMap.Get
    }
}
