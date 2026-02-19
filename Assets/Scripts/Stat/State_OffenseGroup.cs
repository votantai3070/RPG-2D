using System;

[Serializable]
public class State_OffenseGroup
{
    // Physical Offense
    public Stat damage;
    public Stat critDamage;
    public Stat critChance;
    public Stat armorReduction;

    // Elemental Offense
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
