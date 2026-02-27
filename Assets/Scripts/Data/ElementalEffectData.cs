using UnityEngine;

public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowMultiplier;
    [Space]
    public float burnDuration;
    public float burnDamage;
    [Space]
    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public ElementalEffectData(Entity_Stats entityStats, DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillSlowMultiplier = damageScale.chillSlowMultiplier;

        burnDuration = damageScale.burnDuration;
        burnDamage = entityStats.offense.fireDamage.GetValue() * damageScale.burnDamageScale;

        shockCharge = damageScale.shockCharge;
        shockDamage = entityStats.offense.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockDuration = damageScale.shockDuration;
    }
}
