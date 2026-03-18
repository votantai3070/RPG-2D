using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    private UI_SkillTree skillTree;
    private UI ui;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCooldown;
    [SerializeField] private TextMeshProUGUI skillRequirements;
    [Space]
    [SerializeField] private string metConditionHex;
    [SerializeField] private string notMetConditionHex;
    [SerializeField] private string importantConditionHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockedSkillText = "You've taken a different path - this skill is locked.";

    private Coroutine textEffectCo;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponentInParent<UI>();
    }

    private void Start()
    {
        skillTree = ui.skillTree;
    }

    public override void ShowTooltip(bool show, RectTransform target = null)
    {
        base.ShowTooltip(show, target);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, Skill_DataSO skillData, UI_TreeNode node)
    {
        base.ShowTooltip(show, targetRect);

        if (show == false)
            return;

        skillName.text = skillData.displayName;
        skillDescription.text = skillData.description;
        skillCooldown.text = $"Cooldown: {skillData.upgradeData.cooldown} s";

        if (node == null)
        {
            skillRequirements.text = "";
            return;
        }

        string skillLockText = $"<color={importantConditionHex}>{lockedSkillText} </color>";
        string requirements = node.isLocked ? skillLockText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);

        skillRequirements.text = requirements;
    }

    public void LockedSkillEffect()
    {
        if (textEffectCo != null)
            StopCoroutine(textEffectCo);

        textEffectCo = StartCoroutine(TextBlinkEffectCo(skillRequirements, .15f, 3));
    }

    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColoredText(notMetConditionHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
            text.text = GetColoredText(importantConditionHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements:");

        string costColor = skillTree.EnoughSkillPoint(skillCost) ? metConditionHex : notMetConditionHex;
        string costText = $"- {skillCost} skill point(s) </color>";
        string finalCostText = GetColoredText(costColor, costText);

        sb.AppendLine(finalCostText);

        foreach (var node in neededNodes)
        {
            if (node == null)
                continue;

            string nodeColor = node.isUnlocked ? metConditionHex : notMetConditionHex;
            string nodeText = $"- {node.skillData.displayName} </color>";
            string finalNodeText = GetColoredText(nodeColor, nodeText);

            sb.AppendLine(finalNodeText);
        }

        if (conflictNodes.Length <= 0)
            return sb.ToString();

        sb.AppendLine();
        sb.AppendLine($"<color={importantConditionHex}>- Lock out: ");

        foreach (var node in conflictNodes)
        {
            if (node == null)
                continue;

            string nodeText = $"- {node.skillData.displayName} </color>";
            string finalNodeText = GetColoredText(importantConditionHex, nodeText);

            sb.AppendLine(finalNodeText);
        }

        return sb.ToString();
    }
}
