using UnityEngine;

public class Entity_AnimationEvent : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat combat;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        combat = GetComponentInParent<Entity_Combat>();
    }

    private void AttackOver()
    {
        entity.CallAnimationEventAttackOver();
    }

    protected virtual void Attack()
    {
        combat.PerformAttack();
    }
}
