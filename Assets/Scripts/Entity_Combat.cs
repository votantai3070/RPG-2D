using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_DamageVfx vfx;

    [SerializeField] private int damage = 10;

    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsDamageable;

    private void Awake()
    {
        vfx = GetComponent<Entity_DamageVfx>();
    }

    public void PerformAttack()
    {
        foreach (var hit in AttackHits())
        {
            if (!hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                continue;

            damageable.TakeDamaged(damage, transform);
            vfx.GetImapctVfx(hit.transform);
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
