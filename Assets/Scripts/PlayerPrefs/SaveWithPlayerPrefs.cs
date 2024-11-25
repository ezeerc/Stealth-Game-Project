using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveWithPlayerPrefs : MonoBehaviour
{
    [SerializeField] private int _currency = 10;
    [SerializeField] private float _life = 100;
    [SerializeField] private string _playerName = "Zeke";
    [SerializeField] private TextMeshProUGUI[] _textShowingStats;

    private void Awake()
    {
        LoadGame();
    }

    private void Update()
    {
        _textShowingStats[0].text = $"Currency: {_currency}";
        _textShowingStats[1].text = $"Life: {_life}";
        _textShowingStats[2].text = $"Name: {_playerName}";
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.currencyKey, _currency);
        PlayerPrefs.SetFloat("Data_Life", _life);
        PlayerPrefs.SetString("Data_PlayerName", _playerName);
        PlayerPrefs.Save();
        Debug.Log("Game Saved");
    }

    private void LoadGame()
    {
        _currency = PlayerPrefs.GetInt(PlayerPrefsKey.currencyKey, PlayerPrefsKey.value);
        _life = PlayerPrefs.GetFloat("Data_Life", 100);
        _playerName = PlayerPrefs.GetString("Data_PlayerName", "zeke");
        Debug.Log("Game Loaded");
    }

    private void DeleteGame()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Game Deleted");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus) SaveGame();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus) SaveGame();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
