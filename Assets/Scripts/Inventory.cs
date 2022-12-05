using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] protected List<ItemInventory> Itens;
    public bool update;

    private void Start()
    {
        update = false;
    }

    public List<ItemInventory> GetItens()
    { return Itens; }

    public void add(Item item)
    {
        if (!VerifyInventory(item, false))
        {
            ItemInventory i = new ItemInventory(item, 1);
            Itens.Add(i);
        }
        update = true;
    }

    public void use(Item item)
    {
        if (!VerifyInventory(item, true))
        {
            Debug.Log("VAZIO");
        }
        update = true;
    }

    public void use(string item)
    {
        for(int i = 0; i < Itens.Count; i++)
        {
            if (item == Itens[i].item.name)
                use(Itens[i].item);
        }
        update = true;
    }

    bool VerifyInventory(Item item, bool remove)//if remove false, in case of function true +1 on item, if remove true, in case of function true -1 on item
    {
        for (int i = 0; i < Itens.Count; i++)
        {
            if(item.name == Itens[i].item.name)
            {
                if (remove)
                {
                    if (Itens[i].amount > 1)
                        Itens[i].amount--;
                    else
                    {
                        Itens.RemoveAt(i);
                    }
                }
                else
                    Itens[i].amount++;



                return true;
            }
        }

        return false;
    }

    public bool Search(Item item)
    {
        for (int i = 0; i < Itens.Count; i++)
        {
            if (item.name == Itens[i].item.name)
            {
               return true;
            }
        }

        return false;
    }

    public bool Search(string item)
    {
        for (int i = 0; i < Itens.Count; i++)
        {
            if (item == Itens[i].item.name)
            {
                return true;
            }
        }

        return false;
    }
}
