using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    DateTime _nextStaminaTime;
    DateTime _lastStaminaTime;

    [SerializeField] private int _maxStamina;
    [SerializeField] private  int _currentStamina = 10;

    [SerializeField] private float _timeToRecharge = 10;
    [SerializeField] private TextMeshProUGUI _staminaText;
    
    bool _recharging;

    private TimeSpan notifTimer;

    private void Start()
    {
        LoadGame();
        StartCoroutine(ChargingStamina());
    }

    IEnumerator ChargingStamina()
    {
        UpdateStamina();
        UpdateTimer();
        _recharging = true;

        while (_currentStamina < _maxStamina)
        {
            DateTime current = DateTime.Now;
            DateTime nextTime = _nextStaminaTime;

            bool addingStamina = false;

            while (current > nextTime)
            {
                if (_currentStamina < _maxStamina) break;
                
                _currentStamina++;
                addingStamina = true;
                UpdateStamina();

                DateTime timeToAdd = nextTime;

                if (_lastStaminaTime > nextTime) timeToAdd = _lastStaminaTime;
                
                nextTime = AddDuration(timeToAdd, _timeToRecharge);

            }

            if (addingStamina)
            {
                _nextStaminaTime = nextTime;
                _lastStaminaTime = DateTime.Now;
            }

            UpdateTimer();
            UpdateStamina();
            SaveGame();
            
            yield return new WaitForEndOfFrame();
        }
        
        _recharging = false;
    }
    
    DateTime AddDuration(DateTime timeToAdd, float duration) => timeToAdd.AddSeconds(duration);
    
    public bool HasEnoughStamina(int stamina) => _currentStamina - stamina >= 0;

    public void UseStamina(int quanityOfUsage)
    {
        if(HasEnoughStamina(quanityOfUsage))
        {
            _currentStamina -= quanityOfUsage;
            UpdateStamina();
            if (!_recharging)
            {
                _nextStaminaTime = AddDuration(DateTime.Now, _timeToRecharge);
                StartCoroutine(ChargingStamina());
            }
        }
        else
        {
            Debug.Log("no stamina!");
        }
    }


    void UpdateTimer()
    {
        if (_currentStamina >= _maxStamina)
        {
            return;
        }
        
        notifTimer= _nextStaminaTime - DateTime.Now;
        
    }

    void UpdateStamina()
    {
        _staminaText.text = $"{_currentStamina} / {_maxStamina}";
    }

    void SaveGame()
    {
        PlayerPrefs.SetInt("_currentStamina", _currentStamina);
        PlayerPrefs.SetString("_nextStamina", _nextStaminaTime.ToString());
        PlayerPrefs.SetString("_lastStaminaTime", _lastStaminaTime.ToString());
    }

    void LoadGame()
    {
        _currentStamina = PlayerPrefs.GetInt("_currentStamina", _maxStamina);
        _nextStaminaTime = StringToDateTime(PlayerPrefs.GetString("_nextStaminaTime"));
        _lastStaminaTime = StringToDateTime(PlayerPrefs.GetString("_lastStaminaTime"));
        
        UpdateStamina();
    }

    DateTime StringToDateTime(string date)
    {
        if(string.IsNullOrEmpty(date)) 
            return DateTime.Now;
        else 
            return DateTime.Parse(date);
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus) SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause) SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
