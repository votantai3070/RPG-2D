using UnityEngine;

[CreateAssetMenu(fileName = "Item effect data - Heal on doing physical damage", menuName = "RPG Setup/Item Data/Item Effect/Heal On Damage Effect")]
public class ItemEffect_HealOnDoingDamage : ItemEffectDataSO
{
    [SerializeField] private float percentHealOnAttack = .2f;

    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * percentHealOnAttack);
    }

    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }
}
