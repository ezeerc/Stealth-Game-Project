using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class LaserScript : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int defaultLength = 50;

    private bool _laserOn;
    
    public LineRenderer _lineRenderer;
    private RaycastHit hit;
    private bool _laserCancel;

    void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        InstantiateFire.CancelFireON += LaserCancelON; //observer de cancelación de láser
        InstantiateFire.CancelFireOFF += LaserCancelOFF; //observer de cancelación de láser
    }

    public void LaserOn()
    {
        if (_laserCancel) return;
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out hit, defaultLength, layerMask))
        {
            _lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            _lineRenderer.SetPosition(1, transform.position + (transform.forward * defaultLength));
        }

    }

    public void LaserOff()
    {
        if(!_lineRenderer) return;
        _lineRenderer.enabled = false;
    }

    public void LaserCancelON()
    {
        _laserCancel = true;
        LaserOff();
    }
    
    public void LaserCancelOFF()
    {
        _laserCancel = false;
    }

    private void OnDestroy()
    {
        InstantiateFire.CancelFireON -= LaserCancelON; //observer de cancelación de láser
        InstantiateFire.CancelFireOFF -= LaserCancelOFF; //observer de cancelación de láser
    }
}

