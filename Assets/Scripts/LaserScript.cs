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
    
    private LineRenderer _lineRenderer;
    private RaycastHit hit;

    void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public void LaserOn()
    {
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
        _lineRenderer.enabled = false;
    }


}

