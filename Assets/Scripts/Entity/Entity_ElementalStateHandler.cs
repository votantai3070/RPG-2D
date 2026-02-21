using System.Collections;
using UnityEngine;

public class Entity_ElementalStateHandler : MonoBehaviour
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

    public void SetElement(ElementType element)
    {
        currentElement = element;
    }

    public void ApplyChilledEffect(float duration, float chillMultiplier)
    {
        entity.EnterChillEffect(duration, chillMultiplier);
    }

    public void ApplyShockEffect(float duration, float damage, float charge, float shockScaleFactor)
    {
        if (elementalEffectCo != null)
            StopCoroutine(elementalEffectCo);
        elementalEffectCo = StartCoroutine(HandleShockCo(duration, damage, charge, shockScaleFactor));
    }

    public IEnumerator HandleShockCo(float duration, float damage, float charge, float shockScaleFactor)
    {
        float lightninghRes = entityStat.defense.lightninghResistance.GetValue();

        float finalDamage = damage * (1 - lightninghRes);

        SetElement(ElementType.Lightning);
        currentCharge += charge;

        if (currentCharge >= maxCharge)
        {
            currentCharge = 0f;
            entityVFX.ThunderStrikeVfx(transform);
            entityHealth.ReduceHp(Mathf.RoundToInt(finalDamage * shockScaleFactor));
        }
        yield return new WaitForSeconds(duration);

        SetElement(ElementType.None);
    }

    public void ApplyBurnedEffect(float duration, float fireDamage, float scaleFactor = 1)
    {
        if (elementalEffectCo != null)
            StopCoroutine(elementalEffectCo);
        elementalEffectCo = StartCoroutine(HandleBurnCo(duration, fireDamage * scaleFactor));
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

    public bool ApplyElementalVfx(ElementType element)
    {
        if (currentElement == element)
            return false;

        return currentElement == ElementType.None;
    }
}
