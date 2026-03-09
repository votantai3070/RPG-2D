using System;

[Serializable]
public class Inventory_Item
{
    private string itemID;

    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] itemModifiers { get; private set; }
    public ItemEffectDataSO itemEffect { get; private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemEffect = itemData.itemEffect;

        itemModifiers = EquipmentData()?.modifiers;
        itemID = itemData.itemName + " - " + Guid.NewGuid();
    }

    private EquipmentDataSO EquipmentData()
    {
        if (itemData is EquipmentDataSO equipment)
            return equipment;

        return null;
    }

    public void AddModifier(Entity_Stats playerStats)
    {
        foreach (var modifier in itemModifiers)
        {
            Stat statToModifier = playerStats.GetStatByType(modifier.statType);
            statToModifier.AddModifier(modifier.value, itemID);
        }
    }

    public void RemoveModifier(Entity_Stats playerStats)
    {
        foreach (var modifier in itemModifiers)
        {
            Stat statToModifier = playerStats.GetStatByType(modifier.statType);
            statToModifier.RemoveModifier(itemID);
        }
    }

    public void AddItemEffect(Player player) => itemEffect?.Subcribe(player);
    public void RemoveItemEffect() => itemEffect?.Unsubscribe();
    public bool CanStackSize() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
