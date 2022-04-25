using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public string levelFile;
    public int numCheckpoints;
    public List<GameObject> checkpoints;

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

    public void ClearCheckpoints()
    {
        foreach (var c in checkpoints)
        {
            Destroy(c.gameObject);
        }
    }

    public void PlaceCheckpointsUniformly()
    {
        int numPositions = _kitePath.GetPositions().Count;
        for (int i = 0; i < numPositions; i += numPositions / numCheckpoints)
        {
            var checkpointObj = Instantiate(checkpointPrefab, _kitePath.GetPositions()[i], Quaternion.identity);
            var checkpoint = checkpointObj.GetComponent<Checkpoint>();
            checkpoint.transform.up = _kitePath.GetDirs()[i];
            checkpoint.parentPath = this;
            checkpoints.Add(checkpointObj);
        }
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