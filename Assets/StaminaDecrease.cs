using UnityEngine;

public class StaminaDecrease : MonoBehaviour
{
    //[SerializeField] private UnityEngine.UI.Button staminaDecreaseButton;
    [SerializeField] private int staminaDecrease;

    public void OnButtonClick()
    {
        StaminaSystem.Instance.UseStamina(staminaDecrease);
    }
}
