using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxScript : MonoBehaviour
{

    public Vector3 force;
    public Vector3 shift;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + shift, transform.position + force + shift);
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.F))
        {
            _rb.AddForceAtPosition(force, transform.position + shift); 
        }
        
    }
}
