using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    // Start is called before the first frame update
    void Start()
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
