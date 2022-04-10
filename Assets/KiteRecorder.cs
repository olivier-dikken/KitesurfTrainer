using UnityEngine;

public class KiteRecorder : MonoBehaviour
{
    public string filename = "Assets/LevelData/<name>";
    public KitePath KitePath;

    private bool _isRecording;

    void Start()
    {
        KitePath = new KitePath();
        _isRecording = false;
    }

    private void StartRecording()
    {
        KitePath.Clear();
        _isRecording = true;
    }

    private void StopRecording()
    {
        _isRecording = false;
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
            KitePath.SaveToFile(filename);
        }
    }

    void FixedUpdate()
    {
        // if recording save to points
        if (_isRecording)
        {
            KitePath.AddFrame(transform.position, transform.up, Time.time);
        }
    }
}