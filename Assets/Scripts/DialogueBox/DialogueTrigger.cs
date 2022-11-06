using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] protected UI_interaction _dialogueBox;
    [SerializeField] protected Dialogue dialogue;

    public virtual void TriggerDialogue()
    {          
        _dialogueBox.StartDialogue(dialogue);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out movement player))
        {
            _dialogueBox.AppearText();
            TriggerDialogue();
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out movement player))
        {
            _dialogueBox.HidenText();
            _dialogueBox.EndDialogue();
        }
    }
}