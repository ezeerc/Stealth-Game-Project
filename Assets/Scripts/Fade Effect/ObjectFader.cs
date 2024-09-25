using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    private float _originalOpacity;
    private Material[] Mats;
    public bool doFade = false;

    private void Start()
    {
        Mats = GetComponent<Renderer>().materials;
        foreach (Material mat in Mats)
        {
            _originalOpacity = mat.color.a;
        }
    }

    private void Update()
    {
        if (doFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }

    void FadeNow()
    {
        foreach (Material mat in Mats)
        {
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
        }
    }
    
    void ResetFade()
    {
        foreach (Material mat in Mats)
        {
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, _originalOpacity, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
        }
    }
}