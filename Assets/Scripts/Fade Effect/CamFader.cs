using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class CamFader : MonoBehaviour
{
    private ObjectFader _fader1;
    private ObjectFader _fader2;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (_player != null)
        {
            Vector3 dir = _player.transform.position - transform.position;
            Ray ray = new Ray(transform.position,  dir);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity,(1 << 8 | 1<< 6)))
            {
                if (hit.collider == null)
                    return;

                if (hit.collider.gameObject == _player)
                {
                    if (_fader1 != null)
                    {
                        
                        _fader1.doFade = false;
                        _fader1 = null;
                    }

                    if (_fader2 != null)
                    {
                        _fader2.doFade = false;
                        _fader2 = null;
                    }
                }
                else if(hit.collider.gameObject != _player)
                {
                    if (_fader1 == null)
                    {
                        _fader1 = hit.collider.gameObject.GetComponent<ObjectFader>();
                        _fader1.doFade = true;
                    }
                    else if (_fader1 != null)
                    {
                        _fader1.doFade = false;
                        _fader1 = null;
                        _fader2 = hit.collider.gameObject.GetComponent<ObjectFader>();
                        _fader2.doFade = true;
                    }
                    else if (_fader1 != null && _fader2 != null)
                    {
                        _fader1.doFade = false;
                        _fader1 = hit.collider.gameObject.GetComponent<ObjectFader>();
                        _fader1.doFade = true;
                    }
                    
                    if (_fader2 == null)
                    {
                        _fader2 = hit.collider.gameObject.GetComponent<ObjectFader>();
                        _fader2.doFade = true;
                    }
                    else if (_fader2 != null && hit.collider.gameObject != _player)
                    {
                        _fader2.doFade = false;
                        _fader2 = null;
                        _fader1 = hit.collider.gameObject.GetComponent<ObjectFader>();
                        _fader1.doFade = true;
                    }
                    
                }
            }
        }
    }
}