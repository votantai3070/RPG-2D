using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;

    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsDamageable;

    [Header("Physical Info")]
    [SerializeField] private float physicalDamageScaleFactor = 1f;

    [Header("Elemental Info")]
    [SerializeField] private float elementalDamageScaleFactor = .6f;
    [SerializeField] private float chillVfxDuration = 2f;
    [SerializeField] private float burnVfxDuration = 1.5f;
    [SerializeField] private float chillMultiplier = .4f;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var hit in AttackHits())
        {
            if (!hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                continue;

            float elementDamage = stats.GetElementalDamage(out ElementType element, elementalDamageScaleFactor);

            int damage = Mathf.RoundToInt(stats.GetPhysicalDamage(out bool isCrit, physicalDamageScaleFactor));

            bool targetGoHit = damageable.TakeDamaged(damage, elementDamage, element, transform);

            if (element != ElementType.None)
                ApplyElementalEffect(hit, element);

            if (targetGoHit)
            {
                hit.GetComponent<Entity>().ElementalVfx(chillVfxDuration, element);
                vfx.GetImapctVfx(hit.transform, isCrit);
            }
        }
    }

    private void ApplyElementalEffect(Collider2D hit, ElementType element)
    {
        Entity_ElementalStateHandler elementalStateHandler = hit.GetComponent<Entity_ElementalStateHandler>();

        float fireDamage = stats.offense.fireDamage.GetValue();
        //float shockDamage = stats.offense.lightningDamage.GetValue();

        if (element == ElementType.Ice && elementalStateHandler.ApplyElementalVfx(ElementType.Ice))
            elementalStateHandler.ApplyChilledEffect(chillVfxDuration, chillMultiplier);

        if (element == ElementType.Fire && elementalStateHandler.ApplyElementalVfx(ElementType.Fire))
            elementalStateHandler.ApplyBurnedEffect(burnVfxDuration, fireDamage);

        //if (element == ElementType.Lightning && elementalStateHandler.ApplyElementalVfx(ElementType.Lightning))
        //    elementalStateHandler.ApplyShockEffect(vfxDuration, chillMultiplier);
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
