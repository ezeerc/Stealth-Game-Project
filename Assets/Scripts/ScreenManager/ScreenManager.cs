using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

    private Dictionary<string, IScreen> _screens = new Dictionary<string, IScreen>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RegisterScreen(string screenName, IScreen screen)
    {
        if (!_screens.ContainsKey(screenName))
        {
            _screens.Add(screenName, screen);
        }
    }

    public void ShowScreen(string screenName)
    {
        if (_screens.TryGetValue(screenName, out IScreen screen))
        {
            screen.Show();
        }
    }

    public void HideScreen(string screenName)
    {
        if (_screens.TryGetValue(screenName, out IScreen screen))
        {
            screen.Hide();
        }
    }
}