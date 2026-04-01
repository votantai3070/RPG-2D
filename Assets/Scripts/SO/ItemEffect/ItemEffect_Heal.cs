using UnityEngine;

[CreateAssetMenu(fileName = "Item effect data - Restore 10% of Max Health", menuName = "RPG Setup/Item Data/Item Effect/Heal Effect")]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healPercent = .1f;

    public override void ExecuteEffect()
    {
        Player player = FindAnyObjectByType<Player>();
        float healAmount = player.playerStats.GetMaxHealth() * healPercent;
        player.playerHealth.IncreaseHealth(healAmount);
    }
}
