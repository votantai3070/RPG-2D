using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats entityStats;

    public DamageScaleData basicAttackScale;

    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsDamageable;

    [Header("Elemental Info")]
    [SerializeField] private float defaultDuration = 2f;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var hit in AttackHits())
        {
            if (!hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                continue;

            AttackData attackData = entityStats.GetAttackData(basicAttackScale);
            Entity_StatusHandler handler = hit.GetComponent<Entity_StatusHandler>();
            ElementType element = attackData.element;

            float elementDamage = attackData.elementalDamage;
            int physicalDamage = (int)attackData.physicalDamage;
            bool targetGoHit = damageable.TakeDamaged(physicalDamage, elementDamage, element, transform);

            if (element != ElementType.None)
                handler?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGoHit)
            {
                hit.GetComponent<Entity>().ElementalVfx(defaultDuration, element);
                vfx.GetImapctVfx(hit.transform, attackData.isCrit);
            }
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
