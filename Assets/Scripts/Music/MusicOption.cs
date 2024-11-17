using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MusicOption : MonoBehaviour
{
    private MusicManager musicManager;

    [Header("UI Music")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private float initialMasterVolume = 0.5f;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private float initialMusicVolume = 0.5f;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private float initialSFXVolume = 1f;

    private float lastMasterVolume, lastMusicVolume, lastSFXVolume;

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
        if (musicManager == null)
        {
            print("MusicManager no está en la escena");
        }
    }

    private void Start()
    {
        InitializeSliders();
    }

    private void Update()
    {
        UpdateSliders();
    }

    private void InitializeSliders()
    {
        if (masterSlider != null)
        {
            masterSlider.value = initialMasterVolume;
            lastMasterVolume = masterSlider.value;
        }
        if (musicSlider != null)
        {
            musicSlider.value = initialMusicVolume;
            lastMusicVolume = musicSlider.value;
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = initialSFXVolume;
            lastSFXVolume = sfxSlider.value;
        }
    }

    private void UpdateSliders()
    {
        if (masterSlider != null && masterSlider.value != lastMasterVolume)
        {
            musicManager.SetMasterVolume(masterSlider.value);
            lastMasterVolume = masterSlider.value;
        }

        if (musicSlider != null && musicSlider.value != lastMusicVolume)
        {
            musicManager.SetMusicVolume(musicSlider.value);
            lastMusicVolume = musicSlider.value;
        }

        if (sfxSlider != null && sfxSlider.value != lastSFXVolume)
        {
            musicManager.SetSFXVolume(sfxSlider.value);
            lastSFXVolume = sfxSlider.value;
        }
    }
}

