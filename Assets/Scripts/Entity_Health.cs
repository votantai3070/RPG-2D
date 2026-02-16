using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    public Entity entity;

    [Header("Health Info")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [Header("Damaged Info")]
    [SerializeField] private float damagedVfxDuration = .1f;


    [SerializeField] protected bool isDead;

    private void Awake()
    {
        entity = GetComponent<Entity>();

        currentHealth = maxHealth;
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

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;

        entity.TryEnterDeadState();
    }
}
