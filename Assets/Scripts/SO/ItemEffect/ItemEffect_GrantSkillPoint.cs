using UnityEngine;

[CreateAssetMenu(fileName = "Item effect data - Grant skill point", menuName = "RPG Setup/Item Data/Item Effect/Grant Skill Effect")]
public class ItemEffect_GrantSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int addSkillPoints;

    public override void ExecuteEffect()
    {
        UI ui = FindAnyObjectByType<UI>();

        ui.skillTree.AddSkillPoint(addSkillPoints);
    }
}
