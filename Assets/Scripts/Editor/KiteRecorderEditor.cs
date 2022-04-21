using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KiteRecorder))]
public class KiteRecorderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        KiteRecorder recorder = (KiteRecorder) target;

        DrawDefaultInspector();
        
        GUILayout.Space(15);

        // record button
        if (GUILayout.Button("Restart Recording"))
        {
            Debug.Log("Restarted Recording");
            recorder.Path.Clear();
        }

        GUILayout.BeginHorizontal();
        // save recording to file
        var filename = GUILayout.TextField("Assets/LevelData/example.txt");
        if (GUILayout.Button("SaveToFile"))
        {
            recorder.Path.SaveToFile(filename);
            Debug.Log($"Saved to {filename}");
        }

        GUILayout.EndHorizontal();
    }
}