using UnityEngine;

public class Player_Health : Entity_Health
{
    public override bool TakeDamaged(int damage, float elementalDamage, Transform damagedDealer)
    {
        return base.TakeDamaged(damage, elementalDamage, damagedDealer);
    }
}
