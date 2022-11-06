using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerPickUp : ItemTrigger
{
    private void Update()
    {
        if(item.itemDelivered)
        { 
            Destroy(gameObject);
            item.itemDelivered = false;
        }
    }

    public override void TriggerDialogue()
    {
        base.TriggerDialogue();
        item.itemDelivered = false;
        _dialogueBox.SetItem(item);
    }

    private void OnDestroy()
    {
        _dialogueBox.EndDialogue();
    }
}
