using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesObjectsTime : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;

    [SerializeField] private Joystick joystick;

    private bool _oneTime = false;
    public void GameObjectsOff(int time)
    {
        foreach (var _gameObject in gameObjects)
        {
            joystick.SetCenter();
            _gameObject.SetActive(false);
        }

        StartCoroutine(WaitTimeToActivate(time));


    }

    public void GameObjectsOn()
    {
        foreach (var _gameObject in gameObjects)
        {
            _gameObject.SetActive(true);
        }
    }

    IEnumerator WaitTimeToActivate(int time)
    {
        yield return new WaitForSeconds(time);
        GameObjectsOn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_oneTime) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObjectsOff(30);
            _oneTime = true;
        }
    }
}