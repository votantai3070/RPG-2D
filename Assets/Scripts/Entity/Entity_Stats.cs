using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SO defaultStatSetup;

    public Stat_ResourceGroup resource;
    public Stat_MajorGroup major;
    public State_OffenseGroup offense;
    public State_DefenseGroup defense;

    public float GetElementalDamage(out ElementType element, float scaleFactor)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();
        float elementalDamageBonus = major.intelligence.GetValue() * 1f; // Assuming each point of INT gives 1 additional elemental damage

        float hightestElementalDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > hightestElementalDamage)
        {
            hightestElementalDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > hightestElementalDamage)
        {
            hightestElementalDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        if (hightestElementalDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = element == ElementType.Fire ? 0 : fireDamage * 0.5f; // Weaker elements contribute 50% of their damage as bonus
        float bonusIce = element == ElementType.Ice ? 0 : iceDamage * 0.5f;
        float bonusLightning = element == ElementType.Lightning ? 0 : lightningDamage * 0.5f;

        float weakerElementalDamageBonus = bonusFire + bonusIce + bonusLightning;

        float totalElementalDamage = hightestElementalDamage + weakerElementalDamageBonus + elementalDamageBonus;

        return totalElementalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0f;
        float bonusResistance = major.intelligence.GetValue() * 0.5f; // Assuming each point of INT gives 0.5% additional resistance

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireResistance.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceResistance.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightninghResistance.GetValue();
                break;
            default:
                break;
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 0.85f; // Cap elemental resistance at 85%

        float finalResistance = Mathf.Clamp(resistance / 100, 0, resistanceCap); // Convert percentage to decimal and clamp

        return finalResistance;
    }

    public float GetArmorMigitation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue(); // Assuming each point of vitality gives 1 additional armor
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1); // Apply armor reduction to the total armor
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100); // Percentage damage reduction formula
        float mitigationCap = 0.85f; // Cap mitigation at 85%

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        float finalArmorReduction = offense.armorReduction.GetValue() / 100; // Convert percentage to decimal

        return finalArmorReduction;
    }

    public float GetPhysicalDamage(out bool isCriticalHit, float scaleFactor)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f; // Assuming each point of AGI gives 0.3% additional crit chance
        float totalCritChance = baseCritChance + bonusCritChance;

        float baseCritDamage = offense.critDamage.GetValue();
        float bonusCritDamage = major.strength.GetValue() * 0.5f; // Assuming each point of STR gives 0.5% additional crit damage
        float critDamage = (baseCritDamage + bonusCritDamage) / 100;

        isCriticalHit = Random.Range(0, 100) < totalCritChance;
        float finalDamage = isCriticalHit ? totalBaseDamage * critDamage : totalBaseDamage;

        return finalDamage * scaleFactor;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f; // Assuming each point of agility gives 0.5% evasion

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 0.85f; // Cap evasion at 85%

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = resource.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5; // Assuming each point of vitality gives 5 additional health
        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                return resource.maxHealth;
            case StatType.HealthRegen:
                return resource.healthRegen;
            case StatType.Strength:
                return major.strength;
            case StatType.Agility:
                return major.agility;
            case StatType.Intelligence:
                return major.intelligence;
            case StatType.Vitality:
                return major.vitality;
            case StatType.Armor:
                return defense.armor;
            case StatType.Evasion:
                return defense.evasion;
            case StatType.FireResistance:
                return defense.fireResistance;
            case StatType.IceResistance:
                return defense.iceResistance;
            case StatType.LightningResistance:
                return defense.lightninghResistance;
            case StatType.Damage:
                return offense.damage;
            case StatType.CriticalChance:
                return offense.critChance;
            case StatType.CriticalDamage:
                return offense.critDamage;
            case StatType.AttackSpeed:
                return offense.attackSpeed;
            case StatType.ArmorReduction:
                return offense.armorReduction;
            case StatType.FireDamage:
                return offense.fireDamage;
            case StatType.IceDamage:
                return offense.iceDamage;
            case StatType.LightningDamage:
                return offense.lightningDamage;
            default:
                Debug.LogWarning("Stat type not found: " + type);
                return null;
        }
    }

    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.LogWarning("Default stat setup is not assigned.");
            return;
        }

        // Default resource stats
        resource.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resource.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        // Default offense stats
        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critDamage.SetBaseValue(defaultStatSetup.critDamage);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);
        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        // Default defense stats
        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);
        defense.fireResistance.SetBaseValue(defaultStatSetup.fireResistance);
        defense.iceResistance.SetBaseValue(defaultStatSetup.iceResistance);
        defense.lightninghResistance.SetBaseValue(defaultStatSetup.lightningResistance);

        // Default major stats
        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);
    }
}
