using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path : MonoBehaviour
{
    public string levelFile;
    public GameObject checkpointPrefab;
    private KitePath _kitePath;
    private LineRenderer _lr;


    public void Load()
    {
        _lr = GetComponent<LineRenderer>();
        _kitePath = new KitePath();
        _kitePath.ReadFromFile(levelFile);
        UpdateLineRenderer();
    }

    public void AddCheckpoint()
    {
        GameObject checkpointObj = Instantiate(checkpointPrefab, transform);
        Checkpoint checkpoint = checkpointObj.GetComponent<Checkpoint>();
        checkpoint.parentPath = this;
        checkpointObj.transform.position = _kitePath.GetPositions()[0];
        
        Debug.Log(checkpoint.parentPath.name);
        
        // _kitePath.AddCheckPoint(checkpoint);
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