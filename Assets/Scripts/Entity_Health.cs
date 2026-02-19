using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity entity;
    private Entity_Stats stats;

    [Header("Health Info")]
    [SerializeField] private float currentHealth;
    [SerializeField] private Slider healthSlider;

    [Header("Damaged Info")]
    [SerializeField] private float damagedVfxDuration = .1f;


    [SerializeField] protected bool isDead;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
        healthSlider = GetComponentInChildren<Slider>();

        currentHealth = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual bool TakeDamaged(int damage, float elementalDamage, ElementType elementType, Transform damagedDealer)
    {
        if (isDead) return false;

        if (AttackEvaded())
        {
            // Optionally, you can add some evasion VFX or sound here
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }

        transform.GetComponent<Entity_DamageVfx>().DamageVfx(damagedVfxDuration);

        Entity_Stats attackerStats = damagedDealer.GetComponent<Entity_Stats>();

        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0f;

        float migitation = stats.GetArmorMigitation(armorReduction);
        int physicalDamageTaken = Mathf.RoundToInt(damage * (1 - migitation));

        float elementRes = stats.GetElementalResistance(elementType);
        int elementalDamageTaken = Mathf.RoundToInt(elementalDamage * (1 - elementRes));

        int finalDamage = physicalDamageTaken + elementalDamageTaken;

        ReduceHp(finalDamage);

        TakeKnockback(damagedDealer, physicalDamageTaken);

        return true;
    }

    private void TakeKnockback(Transform damagedDealer, int finalDamage)
    {
        float averangeDamage = finalDamage / stats.GetMaxHealth();

        entity.KnockBack(damagedDealer, averangeDamage);
    }

    private bool AttackEvaded()
    {
        float evasionChance = stats.GetEvasion();
        return Random.value < evasionChance;
    }

    private void ReduceHp(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    private void UpdateHealthBar()
    {
        if (healthSlider == null)
            return;

        healthSlider.value = currentHealth / stats.GetMaxHealth();
    }

    protected virtual void Die()
    {
        isDead = true;

        entity.TryEnterDeadState();
    }
}
