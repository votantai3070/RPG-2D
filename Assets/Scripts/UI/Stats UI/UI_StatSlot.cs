using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Player player;
    private Entity_Stats playerStats;
    private RectTransform rect;
    private UI ui;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statType);
        statName.text = GetStatNameByType(statType);
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();
        player = FindAnyObjectByType<Player>();
    }


    public void UpdateStatValue()
    {
        playerStats = player.playerStats;
        Stat statToUpdate = playerStats.GetStatByType(statType);

        if (statToUpdate == null)
            return;

        float value = 0;

        switch (statType)
        {
            // Major stats
            case StatType.Strength:
                value = playerStats.major.strength.GetValue();
                break;
            case StatType.Agility:
                value = playerStats.major.agility.GetValue();
                break;
            case StatType.Intelligence:
                value = playerStats.major.intelligence.GetValue();
                break;
            case StatType.Vitality:
                value = playerStats.major.vitality.GetValue();
                break;

            // Offense stats
            case StatType.Damage:
                value = playerStats.offense.damage.GetValue();
                break;
            case StatType.CriticalChance:
                value = playerStats.GetCritChance();
                break;
            case StatType.CriticalDamage:
                value = playerStats.GetCritDamage();
                break;
            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;
            case StatType.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue() * 100;
                break;

            // Defense stats
            case StatType.MaxHealth:
                value = playerStats.GetMaxHealth();
                break;
            case StatType.HealthRegen:
                value = playerStats.resource.healthRegen.GetValue();
                break;
            case StatType.Evasion:
                value = playerStats.GetEvasion();
                break;
            case StatType.Armor:
                value = playerStats.GetBaseArmor();
                break;

            // Elemental damage stats
            case StatType.IceDamage:
                value = playerStats.offense.iceDamage.GetValue();
                break;
            case StatType.FireDamage:
                value = playerStats.offense.fireDamage.GetValue();
                break;
            case StatType.LightningDamage:
                value = playerStats.offense.lightningDamage.GetValue();
                break;
            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType element, 1);
                break;

            // Elemental resistance stats
            case StatType.FireResistance:
                value = playerStats.GetElementalResistance(ElementType.Fire) * 100;
                break;
            case StatType.LightningResistance:
                value = playerStats.GetElementalResistance(ElementType.Lightning) * 100;
                break;
            case StatType.IceResistance:
                value = playerStats.GetElementalResistance(ElementType.Ice) * 100;
                break;
        }

        statValue.text = IsPercentageStat(statType) ? value + "%" : value.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";

            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";

            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Damage";
            case StatType.CriticalChance: return "Critical Chance";
            case StatType.CriticalDamage: return "Critical Damage";
            case StatType.ArmorReduction: return "Armor Reduction";

            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";
            case StatType.ElementalDamage: return "Elemental Damage";

            case StatType.FireResistance: return "Fire Resistance";
            case StatType.IceResistance: return "Ice Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
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

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.ShowTooltip(true, rect, statType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowTooltip(false, null);
    }
}
