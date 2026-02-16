using UnityEngine;

public class Player_Health : Entity_Health
{
    public override void TakeDamaged(int damage, Transform damagedDealer)
    {
        base.TakeDamaged(damage, damagedDealer);
    }
}
