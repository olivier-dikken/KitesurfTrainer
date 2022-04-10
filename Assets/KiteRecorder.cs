using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class KiteRecorder : MonoBehaviour
{
    public string filename = "Assets/LevelData/<name>";

    private List<Vector3> _points;
    private bool _isRecording;

    void Start()
    {
        _points = new List<Vector3>();
        _isRecording = false;
    }

    private void StartRecording()
    {
        _points = new List<Vector3>();
        _isRecording = true;
    }

    private void StopRecording()
    {
        _isRecording = false;
    }

    private void SaveRecording()
    {
        var text = new StringBuilder();
        foreach (var point in _points)
        {
            text.Append($"{point.x}, {point.y}, {point.z}\n");
        }

        var sr = new StreamWriter(filename);
        sr.Write(text);
    }

    public void ToggleRecording()
    {
        if (!_isRecording)
        {
            Debug.Log("Started Recording");
            StartRecording();
        }
        else
        {
            Debug.Log("Finished Recording");
            StopRecording();
            SaveRecording();
        }
    }

    void FixedUpdate()
    {
        // if recording save to points
        if (_isRecording)
        {
            _points.Add(transform.position);
        }
    }
}