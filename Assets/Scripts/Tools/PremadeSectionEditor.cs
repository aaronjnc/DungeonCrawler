using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

[CustomEditor(typeof(PremadeSection))]
public class PremadeSectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Map"))
        {
            GameObject assetRoot = Selection.activeGameObject as GameObject;
            string assetPath = AssetDatabase.GetAssetPath(assetRoot);
            if (assetPath.Contains("PremadeSections") || assetRoot.name.Contains("Prefab"))
                return;
            using (var editingScope = new PrefabUtility.EditPrefabContentsScope(assetPath))
            {
                var prefabRoot = editingScope.prefabContentsRoot;
                GenerateMaps(prefabRoot.GetComponent<PremadeSection>(), prefabRoot);
                GenerateEnemies(prefabRoot.GetComponent<PremadeSection>(), prefabRoot);
                Debug.Log(prefabRoot.GetComponent<PremadeSection>().floorMap);
            }
            GameObject check = PrefabUtility.LoadPrefabContents(assetPath);
            Debug.Log(check.GetComponent<PremadeSection>().floorMap);
            PrefabUtility.UnloadPrefabContents(check);
        }
    }
    private void GenerateMaps(PremadeSection section, GameObject contents)
    {
        Tilemap[] maps = contents.GetComponentsInChildren<Tilemap>();
        section.floorMap = new byte[maps[0].cellBounds.size.x, maps[0].cellBounds.size.y];
        section.wallMap = new byte[maps[0].cellBounds.size.x, maps[0].cellBounds.size.y];
        int xMin = maps[0].cellBounds.xMin;
        int yMin = maps[0].cellBounds.yMin;
        for (int x = xMin; x < maps[0].cellBounds.xMax; x++)
        {
            for (int y = yMin; y < maps[0].cellBounds.yMax; y++)
            {
                string floorName = maps[0].GetTile(new Vector3Int(x, y, 0)).name;
                byte floorId = ResourceInformation.GetBlockId(floorName);
                byte wallId;
                if (maps[1].GetTile(new Vector3Int(x,y,0)) == null)
                {
                    wallId = 127;
                } else
                {
                    string wallName = maps[1].GetTile(new Vector3Int(x, y, 0)).name;
                    wallId = ResourceInformation.GetBlockId(wallName);
                }
                section.floorMap[x - xMin, y - yMin] = floorId;
                section.wallMap[x - xMin, y - yMin] = wallId;
            }
        }
        //DestroyImmediate(maps[0].gameObject, true);
        //DestroyImmediate(maps[1].gameObject, true);
        //DestroyImmediate(contents.transform.Find("Grid").gameObject);
    }
    private void GenerateEnemies(PremadeSection section, GameObject contents)
    {
        foreach(EnemyInfo enemy in contents.GetComponentsInChildren<EnemyInfo>())
        {
            Vector2Int pos = new Vector2Int((int)enemy.gameObject.transform.position.x, (int)enemy.gameObject.transform.position.y);
            section.enemies.Add(pos, enemy.id);
        }
        //DestroyImmediate(contents.transform.Find("Enemies").gameObject);
    }
}
