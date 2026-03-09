using UnityEngine;

[CreateAssetMenu(fileName = "Item effect data - Thunder strike on damage", menuName = "RPG Setup/Item Data/Item Effect/Thunder Strike On Damage Effect")]
public class ItemEffect_ThunderStrikeOnDamage : ItemEffectDataSO
{
    [SerializeField] private float chance = .15f;
    [SerializeField] private GameObject thunderStrike;
    [SerializeField] private float ThunderDamage = 30;
    [SerializeField] private LayerMask whatIsEnemy;

    public override void ExecuteEffect()
    {
        bool random = Random.value < chance;

        if (random)
            DamageEnemiesWithThunderStrike();
    }

    private void DamageEnemiesWithThunderStrike()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {
            if (!target.TryGetComponent<IDamageable>(out var damageable)) continue;

            bool targetOnHit = damageable.TakeDamage(0, ThunderDamage, ElementType.Lightning, player.transform);

            if (targetOnHit)
                player.playerVfx.CreateEffectOf(thunderStrike, target.transform);
        }
    }

    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.combat.OnDoingThunderStrikeDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingThunderStrikeDamage -= ExecuteEffect;
        player = null;
    }
}
