using UnityEngine;

[CreateAssetMenu(fileName = "Item effect data - Ice Blast On Taking Damage", menuName = "RPG Setup/Item Data/Item Effect/Ice Blast Effect")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;
    [Space]
    [SerializeField] private float healthPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    private float lastTimeUsed = -999;
    [SerializeField] private GameObject iceBlast;
    [SerializeField] private GameObject onHitVfx;

    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;

        if (noCooldown && reachedThreshold)
        {
            player.playerVfx.CreateEffectOf(iceBlast, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {

            if (!target.TryGetComponent<IDamageable>(out var damageable)) continue;

            bool targetOnHit = damageable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            if (targetOnHit)
                player.playerVfx.CreateEffectOf(onHitVfx, target.transform);
        }
    }

    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.health.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage += ExecuteEffect;
        player = null;
    }
}
