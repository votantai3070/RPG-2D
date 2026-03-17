using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnInventoryChange;

    public List<Inventory_Item> itemList = new();

    public int maxInventorySlots = 12;

    protected virtual void Awake()
    {
    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);

        if (consumable == null)
            return;

        consumable.itemEffect.ExecuteEffect();

        if (consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);

        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindStackableItem(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanStackSize())
                return stackableItem;
        }

        return null;
    }

    public void RemoveFullStack(Inventory_Item item)
    {
        for (int i = 0; i < item.stackSize; i++)
        {
            RemoveOneItem(item);
        }
    }

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = FindStackableItem(itemToAdd) != null;
        return hasStackable || itemList.Count < maxInventorySlots;
    }


    public void AddItem(Inventory_Item item)
    {
        Inventory_Item itemInInventory = FindItem(item);

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(item);

        OnInventoryChange?.Invoke();
    }

    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        Inventory_Item itemRemove = FindItem(itemToRemove);

        if (itemRemove.stackSize > 1)
            itemRemove.RemoveStack();
        else
            itemList.Remove(itemToRemove);

        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item == itemToFind);
    }

    public void TriggerUpdateUI()
    {
        OnInventoryChange?.Invoke();
    }
}
