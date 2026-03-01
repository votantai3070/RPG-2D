using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_ThrowSword skillSwordManager;

    protected bool shouldComeback;
    protected float combackSpeed = 20;
    protected float maxAllowedDistance = 20;

    protected virtual void Update()
    {
        transform.right = rb.linearVelocity;

        HandleComeback();
    }

    public virtual void GetSwordBackToPlayer() => shouldComeback = true;

    public virtual void SetupSword(Skill_ThrowSword swordManger, Vector2 direction)
    {
        rb.linearVelocity = direction;

        skillSwordManager = swordManger;

        player = swordManger.player;

        playerStats = swordManger.player.entityStats;
        damageScale = swordManger.damageScaleData;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemiesInRadius(transform, 1);

    }

    protected void HandleComeback()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > maxAllowedDistance)
            GetSwordBackToPlayer();

        if (!shouldComeback)
            return;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, combackSpeed * Time.deltaTime);

        if (distance < .5f)
            Destroy(gameObject);
    }

    protected void StopSword(Collider2D collider)
    {
        rb.simulated = false;
        transform.parent = collider.transform;
    }
}
