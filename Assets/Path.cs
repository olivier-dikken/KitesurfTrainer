using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Path : MonoBehaviour
{
    public string levelFile;
    private KitePath _kitePath;
    private LineRenderer _lr;

    public void Load()
    {
        _lr = GetComponent<LineRenderer>();
        _kitePath = new KitePath();
        _kitePath.ReadFromFile(levelFile);
        UpdateLineRenderer();
    }
    
    private void UpdateLineRenderer()
    {
        _lr.positionCount = _kitePath.GetPositions().Count;
        for (int i = 0; i < _kitePath.GetPositions().Count; i++)
        {
            _lr.SetPosition(i, _kitePath.GetPositions()[i]);
        }
        
    }
    
}