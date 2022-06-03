using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Path path = (Path) target;

        DrawDefaultInspector();
    
        // Load path from File
        if (GUILayout.Button("Load"))
        {
            path.Load();
        }
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear Checkpoints"))
        {
            path.ClearCheckpoints();
        }

        if (GUILayout.Button("Load Checkpoints uniformly"))
        {
            path.PlaceCheckpointsUniformly();
        }
        
        GUILayout.EndHorizontal();
        
        
        
    }
    
}