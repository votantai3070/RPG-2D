using UnityEngine;

[CreateAssetMenu(fileName = "Item effect data - Refund all skills", menuName = "RPG Setup/Item Data/Item Effect/Refund All Skills Effect")]
public class ItemEffect_RefundAllSkills : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTree.RefundAllSkills();
    }
}
