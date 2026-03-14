using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public int gold = 10000;

    private Player player;
    public List<Inventory_EquipmentSlot> equipList = new();
    public Inventory_Storage storage { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
            return;

        var inventoryItem = FindItem(item.itemData);

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

        EquipItem(inventoryItem, slotToReplace);
        UnequipItem(itemToUnequip, slotToReplace != null);
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
