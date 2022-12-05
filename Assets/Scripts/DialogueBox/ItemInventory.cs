using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ItemInventory
{
    public Item item;
    public int amount;

    public ItemInventory(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ItemInventory(Item item)
    {
        this.item = item;
        amount = 1;
    }
}
