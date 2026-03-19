using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public event Action<int, Inventory_Item> OnQuickSlotUsed;
    public int gold = 10000;

    private Player player;
    public List<Inventory_EquipmentSlot> equipList = new();
    public Inventory_Storage storage { get; private set; }

    [Header("Quick Item Slots")]
    [SerializeField] private Inventory_Item[] quickItems = new Inventory_Item[2];

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void SetQuickItemInSlot(int slotNumber, Inventory_Item itemToSet)
    {
        quickItems[slotNumber - 1] = itemToSet;
        OnQuickSlotUsed?.Invoke(slotNumber - 1, itemToSet);
    }

    public void TryUseQuickItemInSlot(int passSlotNumber)
    {
        int slotNumber = passSlotNumber - 1;
        var itemToUse = quickItems[slotNumber];

        TryUseItem(itemToUse);

        if (FindItem(itemToUse) == null)
        {
            quickItems[slotNumber] = FindSameItem(itemToUse);
        }

        OnQuickSlotUsed?.Invoke(slotNumber, quickItems[slotNumber]);
    }

    public void TryEquipItem(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
            return;

        var inventoryItem = FindItem(item);

        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equipedItem;

        UnequipItem(itemToUnequip, slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slotToEquip)
    {
        float savedHealthPercent = player.health.GetHealthPercent();

        slotToEquip.equipedItem = itemToEquip;
        slotToEquip.equipedItem.AddModifier(player.playerStats);
        slotToEquip.equipedItem.AddItemEffect(player);

        player.health.SetHealthPercent(savedHealthPercent);
        RemoveOneItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false && replacingItem == false)
        {
            Debug.Log("No space");
            return;
        }

        float savedHealthPercent = player.health.GetHealthPercent();

        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if (slotToUnequip != null)
            slotToUnequip.equipedItem = null;

        itemToUnequip.RemoveModifier(player.playerStats);
        itemToUnequip.RemoveItemEffect();

        player.health.SetHealthPercent(savedHealthPercent);

        AddItem(itemToUnequip);
    }
}
