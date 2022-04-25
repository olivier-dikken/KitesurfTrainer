using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class KitePath
{
    
    
    private List<Vector3> _positions;
    private List<Vector3> _directions;
    private List<Double> _times;
    
    private List<Checkpoint> _checkpoints;

    public KitePath()
    {
        Clear();
    }

    public void AddCheckPoint(Checkpoint point)
    {
        _checkpoints.Add(point);
    }
   

    public void AddFrame(Vector3 pos, Vector3 dir, Double time)
    {
        _positions.Add(pos);
        _directions.Add(dir);
        _times.Add(time);
    }

    public void ReadFromFile(string filename)
    {
        var sr = new StreamReader(filename, true);

        Clear();

        string line;
        while ((line = sr.ReadLine()) != null)
        {
            // split fields by semicolon
            var fields = line.Split(';');

            // add position
            var position = fields[0].Split(',');
            float x = float.Parse(position[0]);
            float y = float.Parse(position[1]);
            float z = float.Parse(position[2]);
            _positions.Add(new Vector3(x, y, z));

            // add direction
            var direction = fields[1].Split(',');
            float dx = float.Parse(direction[0]);
            float dy = float.Parse(direction[1]);
            float dz = float.Parse(direction[2]);
            _directions.Add(new Vector3(dx, dy, dz));

            // add time
            _times.Add(float.Parse(fields[2]));
        }
    }

    public void SaveToFile(string filename)
    {
        var text = new StringBuilder();
        for (int i = 0; i < _positions.Count; i++)
        {
            text.Append($"{_positions[i].x},{_positions[i].y},{_positions[i].z};");
            text.Append($"{_directions[i].x},{_directions[i].y},{_directions[i].z};");
            text.Append($"{_times[i]}\n");
        }

        var sr = new StreamWriter(filename);
        sr.Write(text);
    }

    public List<Vector3> GetPositions()
    {
        return _positions;
    }

    public void Clear()
    {
        _positions = new List<Vector3>();
        _directions = new List<Vector3>();
        _times = new List<Double>();
    }
}