using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public override bool TakeDamaged(int damage, float elementalDamage, Transform damagedDealer)
    {

        bool wasHit = base.TakeDamaged(damage, elementalDamage, damagedDealer);

        if (!wasHit)
            return false;

        if (damagedDealer.GetComponent<Player>() != null)
            enemy.TryEnterBattleState(damagedDealer.GetComponent<Player>());

        return true;
    }
}
