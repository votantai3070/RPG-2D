using UnityEngine;

public class Player_Health : Entity_Health
{
    public override bool TakeDamaged(int damage, float elementalDamage, ElementType elementType, Transform damagedDealer)
    {
        return base.TakeDamaged(damage, elementalDamage, elementType, damagedDealer);
    }
}
