using UnityEngine;

public class Player_Health : Entity_Health
{
    public override bool TakeDamaged(int damage, Transform damagedDealer)
    {
        return base.TakeDamaged(damage, damagedDealer);
    }
}
