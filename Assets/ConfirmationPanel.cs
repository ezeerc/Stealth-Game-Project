using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private UnityEngine.UI.Button confirmButton;
    [SerializeField] private UnityEngine.UI.Button cancelButton;

    private System.Action onConfirm;

    public void Show(System.Action onConfirmAction)
    {
        gameObject.SetActive(true);
        onConfirm = onConfirmAction;
    }

    public void OnConfirm()
    {
        onConfirm?.Invoke();
        Close();
    }

    public void OnCancel()
    {
        Close();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}

