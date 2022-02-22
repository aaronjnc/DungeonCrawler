using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PremadeSection))]
public class PremadeSectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PremadeSection section = (PremadeSection)target;
        if (GUILayout.Button("Generate Map"))
        {

        }
    }
}
