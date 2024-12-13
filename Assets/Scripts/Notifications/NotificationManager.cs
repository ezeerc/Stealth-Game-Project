using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }
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

    public void ScheduleStaminaFullNotification(TimeSpan timeToWait)
    {
        var notification = new AndroidNotification
        {
            Title = "¡Tu stamina está completa!",
            Text = "Ya puedes jugar con toda tu stamina disponible.",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = DateTime.Now.Add(timeToWait),
        };
        AndroidNotificationCenter.SendNotification(notification, "stamina_channel");
    }

    public void ScheduleComeBackNotification()
    {
        var notification = new AndroidNotification
        {
            Title = "¡Te extrañamos!",
            Text = "Regresa al juego, Mr. Sleep te espera.",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = DateTime.Now.AddSeconds(80),
        };
        AndroidNotificationCenter.SendNotification(notification, "stamina_channel");
    }

    public void ScheduleUpdateNotification()
    {
        var notification = new AndroidNotification
        {
            Title = "¡Nueva actualización disponible!",
            Text = "Descubre lo nuevo en el juego actualizando ahora.",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = DateTime.Now,
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
}
