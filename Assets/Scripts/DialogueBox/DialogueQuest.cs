using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQuest : MonoBehaviour
{
    [SerializeField] protected UI_interaction _dialogueBox;
    [SerializeField] protected Quest quest;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out movement player))
            _dialogueBox.SetQuest(quest);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out movement player))
        {
            Quest q = new Quest();
            q.Initialize();
            _dialogueBox.SetQuest(q);
        }
    }
}

