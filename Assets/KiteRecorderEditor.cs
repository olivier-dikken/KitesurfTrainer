using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(KiteRecorder))]

public class KiteRecorderEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        KiteRecorder recorder = (KiteRecorder) target;

        DrawDefaultInspector();

        if (GUILayout.Button("Record"))
        {
            recorder.ToggleRecording(); 
        }

    }
}