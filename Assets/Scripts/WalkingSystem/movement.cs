using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{

    protected GameObject interaction;

    public float mSpeed;

    public CharacterController pc;

    Vector3 move;

    [SerializeField] protected List<Item> Items;
    [SerializeField] protected List<string> Quest;

    [SerializeField] protected UI_interaction _dialogueBox;

    void Update()
    {
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")) * mSpeed;

        //if (interaction != null)
        //{
        //    if (interaction.TryGetComponent(out item_int item))
        //    {
        //        if (Input.GetKeyDown(KeyCode.Space)) items.Add(item.pick_up());
        //    }
        //    else if (interaction.TryGetComponent(out Scene_Activator combat))
        //    {
        //        if (Input.GetKeyDown(KeyCode.Space)) combat.ChangeScene();
        //    }
        //}
    }

    void FixedUpdate()
    {
        pc.SimpleMove(move);
    }

    public bool Search(string item_name)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if(Items[i].name == item_name) return true;
        }

        return false;
    }

    public void setItem(Item item)
    {
        Items.Add(item);
    }

    public void setQuest(string quest)
    {
        Quest.Add(quest);
    }

    public void UseItem(string item_name)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].name == item_name)
            {
                Items.RemoveAt(i);
            }
        }
    }
}
