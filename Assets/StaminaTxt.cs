using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaminaTxt : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _staminaText;
    void Start()
    {
        StaminaSystem.Instance.SetStaminaText(_staminaText);
    }
}
