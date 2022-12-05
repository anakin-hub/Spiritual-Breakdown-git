using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] protected TMP_Text _name;
    [SerializeField] protected TMP_Text _amount;
    [SerializeField] protected Inventory _inventory;
    public string _itemName;
    Button button;



    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Use);
    }

    public void Set(string name, int amount)
    {
        _itemName = name;
        _name.text = _itemName;
        _amount.text = amount.ToString();
    }

    void Use()
    {
        _inventory.use(_itemName);
        Debug.Log("FUNCIONA!");
    }
}
