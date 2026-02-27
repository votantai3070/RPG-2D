using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkDamageRadius = 1;
    [SerializeField] protected float checkEnemyRadius = 3;

    [SerializeField] private float defaultDuration = 2f;

    protected Entity_Stats playerStats;
    protected DamageScaleData damageScale;
    protected ElementType currentElement;

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemyAround(t, radius))
        {
            Debug.Log("Target: " + target.gameObject.name);

            if (!target.CompareTag("Enemy"))
                continue;

            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null) continue;

            AttackData attackData = playerStats.GetAttackData(damageScale);
            ElementType element = attackData.element;

            int physicalDamage = (int)attackData.physicalDamage;
            int elementalDamage = (int)attackData.elementalDamage;

            bool targetGoHit = damageable.TakeDamaged(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                target.GetComponent<Entity_ElementalStateHandler>().ApplyStatusEffect(element, attackData.effectData);

            if (targetGoHit)
            {
                target.GetComponent<Entity>().ElementalVfx(defaultDuration, element);
            }

            currentElement = element;
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
