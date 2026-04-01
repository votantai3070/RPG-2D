using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Die();
        }
    }

    public override bool TakeDamage(int damage, float elementalDamage, ElementType elementType, Transform damagedDealer)
    {
        return base.TakeDamage(damage, elementalDamage, elementType, damagedDealer);
    }

    protected override void Die()
    {
        base.Die();

        player.ui.OpenDeathScreenUI();
    }
}
