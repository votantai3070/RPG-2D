using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillTooltip tooltip;
    public UI_SkillTree skillTree;

    bool skillTreeEnabled;

    private void Awake()
    {
        tooltip = GetComponentInChildren<UI_SkillTooltip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
    }

    public void ToggleSkillTree()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        tooltip.ShowTooltip(false);
    }
}
