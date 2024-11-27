using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;

    private MusicOption musicOptions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindMusicOptions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FindMusicOptions()
    {
        GameObject optionsObject = GameObject.Find("MusicOptions");
        if (optionsObject != null)
        {
            musicOptions = optionsObject.GetComponent<MusicOption>();
        }
        else
        {
            print("MusicOptions no está en la escena");
        }
    }

    public void SetSliderVolume(Slider slider, string volumeParameter)
    {
        if (slider == null) return;
        float volume = Mathf.Clamp(slider.value, 0.0001f, 1f);
        audioMixer.SetFloat(volumeParameter, Mathf.Log10(volume) * 20);
    }

    public void SetMasterVolume(float value)
    {
        SetVolume("MasterVol", value);
    }

    public void SetMusicVolume(float value)
    {
        SetVolume("MusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        SetVolume("SFXVol", value);
    }

    private void SetVolume(string parameter, float value)
    {
        float clampedValue = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat(parameter, Mathf.Log10(clampedValue) * 20);
    }

    public void PlayAudio(AudioClip clip)
    {
        if (clip == null)
        {
            print("AudioClip es nulo -no está asignado-");
            return;
        }

        if (clip == audioSource.clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void PlaySameAudio()
    {
        audioSource.Play();
    }
}
