using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public static StaminaSystem Instance { get; private set; }

    DateTime _nextStaminaTime;
    DateTime _lastStaminaTime;

    [SerializeField] private int _maxStamina = 10; // Valor por defecto
    [SerializeField] private int staminaPerGame = 30;
    public int _currentStamina = 10;

    [SerializeField] private float _timeToRecharge = 10; // Tiempo para recargar una unidad de stamina
    [SerializeField] private TextMeshProUGUI _staminaText;

    private bool _recharging;

    private TimeSpan notifTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadGame();
        if (_currentStamina < _maxStamina)
        {
            _recharging = true;
            StartCoroutine(ChargingStamina());
        }
    }

    IEnumerator ChargingStamina()
    {
        UpdateStamina();
        UpdateTimer();

        while (_currentStamina < _maxStamina)
        {
            DateTime current = DateTime.Now;

            while (current > _nextStaminaTime)
            {
                //if (_currentStamina >= _maxStamina) break;
                if (_currentStamina >= _maxStamina)
                {
                    _recharging = false;
                    yield break;
                }

                _currentStamina++;
                UpdateStamina();

                _nextStaminaTime = AddDuration(_nextStaminaTime, _timeToRecharge);
            }

            UpdateTimer();
            SaveGame();
            yield return new WaitForSeconds(1); // Reduce la frecuencia para optimizar
        }

        _recharging = false;
    }

    DateTime AddDuration(DateTime timeToAdd, float duration) => timeToAdd.AddSeconds(duration);

    public bool HasEnoughStamina(int stamina) => _currentStamina - stamina >= 0;

    public void UseStamina(int quantityOfUsage)
    {
        if (HasEnoughStamina(quantityOfUsage))
        {
            _currentStamina -= quantityOfUsage;
            UpdateStamina();

            if (!_recharging)
            {
                _recharging = true;
                _nextStaminaTime = AddDuration(DateTime.Now, _timeToRecharge);
                StartCoroutine(ChargingStamina());
            }
        }
        else
        {
            Debug.Log("Not enough stamina!");
        }
    }

    void UpdateTimer()
    {
        if (_currentStamina >= _maxStamina)
        {
            _nextStaminaTime = DateTime.Now; // Resetea si ya se llenó
            return;
        }

        notifTimer = _nextStaminaTime - DateTime.Now;
    }

    public void UpdateStamina()
    {
        if (_staminaText)
            _staminaText.text = $"{_currentStamina} / {_maxStamina}";
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("_currentStamina", _currentStamina);
        PlayerPrefs.SetString("_nextStaminaTime", _nextStaminaTime.ToString());
        PlayerPrefs.SetString("_lastStaminaTime", _lastStaminaTime.ToString());
    }

    void LoadGame()
    {
        _currentStamina = PlayerPrefs.GetInt("_currentStamina", _currentStamina);
        _nextStaminaTime = StringToDateTime(PlayerPrefs.GetString("_nextStaminaTime"));
        _lastStaminaTime = StringToDateTime(PlayerPrefs.GetString("_lastStaminaTime"));

        UpdateStamina();
    }

    DateTime StringToDateTime(string date)
    {
        return string.IsNullOrEmpty(date) ? DateTime.Now : DateTime.Parse(date);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveGame();

            //TimeSpan inactivityTime = DateTime.Now - _lastStaminaTime;
            //TimeSpan inactivityTime = TimeSpan.FromSeconds(60);
            //var inactivityTime = DateTime.Now.AddSeconds(60);
            //if (inactivityTime == DateTime.Now) // verificar tiempo para no volver loco al jugador
            //if (inactivityTime == DateTime.Now)

            NotificationManager notificationManager = FindObjectOfType<NotificationManager>();
            notificationManager?.SchedulePauseNotification();

            if (_currentStamina < _maxStamina)
            {
                notificationManager?.ScheduleStaminaFullNotification(CalculateTimeToStamina(_maxStamina));
            }
            
            if (_currentStamina < staminaPerGame)
            {
                notificationManager?.ScheduleStaminaNotification(CalculateTimeToStamina(staminaPerGame));
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();

        //TimeSpan inactivityTime = DateTime.Now - _lastStaminaTime;
        //TimeSpan inactivityTime = TimeSpan.FromSeconds(60);
        //var inactivityTime = DateTime.Now.AddSeconds(60);
        //if (inactivityTime.TotalSeconds >= 60) // verificar tiempo para no volver loco al jugador
        //if (inactivityTime == DateTime.Now)

        NotificationManager notificationManager = FindObjectOfType<NotificationManager>();
        notificationManager?.ScheduleComeBackNotification();

        if (_currentStamina < _maxStamina)
        {
            notificationManager?.ScheduleStaminaFullNotification(CalculateTimeToStamina(_maxStamina));
        }
        
        if (_currentStamina < staminaPerGame)
        {
            notificationManager?.ScheduleStaminaNotification(CalculateTimeToStamina(staminaPerGame));
        }
    }

    public void SetStaminaText(TextMeshProUGUI staminaText)
    {
        _staminaText = staminaText;
        UpdateStamina();
    }

    public void ResetStamina()
    {
        _currentStamina = 100;
        SaveGame();
        UpdateStamina();
    }

    public void AddStamina(int amount)
    {
        _currentStamina += amount;
    }

    private int CalculateTimeToStamina(int amount)
    {
        var timeToMaxStamina = ((amount - _currentStamina) * _timeToRecharge);
        print(timeToMaxStamina);
        return (int)timeToMaxStamina;
    }
}