using TMPro;
using UnityEngine;

public class UI_StatTooltip : UI_ItemTooltip
{
    [SerializeField] private TextMeshProUGUI statInfo;
    private UI ui;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponentInParent<UI>();
    }

    public void ShowTooltip(bool show, RectTransform rect, StatType statType)
    {
        base.ShowTooltip(show, rect);

        statInfo.text = GetStatTextByType(statType);
    }

    public string GetStatTextByType(StatType type)
    {
        switch (type)
        {
            // Major Attributes
            case StatType.Strength:
                return "Increases physical damage by 1 per point." +
                       "\n Increases critical power by 0.5% per point.";
            case StatType.Agility:
                return "Increases critical chance by 0.3% per point." +
                       "\n Increases evasion by 0.5% per point.";
            case StatType.Intelligence:
                return "Increases elemental resistances by 0.5% per point." +
                        "\n Adds 1 elemental damage per point as a bonus. " +
                        "\n If all elements have 0 damage, the bonus will not be applied.";
            case StatType.Vitality:
                return "Increases maximum playerHealth by 5 per point" +
                       "\n Increases armor by 1 per point.";

            // Physical Damage
            case StatType.Damage:
                return "Determines the physical damage of your attacks.";
            case StatType.CriticalChance:
                return "Chance for your attacks to critically strike.";
            case StatType.CriticalDamage:
                return "Increases the damage dealt by critical strikes.";
            case StatType.ArmorReduction:
                return "Percent of armor that will be ignored by your attacks.";
            case StatType.AttackSpeed:
                return "Determines how quickly you can attack.";

            // Defense
            case StatType.MaxHealth:
                return "Determines how much total playerHealth you have.";
            case StatType.HealthRegen:
                return "Amount of playerHealth restored per second.";
            case StatType.Armor:
                return "Reduces incoming physical damage."
                    + "\n Armor mitigation is Limited at 85%."
                    + "Current mitigation is: " + ui.player.playerStats.GetArmorMitigation(0) * 100 + "%.";
            case StatType.Evasion:
                return "Chance to completely avoid attacks." + "\n Limited at 85%.";

            // Elemental Damage
            case StatType.IceDamage:
                return "Determines the ice damage of your attacks.";
            case StatType.FireDamage:
                return "Determines the fire damage of your attacks.";
            case StatType.LightningDamage:
                return "Determines the lightning damage of your attacks.";
            case StatType.ElementalDamage:
                return
                    "Elemental damage combines all three elements. " +
                    "\n The highest element applies corresponding element status effect and full damage. " +
                    "\n The other two elements contribute 50% of their damage as a bonus.";

            // Elemental Resistances
            case StatType.IceResistance:
                return "Reduces ice damage taken.";
            case StatType.FireResistance:
                return "Reduces fire damage taken.";
            case StatType.LightningResistance:
                return "Reduces lightning damage taken.";

            default:
                return "No tooltip avalible for this stat.";
        }
    }
}
