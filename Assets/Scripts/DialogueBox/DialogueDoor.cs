using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDoor : MonoBehaviour
{
    [SerializeField] protected UI_interaction _dialogueBox;
    [SerializeField] protected Door door;
    [SerializeField] protected Sentence sentence;

    public void TriggerDialogue()
    {
        _dialogueBox.StartDialogue(sentence);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out movement player))
        {
            _dialogueBox.SetDoor(door);
            _dialogueBox.AppearText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out movement player))
        {
            _dialogueBox.HidenText();
            _dialogueBox.EndDialogue();
        }
    }
}
