using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    public override void ShowTooltip(bool show, RectTransform target)
    {
        base.ShowTooltip(show, target);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, Skill_SO skillData)
    {
        base.ShowTooltip(show, targetRect);

        skillName.text = skillData.displayName;
        skillDescription.text = skillData.description;
        skillRequirements.text = "Requiment: \n "
            + " -" + $"Cost: {skillData.cost}" + " skill point.";
    }
}
