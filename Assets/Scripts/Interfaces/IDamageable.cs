using UnityEngine;

public interface IDamageable
{
    public bool TakeDamage(int damage, float elementDamage, ElementType elementType, Transform damageDealer);
}
