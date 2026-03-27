using UnityEngine;

public class Player_Health : Entity_Health
{
    public override bool TakeDamage(int damage, float elementalDamage, ElementType elementType, Transform damagedDealer)
    {
        return base.TakeDamage(damage, elementalDamage, elementType, damagedDealer);
    }

    protected override void Die()
    {
        base.Die();
    }
}
