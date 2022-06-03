using System;
using UnityEngine;

public class KiteRecorder : MonoBehaviour
{
    public KitePath Path;

    void Awake()
    {
        Path = new KitePath();
    }

    private void Update()
    {
        Path.AddFrame(transform.position, transform.up, Time.time);
    }
}