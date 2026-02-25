using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var targert in EnemyAround(t, radius))
        {
            IDamageable damageable = targert.GetComponent<IDamageable>();

            if (damageable == null)
                continue;

            damageable.TakeDamaged(1, 1, ElementType.None, transform);
        }
    }

    protected Collider2D[] EnemyAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
