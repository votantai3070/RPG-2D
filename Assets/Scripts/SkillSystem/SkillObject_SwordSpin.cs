using UnityEngine;

public class SkillObject_SwordSpin : SkillObject_Sword
{
    private float maxDistance;
    private float attackPerSecond;
    private float attackTimer;

    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        HandleComeback();
    }

    public override void SetupSword(Skill_ThrowSword swordManger, Vector2 direction)
    {
        base.SetupSword(swordManger, direction);

        anim?.SetTrigger("Spin");

        maxDistance = swordManger.maxDistance;
        attackPerSecond = swordManger.attackPerSecond;

        Invoke(nameof(GetSwordBackToPlayer), swordManger.maxSpinDuration);
    }

    private void HandleStopping()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > maxDistance && rb.simulated)
            rb.simulated = false;
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            DamageEnemiesInRadius(transform, 1);
            attackTimer = 1f / attackPerSecond;
        }

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}
