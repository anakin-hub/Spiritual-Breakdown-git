using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : DialogueTrigger
{
    [SerializeField] protected Item item;

    public override void TriggerDialogue()
    {
        item.itemDelivered = false;
        base.TriggerDialogue();
        _dialogueBox.SetItem(item);
    }
}

