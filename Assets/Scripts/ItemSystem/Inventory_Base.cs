using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnInventoryChange;

    public List<Inventory_Item> itemList = new();

    private int maxInventorySlots = 12;

    protected virtual void Awake()
    {
    }

    public bool CanAddItem() => itemList.Count <= maxInventorySlots;

    public void AddItem(Inventory_Item item)
    {
        Inventory_Item itemInInventory = FindItem(item.itemData);

        if (itemInInventory != null && itemInInventory.CanStackSize())
            itemInInventory.AddStack();
        else
            itemList.Add(item);

        OnInventoryChange?.Invoke();
    }

    public void RemoveItem(Inventory_Item item)
    {
        itemList.Remove(FindItem(item.itemData));

        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(ItemDataSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }
}
