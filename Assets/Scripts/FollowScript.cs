using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    private Transform _player;
    private CinemachineVirtualCamera _cam;
    private void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _cam.Follow = _player;
    }
}
