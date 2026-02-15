using UnityEngine;

public class Entity_AnimationEvent : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void AttackOver()
    {
        entity.CallAnimationEventAttackOver();
    }
}
