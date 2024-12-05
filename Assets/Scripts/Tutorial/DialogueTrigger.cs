using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private LayerMask playerLayerMask;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || (playerLayerMask.value & (1 << other.gameObject.layer)) == 0) return;

        hasTriggered = true;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueData);
    }
}
