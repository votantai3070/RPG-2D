using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamaged(int damage, Transform damagedDealer)
    {
        enemy.TryEnterBattleState(damagedDealer.GetComponent<Player>());

        base.TakeDamaged(damage, damagedDealer);
    }
}
