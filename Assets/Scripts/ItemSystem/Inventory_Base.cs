using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnInventoryChange;

    public List<Inventory_Item> itemList = new();
    public UI_Inventory inventoryUI;

    private int maxInventorySlots = 10;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<UI_Inventory>();

        maxInventorySlots = inventoryUI.itemSlots.Length;
    }

    public bool CanAddItem() => itemList.Count <= maxInventorySlots;

    public void AddItem(Inventory_Item item)
    {
        Inventory_Item itemInInventory = FindItem(item.itemData);

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(item);

        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(ItemDataSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData && item.CanStackSize());
    }
}
