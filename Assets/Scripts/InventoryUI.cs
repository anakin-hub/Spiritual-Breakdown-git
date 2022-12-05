using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] protected Inventory _inventory;
    List<ItemInventory> itens;
    public RectTransform position;
    [SerializeField] protected GameObject ItemUI;

    void Start()
    {
        itens = new List<ItemInventory>();
        itens = _inventory.GetItens();
        DrawMenu();
    }

    void Update()
    {
        if(_inventory.update)
        {
            itens = _inventory.GetItens();
            DrawMenu();
            _inventory.update = false;
        }
    }

    void DrawMenu()
    {
        Vector3 _position = position.position;
        foreach (Transform child in position)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemInventory item in itens)
        {
            ItemUI.GetComponent<InventoryButton>().Set(item.item.name, item.amount);
            GameObject UIitem = Instantiate(ItemUI, position.transform);
            UIitem.transform.SetParent(position.transform, false);
            UIitem.transform.position = _position;
            _position.y -= 60;
        }
    }
}
