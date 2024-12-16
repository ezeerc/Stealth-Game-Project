using UnityEngine;
using UnityEngine.UI;
using System;

public class Button24Hours : MonoBehaviour
{
    public UnityEngine.UI.Button button; // Reference to the button in the scene
    private const string SavedDateKey = "LastActivationDate";

    void Start()
    {
        if (CanBeActivated())
        {
            ActivateButton();
        }
        else
        {
            DeactivateButton();
        }
    }
    private bool CanBeActivated()
    {
        if (!PlayerPrefs.HasKey(SavedDateKey))
        {
            return true;
        }

        string savedDate = PlayerPrefs.GetString(SavedDateKey);
        DateTime lastDate = DateTime.Parse(savedDate);
        
        return (DateTime.Now - lastDate).TotalHours >= 24;
    }
    public void ActivateButton()
    {
        button.interactable = true;
    }
    private void DeactivateButton()
    {
        button.interactable = false;
    }
    public void OnButtonClick()
    {
        PlayerPrefs.SetString(SavedDateKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
        DeactivateButton();
    }

    public void ResetData()
    {
        if (PlayerPrefs.HasKey(SavedDateKey))
        {
            PlayerPrefs.DeleteKey(SavedDateKey);
            PlayerPrefs.Save();
        }
        ActivateButton();
    }
}