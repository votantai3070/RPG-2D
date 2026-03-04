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
        itemList.Add(item);
        OnInventoryChange?.Invoke();
    }
}
