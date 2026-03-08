using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public void ShowTooltip(bool show, RectTransform target, Inventory_Item itemToShow)
    {
        base.ShowTooltip(show, target);

        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }

    private string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
            return "Used for crafting.";

        if (item.itemData.itemType == ItemType.Consumable)
            return item.itemData.itemEffect.effectionDiscription;

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        foreach (var modifier in item.itemModifiers)
        {
            string modType = GetStatNameByType(modifier.statType);
            string modValue = IsPercentageStat(modifier.statType) ? modifier.value.ToString() + "%" : modifier.value.ToString();
            sb.Append("+ " + modValue + " " + modType + "\n");
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
