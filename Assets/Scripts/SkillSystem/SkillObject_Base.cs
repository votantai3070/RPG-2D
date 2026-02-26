using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkDamageRadius = 1;
    [SerializeField] protected float checkEnemyRadius = 3;

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

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in EnemyAround(transform, checkEnemyRadius))
        {

            if (enemy.GetComponent<Enemy>() == null)
                continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }

        }

        return target;
    }

    protected Collider2D[] EnemyAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkDamageRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetCheck.position, checkEnemyRadius);
    }
}
