using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;
    [Space]
    [SerializeField] private string metConditionHex;
    [SerializeField] private string notMetConditionHex;
    [SerializeField] private string importantConditionHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockedSkillText = "You've taken a different path - this skill is locked.";

    protected override void Awake()
    {
        base.Awake();

        skillTree = GetComponentInParent<UI_SkillTree>();
    }

    public override void ShowTooltip(bool show, RectTransform target)
    {
        base.ShowTooltip(show, target);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, UI_TreeNode node)
    {
        base.ShowTooltip(show, targetRect);

        skillName.text = node.skillData.displayName;
        skillDescription.text = node.skillData.description;

        string skillLockText = $"<color={importantConditionHex}>{lockedSkillText} </color>";
        string requirements = node.isLocked ? skillLockText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);

        skillRequirements.text = requirements;
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements:");

        string costColor = skillTree.EnoughSkillPoint(skillCost) ? metConditionHex : notMetConditionHex;

        sb.AppendLine($"<color={costColor}>- {skillCost} skill point(s) </color>");

        foreach (var node in neededNodes)
        {
            string nodeColor = node.isUnlocked ? metConditionHex : notMetConditionHex;
            sb.AppendLine($"<color={nodeColor}>- {node.skillData.displayName} </color>");
        }

        if (conflictNodes.Length <= 0)
            return sb.ToString();

        sb.AppendLine();
        sb.AppendLine($"<color={importantConditionHex}>- Lock out:");

        foreach (var node in conflictNodes)
        {
            sb.AppendLine($"<color={importantConditionHex}>- {node.skillData.displayName} </color>");
        }

        return sb.ToString();
    }
}
