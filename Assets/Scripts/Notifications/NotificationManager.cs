using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Notifications.Android;
using Unity.VisualScripting;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }
    private bool _update;
    private string _dateTime;

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
        StartCoroutine(NotificationPermission());
        CreateNotificationChannel();
        CancelNotifications();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) CancelNotifications();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) return;
        UpdateNotification();
    }

    private void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "stamina_channel",
            Name = "Stamina Notifications",
            Importance = Importance.High,
            Description = "Notifications about stamina and game updates.",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void ScheduleStaminaFullNotification(int timeToWait)
    {
        var time = DateTime.Now.AddSeconds(timeToWait);
        print(time);
        var notification = new AndroidNotification
        {
            Title = "¡Tu stamina está completa!",
            Text = "Ya puedes jugar con toda tu stamina disponible.",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = time,
        };
        AndroidNotificationCenter.SendNotification(notification, "stamina_channel");
    }

    public void ScheduleStaminaNotification(int timeToWait)
    {
        var time = DateTime.Now.AddSeconds(timeToWait);
        print(time);
        var notification = new AndroidNotification
        {
            Title = "¡Tienes stamina para jugar!",
            Text = "Ya tienes stamina suficiente para jugar otra partida",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = time,
        };
        AndroidNotificationCenter.SendNotification(notification, "stamina_channel");
    }

    public void SchedulePauseNotification()
    {
        var time = DateTime.Now.AddSeconds(10);
        print(time);
        var notification = new AndroidNotification
        {
            Title = "¡Soldado!",
            Text = "¡Ven y termina la misión, tu deber te llama!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = time,
        };
        AndroidNotificationCenter.SendNotification(notification, "stamina_channel");
    }

    public void ScheduleComeBackNotification()
    {
        var time = DateTime.Now.AddSeconds(20);
        print(time);
        var notification = new AndroidNotification
        {
            Title = "¡Te necesitamos!",
            Text = "Agente, ha pasado mucho tiempo sin que te reportes.",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = time,
        };
        AndroidNotificationCenter.SendNotification(notification, "stamina_channel");
    }

    public void ScheduleUpdateNotification(string timeDate)
    {
        var time = DateTime.Parse(timeDate);
        var currentTime = DateTime.Now;
        print(timeDate);

        if (currentTime >= time) return;
        var notification = new AndroidNotification
        {
            Title = "¡Nueva actualización disponible!",
            Text = "Descubre lo nuevo en el juego actualizando ahora.",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = time,
        };
        AndroidNotificationCenter.SendNotification(notification, "stamina_channel");
    }

    IEnumerator NotificationPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
        {
            yield return null;
        }
    }

    private void CancelNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }

    private void UpdateNotification()
    {
        var update = RemoteConfig.Instance.updateAvailable;
        var dateTimeUpdate = RemoteConfig.Instance.dateTimeUpdate;
        if (update) ScheduleUpdateNotification(dateTimeUpdate);
    }

}