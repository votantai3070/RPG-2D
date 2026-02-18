using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity entity;

    [Header("Health Info")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthSlider;

    [Header("Damaged Info")]
    [SerializeField] private float damagedVfxDuration = .1f;


    [SerializeField] protected bool isDead;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        healthSlider = GetComponentInChildren<Slider>();

        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public virtual void TakeDamaged(int damage, Transform damagedDealer)
    {
        if (isDead) return;

        ReduceHp(damage);

        transform.GetComponent<Entity_DamageVfx>().DamageVfx(damagedVfxDuration);

        float averangeDamage = damage / maxHealth;

        entity.KnockBack(damagedDealer, averangeDamage);
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

        healthSlider.value = (float)currentHealth / maxHealth;
    }

    protected virtual void Die()
    {
        isDead = true;

        entity.TryEnterDeadState();
    }
}
