using UnityEngine;

public class Entity_AnimationEvent : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat combat;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        combat = GetComponentInParent<Entity_Combat>();
    }

    private void AttackOver()
    {
        entity.CallAnimationEventAttackOver();
    }

    private void Attack()
    {
        combat.PerformAttack();

    }
}
