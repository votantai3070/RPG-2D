using UnityEngine;

public class SkillObject_SwordPierce : SkillObject_Sword
{
    private int amountPierce;

    public override void SetupSword(Skill_ThrowSword swordManger, Vector2 direction)
    {
        base.SetupSword(swordManger, direction);
        amountPierce = swordManger.pierceAmount;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (amountPierce <= 0 || groundHit)
        {
            DamageEnemiesInRadius(transform, .3f);
            StopSword(collision);
            return;
        }

        amountPierce--;
        DamageEnemiesInRadius(transform, .3f);
    }
}
