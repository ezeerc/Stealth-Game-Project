using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private UnityEngine.UI.Button continueButton;

    private Queue<string> dialogueQueue;
    private System.Action onDialogueEnd;

    private void Start()
    {
        dialogueQueue = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue (DialogueData dialogue, System.Action onComplete = null)
    {
        Time.timeScale = 0f;
        dialogueQueue.Clear();

        foreach (string id in dialogue.dialogueIDs)
        {
            dialogueQueue.Enqueue(id);
        }

        onDialogueEnd = onComplete;
        dialoguePanel.SetActive(true);
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string currentID = dialogueQueue.Dequeue();
        dialogueText.text = LocalizationManager.Instance.GetTranslation(currentID);
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
        onDialogueEnd?.Invoke();
    }
}
