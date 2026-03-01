using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity entity;
    private Entity_Stats entityStats;

    [Header("Health Info")]
    [SerializeField] private float currentHealth;
    [SerializeField] private Slider healthSlider;
    [Space]
    private bool canRegenerateHealth = true;

    [Header("Damaged Info")]
    [SerializeField] private float damagedVfxDuration = .1f;

    [SerializeField] protected bool isDead;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityStats = GetComponent<Entity_Stats>();
        healthSlider = GetComponentInChildren<Slider>();

        SetupHealth();
    }

    private void SetupHealth()
    {
        if (entityStats == null)
            return;

        currentHealth = entityStats.GetMaxHealth();
        UpdateHealthBar();
    }

    public float GetHealthPercent() => currentHealth / entityStats.GetMaxHealth();

    public void SetHealthPercent(float percent)
    {
        currentHealth = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }


    public void Heal()
    {
        if (!canRegenerateHealth)
            return;

        float healRegenAmount = entityStats.resource.healthRegen.GetValue();
        InsreaseHealth(healRegenAmount);
    }

    private void InsreaseHealth(float amount)
    {
        if (isDead)
            return;

        currentHealth += amount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(currentHealth, maxHealth);
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

        if (transform.GetComponent<Entity_VFX>() != null)
            transform.GetComponent<Entity_VFX>().DamageVfx(damagedVfxDuration);

        Entity_Stats attackerStats = damagedDealer.GetComponent<Entity_Stats>();

        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0f;
        float migitation = entityStats != null ? entityStats.GetArmorMigitation(armorReduction) : 0;
        float elementRes = entityStats != null ? entityStats.GetElementalResistance(elementType) : 0;

        int physicalDamageTaken = Mathf.RoundToInt(damage * (1 - migitation));
        int elementalDamageTaken = Mathf.RoundToInt(elementalDamage * (1 - elementRes));

        int finalDamage = physicalDamageTaken + elementalDamageTaken;

        ReduceHp(finalDamage);

        TakeKnockback(damagedDealer, physicalDamageTaken);

        return true;
    }

    private void TakeKnockback(Transform damagedDealer, int finalDamage)
    {
        if (entityStats == null)
            return;

        float averangeDamage = finalDamage / entityStats.GetMaxHealth();

        entity.KnockBack(damagedDealer, averangeDamage);
    }

    private bool AttackEvaded()
    {
        if (entityStats == null)
            return false;
        else
            return Random.value < entityStats.GetEvasion();
    }

    public void ReduceHp(int damage)
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

        healthSlider.value = currentHealth / entityStats.GetMaxHealth();
    }

    protected virtual void Die()
    {
        isDead = true;

        entity.TryEnterDeadState();
    }
}
