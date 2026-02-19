using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [SerializeField] private Stat maxHealth;
    [SerializeField] private Stat_MajorGroup major;
    [SerializeField] private State_OffenseGroup offense;
    [SerializeField] private State_DefenseGroup defense;

    public float GetPhysicalDamage(out bool isCriticalHit)
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

        return finalDamage;
    }

    public float GetMaxHealth()
    {
        float baseHealth = maxHealth.GetValue();
        float bonusHealth = major.vitality.GetValue() * 5; // Assuming each point of vitality gives 5 additional health

        return baseHealth + bonusHealth;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f; // Assuming each point of agility gives 0.5% evasion

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 0.75f; // Cap evasion at 75%

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }


}
