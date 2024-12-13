using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    [SerializeField] GameObject panel;
    public static Action OnRestart;

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void Restart()
    {
        GameManager.Instance.LoadGame();
    }
}