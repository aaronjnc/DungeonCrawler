using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;

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
        string floorMap = "";
        string wallMap = "";
        for (int x = maps[0].cellBounds.xMax - 1; x >= xMin; x--)
        {
            string floorLine = "";
            string wallLine = "";
            for (int y = maps[0].cellBounds.yMax - 1; y >= yMin; y--)
            {
                TileBase t = maps[0].GetTile(new Vector3Int(x, y, 0));
                if (t == null)
                    continue;
                string floorName = t.name;
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
                if (!floorLine.Equals(""))
                {
                    floorLine += "|";
                    wallLine += "|";
                }
                floorLine += floorId;
                wallLine += wallId;
            }
            floorMap += floorLine + "\n";
            wallMap += wallLine + "\n";
        }
        floorMap = Regex.Replace(floorMap, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();
        wallMap = Regex.Replace(wallMap, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();
        wallWriter.Write(wallMap);
        floorWriter.Write(floorMap);
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
