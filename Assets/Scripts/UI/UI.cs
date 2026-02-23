using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillTooltip tooltip;
    public UI_SkillTree skillTree;

    private void Awake()
    {
        tooltip = GetComponentInChildren<UI_SkillTooltip>();
        skillTree = GetComponentInChildren<UI_SkillTree>();
    }
}
