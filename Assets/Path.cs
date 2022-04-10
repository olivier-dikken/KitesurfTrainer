using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Path : MonoBehaviour
{
    public string levelFile;
    private List<Vector3> _points;
    private LineRenderer _lr;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _points = new List<Vector3>();
        Debug.Log(levelFile);
        ReadFromFile(levelFile); 
    }

    public void ReadFromFile(string file)
    {
        var sr = new StreamReader(file, true);

        _points = new List<Vector3>();

        string line;
        while ((line = sr.ReadLine()) != null)
        {
            var coords = line.Split(',');
            float x = float.Parse(coords[0]);
            float y = float.Parse(coords[1]);
            float z = float.Parse(coords[2]);

            _points.Add(new Vector3(x, y, z));
        }
        
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        _lr.positionCount = _points.Count;
        for (int i = 0; i < _points.Count; i++)
        {
            _lr.SetPosition(i, _points[i]);
        }
        
    }
    
}