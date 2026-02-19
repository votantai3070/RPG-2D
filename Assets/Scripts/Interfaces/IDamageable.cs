using UnityEngine;

public interface IDamageable
{
    public bool TakeDamaged(int damage, float elementDamage, Transform damageDealer);
}
