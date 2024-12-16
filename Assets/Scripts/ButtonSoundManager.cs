using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip clipToPlayButton;
    [SerializeField] private AudioClip clipChangeWeapon;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        audioSource.clip = clipToPlayButton;
        audioSource.Stop();
        audioSource.Play();
    }
    
    public void PlayButtonChangeWeapon()
    {
        audioSource.clip = clipChangeWeapon;
        audioSource.Stop();
        audioSource.Play();
    }
}
