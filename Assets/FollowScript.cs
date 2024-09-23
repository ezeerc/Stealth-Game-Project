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
        StartCoroutine(WaitTime(0.1f));
    }

    IEnumerator WaitTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _cam.Follow = _player;
    }
}
