using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;  // Funciona igual que los eventos usando accion pero es de unity. Dejo los dos metodos porque es interesante. Este metodo permite asignar al evento mediante el inspector
using System;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private List<GameObject> _canvasObjects;
    private bool _hasTriggered = false;

    [SerializeField] private UnityEvent _onTriggerEnter;
    //public event Action _onTriggerEnterEvent;


    private void Awake()
    {
        _onTriggerEnter.AddListener(TriggerDialogue);
        //_onTriggerEnterEvent += TriggerDialogue;
    }

    private void OnDestroy()
    {
        _onTriggerEnter.RemoveListener(TriggerDialogue);
        //_onTriggerEnterEvent -= TriggerDialogue;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (_hasTriggered || (_playerLayerMask.value & (1 << other.gameObject.layer)) == 0) return;
        //GameManager.Instance.SaveGame();
        _hasTriggered = true;


        _onTriggerEnter.Invoke();
        /*
        foreach (GameObject canvasItem in _canvasObjects)
        {
            if (canvasItem != null)
            {
                canvasItem.SetActive(true);
            }
        }

        FindObjectOfType<DialogueManager>().StartDialogue(_dialogueData);
        */
    }


    private void TriggerDialogue()
    {
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
