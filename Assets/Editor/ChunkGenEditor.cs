using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(ChunkGen))]
public class ChunkGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ChunkGen chunkGen = (ChunkGen)target;
        if (GUILayout.Button("Regenerate"))
        {
            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                return;
            }
            chunkGen.Regenerate();
        }
    }
}
