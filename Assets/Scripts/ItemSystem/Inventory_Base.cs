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

    public bool CanAddToStack(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItem = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stack in stackableItem)
        {
            if (stack.CanStackSize())
                return true;
        }

        return false;
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
