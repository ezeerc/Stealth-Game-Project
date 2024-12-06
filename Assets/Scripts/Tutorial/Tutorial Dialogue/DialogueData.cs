using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDialogue", menuName = "ScriptableObjects/Tutorial Dialogue")]
public class DialogueData : ScriptableObject
{
    public List<string> dialogueIDs;
}
