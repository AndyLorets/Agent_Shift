using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private InventorySlot[] _slots;

    void Start()
    {
        _slots = GetComponentsInChildren<InventorySlot>();
        Inventory.items.Clear();
        UpdateUI();
    }

    public void AddItemToInventory(Item item)
    {
        Inventory.AddItem(item);
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < Inventory.items.Count)
            {
                _slots[i].AddItem(Inventory.items[i]);
            }
            else
            {
                _slots[i].ClearSlot();
            }
        }
    }
}
