using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats entityStat;

    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsDamageable;

    [Header("Physical Info")]
    [SerializeField] private float physicalDamageScaleFactor = 1f;

    [Header("Elemental Info")]
    [SerializeField] private float elementalDamageScaleFactor = .6f;
    [SerializeField] private float defaultDuration = 2f;
    [Space]
    [SerializeField] private float chillVfxDuration = 2f;
    [SerializeField] private float chillMultiplier = .4f;
    [Space]
    [SerializeField] private float burnVfxDuration = 1.5f;
    [Space]
    [SerializeField] private float shockDuration = 1f;
    [SerializeField] private float shockCharge = .4f;
    [SerializeField] private float shockScaleFactor = 1.5f;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        entityStat = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var hit in AttackHits())
        {
            if (!hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                continue;

            float elementDamage = entityStat.GetElementalDamage(out ElementType element, elementalDamageScaleFactor);

            int damage = Mathf.RoundToInt(entityStat.GetPhysicalDamage(out bool isCrit, physicalDamageScaleFactor));

            bool targetGoHit = damageable.TakeDamaged(damage, elementDamage, element, transform);

            //if (element == ElementType.Lightning)
            //    entityStat.offense.attackSpeed.SetValue(1.5f);
            //else
            //    entityStat.offense.attackSpeed.SetValue(1f);

            if (element != ElementType.None)
                ApplyElementalEffect(hit, element);

            if (targetGoHit)
            {
                hit.GetComponent<Entity>().ElementalVfx(defaultDuration, element);
                vfx.GetImapctVfx(hit.transform, isCrit);
            }
        }
    }

    private void ApplyElementalEffect(Collider2D hit, ElementType element)
    {
        Entity_ElementalStateHandler elementalStateHandler = hit.GetComponent<Entity_ElementalStateHandler>();

        float fireDamage = entityStat.offense.fireDamage.GetValue();
        float shockDamage = entityStat.offense.lightningDamage.GetValue();

        if (element == ElementType.Ice && elementalStateHandler.ApplyElementalVfx(ElementType.Ice))
            elementalStateHandler.ApplyChilledEffect(chillVfxDuration, chillMultiplier);

        if (element == ElementType.Fire && elementalStateHandler.ApplyElementalVfx(ElementType.Fire))
            elementalStateHandler.ApplyBurnedEffect(burnVfxDuration, fireDamage);

        if (element == ElementType.Lightning && elementalStateHandler.ApplyElementalVfx(ElementType.Lightning))
            elementalStateHandler.ApplyShockEffect(shockDuration, shockDamage, shockCharge, shockScaleFactor);
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
