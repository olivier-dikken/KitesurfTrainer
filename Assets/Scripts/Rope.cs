using System;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private readonly List<Rigidbody> _points = new();
    [SerializeField] private int numPoints = 35;
    [SerializeField] private float ropeSegLen = 0.25f;
    [SerializeField] private float lineWidth = 0.1f;

    private void Start()
    {
        for (int i = 0; i < numPoints; i++)
        {
        }
    }
}