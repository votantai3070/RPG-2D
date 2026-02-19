using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_DamageVfx vfx;
    private Entity_Stats stats;

    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsDamageable;

    private void Awake()
    {
        vfx = GetComponent<Entity_DamageVfx>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var hit in AttackHits())
        {
            if (!hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                continue;

            float elementDamage = stats.GetElementalDamage(out ElementType element);
            int damage = Mathf.RoundToInt(stats.GetPhysicalDamage(out bool isCrit));

            bool targetGoHit = damageable.TakeDamaged(damage, elementDamage, element, transform);

            if (targetGoHit)
                vfx.GetImapctVfx(hit.transform, isCrit);
        }
    }

    protected Collider2D[] AttackHits()
    {
        return Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsDamageable);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
