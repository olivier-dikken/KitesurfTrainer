using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Path path = (Path) target;

        DrawDefaultInspector();

        if (GUILayout.Button("Load"))
        {
            path.Load();
        }
    }
}