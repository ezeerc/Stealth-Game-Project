using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeCameraScript : MonoBehaviour
{
        private CinemachineVirtualCamera _vcam;
        [SerializeField] private float secs;
        private void Start()
        {
                _vcam = GetComponent<CinemachineVirtualCamera>();
                StartCoroutine(ChangeCamera(0.5f));
        }

        IEnumerator ChangeCamera(float secs)
        {
                yield return new WaitForSecondsRealtime(secs);
                _vcam.Priority = 9;
        }
}
