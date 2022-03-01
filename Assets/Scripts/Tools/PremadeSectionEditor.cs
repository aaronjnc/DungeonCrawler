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
        if (GUILayout.Button("Generate Maps"))
        {
            PremadeSection section = (PremadeSection)target;
            GameObject assetRoot = section.gameObject;
            if (assetRoot.name.Contains("Prefab") || section.generated)
                return;
            GenerateMaps(section, assetRoot);
            GenerateEnemies(section, assetRoot);
            section.generated = true;
            string assetPath = "Assets/Resources/PremadeSections/" + assetRoot.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(assetRoot, assetPath);
        }
    }
    private void GenerateMaps(PremadeSection section, GameObject contents)
    {
        string floorPath = "Assets/Resources/PremadeMaps/" + contents.name + "floor.txt";
        string wallPath = "Assets/Resources/PremadeMaps/" + contents.name + "wall.txt";
        Tilemap[] maps = contents.GetComponentsInChildren<Tilemap>();
        //section.floorMap = new byte[maps[0].cellBounds.size.x, maps[0].cellBounds.size.y];
        //section.wallMap = new byte[maps[0].cellBounds.size.x, maps[0].cellBounds.size.y];
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
                //section.floorMap[x - xMin, y - yMin] = floorId;
                //section.wallMap[x - xMin, y - yMin] = wallId;
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
        section.floorMap = Resources.Load("PremadeMaps/" + contents.name + "floor") as TextAsset;
        section.wallMap = Resources.Load("PremadeMaps/" + contents.name + "wall") as TextAsset;
    }
    private void GenerateEnemies(PremadeSection section, GameObject contents)
    {
        foreach (EnemyInfo enemy in contents.GetComponentsInChildren<EnemyInfo>())
        {
            Vector2Int pos = new Vector2Int((int)enemy.gameObject.transform.position.x, (int)enemy.gameObject.transform.position.y);
            section.enemies.Add(pos, enemy.id);
        }
    }
}
