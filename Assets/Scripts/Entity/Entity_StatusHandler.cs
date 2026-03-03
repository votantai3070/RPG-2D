using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_Stats entityStat;
    private Entity_Health entityHealth;
    private Entity_VFX entityVFX;

    [Header("Elemental Info")]
    [SerializeField] private ElementType currentElement;
    private float currentCharge;
    private float maxCharge = 1f;
    private Coroutine elementalEffectCo;
    [SerializeField] private float defaultDuration;
    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityStat = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
        entityVFX = GetComponent<Entity_VFX>();
    }

    private void Start()
    {
        currentElement = ElementType.None;
    }

    public void RemoveAllNegativeEffects()
    {
        StopAllCoroutines();
        currentElement = ElementType.None;
        entityVFX.StopAllCoroutines();
    }

    public void ApplyStatusEffect(ElementType element, ElementalEffectData effectData)
    {
        if (element == ElementType.Ice && CanBeApplyEffect(ElementType.Ice))
            ApplyChilledEffect(effectData.chillDuration, effectData.chillSlowMultiplier);


        if (element == ElementType.Fire && CanBeApplyEffect(ElementType.Fire))
            ApplyBurnedEffect(effectData.burnDuration, effectData.burnDamage);

        if (element == ElementType.Lightning && CanBeApplyEffect(ElementType.Lightning))
            ApplyShockEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
    }

    public void SetElement(ElementType element)
    {
        currentElement = element;
    }

    private void ApplyChilledEffect(float duration, float chillMultiplier)
    {
        entity.SlowDownEffect(duration, chillMultiplier);
    }

    private void ApplyShockEffect(float duration, float damage, float charge)
    {
        if (elementalEffectCo != null)
            StopCoroutine(elementalEffectCo);
        elementalEffectCo = StartCoroutine(HandleShockCo(duration, damage, charge));
    }

    public IEnumerator HandleShockCo(float duration, float damage, float charge)
    {
        float lightninghRes = entityStat.defense.lightninghResistance.GetValue();

        float finalDamage = damage * (1 - lightninghRes);

        SetElement(ElementType.Lightning);
        currentCharge += charge;


        if (currentCharge >= maxCharge)
        {
            entityVFX.ThunderStrikeVfx(transform);
            entityHealth.ReduceHp(Mathf.RoundToInt(finalDamage));
            currentCharge = 0f;
        }
        yield return new WaitForSeconds(duration);

        SetElement(ElementType.None);
    }

    private void ApplyBurnedEffect(float duration, float fireDamage)
    {
        if (elementalEffectCo != null)
            StopCoroutine(elementalEffectCo);
        elementalEffectCo = StartCoroutine(HandleBurnCo(duration, fireDamage));
    }

    public IEnumerator HandleBurnCo(float duration, float damage)
    {
        SetElement(ElementType.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(damage * duration);

        float damagePerTick = damage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        float fireRes = entityStat.defense.fireResistance.GetValue();

        float finalDamage = damagePerTick * (1 - fireRes);

        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHp(Mathf.RoundToInt(finalDamage));
            yield return new WaitForSeconds(tickInterval);
        }

        SetElement(ElementType.None);
    }

    public bool CanBeApplyEffect(ElementType element)
    {
        if (currentElement == element)
            return false;

        return currentElement == ElementType.None;
    }
}
