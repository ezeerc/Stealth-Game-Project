using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesObjectsTime : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;


    public void GameObjectsOff()
    {
        foreach (var _gameObject in gameObjects)
        {
            _gameObject.SetActive(false);
        }
    }

    public void GameObjectsOn()
    {
        foreach (var _gameObject in gameObjects)
        {
            _gameObject.SetActive(true);
        }
    }
}