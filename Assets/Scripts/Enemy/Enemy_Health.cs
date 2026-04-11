using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;
    private Player_QuestManager questManager;

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();

        questManager = Player.instance.questManager;
    }

    public override bool TakeDamage(int damage, float elementalDamage, ElementType elementType, Transform damagedDealer)
    {
        if (canTakeDamage == false)
            return false;

        bool wasHit = base.TakeDamage(damage, elementalDamage, elementType, damagedDealer);

        if (!wasHit)
            return false;

        if (damagedDealer.GetComponent<Player>() != null)
            enemy.TryEnterBattleState(damagedDealer.GetComponent<Player>());

        return true;
    }

    protected override void Die()
    {
        base.Die();

        questManager.AddProgrres(enemy.questTargetId);
    }
}
