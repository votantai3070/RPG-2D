using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item effect data - Buff", menuName = "RPG Setup/Item Data/Item Effect/Buff Effect")]
public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    public override bool CanBeUsed(Player player)
    {
        if (player.playerStats.CanApplyBuffOf(source))
        {
            this.player = player;
            return true;
        }
        else
        {
            Debug.Log("Same buff effect cannot be apllied twice!");
            return false;
        }
    }

    public override void ExecuteEffect()
    {
        player.playerStats.ApplyBuff(buffsToApply, duration, source);
    }
}
