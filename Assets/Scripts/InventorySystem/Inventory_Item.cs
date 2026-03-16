using System;
using System.Text;

[Serializable]
public class Inventory_Item
{
    private string itemID;

    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] itemModifiers { get; private set; }
    public ItemEffectDataSO itemEffect { get; private set; }

    public int buyPrice { get; private set; }
    public float sellPrice { get; private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemEffect = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice * .35f;

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

    public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();

        if (itemData.itemType == ItemType.Material)
        {
            sb.Append("");
            sb.Append("Used for crafting");
            sb.Append("");
            sb.Append("");
            return sb.ToString();
        }

        if (itemData.itemType == ItemType.Consumable)
        {
            sb.Append("");
            sb.Append(itemData.itemEffect.effectionDiscription);
            sb.Append("");
            sb.Append("");
            return sb.ToString();
        }


        sb.AppendLine("");

        foreach (var modifier in itemModifiers)
        {
            string modType = GetStatNameByType(modifier.statType);
            string modValue = IsPercentageStat(modifier.statType) ? modifier.value.ToString() + "%" : modifier.value.ToString();
            sb.Append("+ " + modValue + " " + modType + "\n");
        }

        if (itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique effect: ");
            sb.AppendLine(itemEffect.effectionDiscription);
        }

        return sb.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";
            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Damage";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";
            case StatType.CriticalChance: return "Critical Chance";
            case StatType.CriticalDamage: return "Critical Damage";
            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.IceResistance: return "Ice Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            case StatType.ArmorReduction: return "Armor Reduction";
            default: return "Unknow Stat";
        }
    }

    private bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.CriticalChance:
            case StatType.CriticalDamage:
            case StatType.FireResistance:
            case StatType.IceResistance:
            case StatType.LightningResistance:
            case StatType.ArmorReduction:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;

            default: return false;
        }
    }
}
