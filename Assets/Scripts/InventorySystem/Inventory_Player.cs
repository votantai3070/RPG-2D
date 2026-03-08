using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    public List<Inventory_EquipmentSlot> equipList = new();

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
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
        UnequipItem(itemToUnequip);
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slotToEquip)
    {
        float savedHealthPercent = player.health.GetHealthPercent();

        slotToEquip.equipedItem = itemToEquip;
        slotToEquip.equipedItem.AddModifier(player.playerStats);

        player.health.SetHealthPercent(savedHealthPercent);
        RemoveItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip)
    {
        if (CanAddItem() == false)
        {
            Debug.Log("No space");
            return;
        }

        float savedHealthPercent = player.health.GetHealthPercent();

        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if (slotToUnequip != null)
            slotToUnequip.equipedItem = null;

        player.health.SetHealthPercent(savedHealthPercent);
        itemToUnequip.RemoveModifier(player.playerStats);
        AddItem(itemToUnequip);
    }
}
