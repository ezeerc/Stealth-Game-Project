using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private List<GameObject> _canvasObjects;
    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered || (_playerLayerMask.value & (1 << other.gameObject.layer)) == 0) return;

        _hasTriggered = true;

        foreach (GameObject canvasItem in _canvasObjects)
        {
            if (canvasItem != null)
            {
                canvasItem.SetActive(true);
            }
        }

        FindObjectOfType<DialogueManager>().StartDialogue(_dialogueData);
    }
}
