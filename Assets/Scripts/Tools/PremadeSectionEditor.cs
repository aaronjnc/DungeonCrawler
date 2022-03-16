using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using System.IO;

[CustomEditor(typeof(PremadeSection))]
public class PremadeSectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PremadeSection section = (PremadeSection)target;
        if (section.entireChunk)
        {
            section.biome = EditorGUILayout.IntField("Biome", section.biome);
        }
        if (GUILayout.Button("Generate Maps"))
        {
            GameObject assetRoot = section.gameObject;
            if (assetRoot.name.Contains("Prefab"))
                return;
            string dir = Path.Combine("Assets/Resources/PremadeMaps", assetRoot.name);
            Directory.CreateDirectory(dir);
            GenerateMaps(section, assetRoot, dir);
            GenerateEnemies(section, assetRoot, dir);
            string assetPath = "Assets/Resources/PremadeSections/" + assetRoot.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(assetRoot, assetPath);
        }
    }
    /// <summary>
    /// Generate text files for tilemaps
    /// </summary>
    /// <param name="section"></param>
    /// <param name="contents"></param>
    private void GenerateMaps(PremadeSection section, GameObject contents, string dir)
    {
        string floorPath = Path.Combine(dir, contents.name + "floor.txt");
        string wallPath = Path.Combine(dir, contents.name + "wall.txt");
        Tilemap[] maps = contents.GetComponentsInChildren<Tilemap>();
        File.WriteAllText(floorPath, "");
        File.WriteAllText(wallPath, "");
        StreamWriter floorWriter = new StreamWriter(floorPath, true);
        StreamWriter wallWriter = new StreamWriter(wallPath, true);
        int xMin = maps[0].cellBounds.xMin;
        int yMin = maps[0].cellBounds.yMin;
        for (int y = maps[0].cellBounds.yMax - 1; y >= yMin; y--)
        {
            for (int x = xMin; x < maps[0].cellBounds.xMax; x++)
            {
                string floorName = maps[0].GetTile(new Vector3Int(x, y, 0)).name;
                byte floorId = ResourceInformation.GetBlockId(floorName);
                byte wallId;
                if (maps[1].GetTile(new Vector3Int(x, y, 0)) == null)
                {
                    wallId = 127;
                }
                else
                {
                    string wallName = maps[1].GetTile(new Vector3Int(x, y, 0)).name;
                    wallId = ResourceInformation.GetBlockId(wallName);
                }
                floorWriter.Write(floorId);
                wallWriter.Write(wallId);
                if (x != maps[0].cellBounds.xMax - 1)
                {
                    floorWriter.Write("|");
                    wallWriter.Write("|");
                }
            }
            if (y != yMin)
            {
                floorWriter.Write("\n");
                wallWriter.Write("\n");
            }     
        }
        wallWriter.Close();
        floorWriter.Close();
        AssetDatabase.ImportAsset(floorPath);
        AssetDatabase.ImportAsset(wallPath);
        section.floorMap = Resources.Load("PremadeMaps/" + contents.name + "/" + contents.name + "floor") as TextAsset;
        section.wallMap = Resources.Load("PremadeMaps/" + contents.name + "/" + contents.name + "wall") as TextAsset;
    }
    private void GenerateEnemies(PremadeSection section, GameObject contents, string dir)
    {
        string enemyPath = Path.Combine(dir, contents.name + "enemy.txt");
        File.WriteAllText(enemyPath, "");
        StreamWriter sw = new StreamWriter(enemyPath, true);
        Tilemap tilemap = contents.GetComponentInChildren<Tilemap>();
        Vector2Int bottomLeft = new Vector2Int(tilemap.cellBounds.x, tilemap.cellBounds.y);
        foreach (EnemyInfo enemy in contents.GetComponentsInChildren<EnemyInfo>())
        {
            Vector2Int pos = new Vector2Int((int)enemy.gameObject.transform.position.x, (int)enemy.gameObject.transform.position.y);
            Vector2Int realPos = pos - bottomLeft;
            sw.WriteLine(realPos.x + " " + realPos.y + "|" + enemy.id);
        }
        sw.Close();
        AssetDatabase.ImportAsset(enemyPath);
        section.enemies = Resources.Load("PremadeMaps/" + contents.name + "/" + contents.name + "enemy") as TextAsset;
    }
}
