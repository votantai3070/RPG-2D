using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Skill_SO skillData;
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;

    [Header("Unlock node details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isLocked;
    public bool isUnlocked;

    [Header("Skill details")]
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private Color lockColor;
    private Color lastColor;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();
    }

    private void Start()
    {
        skillIcon.color = lockColor;
    }

    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        skillTree.AddSkillPoint(skillCost);

        skillIcon.color = lockColor;
    }

    private void Unlock()
    {
        if (CanBeUnlock())
        {
            isUnlocked = true;
            UpdateIconColor(Color.white);
            skillTree.RemoveSkillPoint(skillCost);
            LockConflictNodes();
            connectHandler.ConnectionImageUnlocked(true);

            Debug.Log("Node unlocked: " + gameObject.name);
        }
        else
        {
            ui.tooltip.LockedSkillEffect();
            Debug.Log("Cannot unlock this node.");
        }
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
        }
    }

    private bool CanBeUnlock()
    {
        if (isLocked || isUnlocked)
            return false;

        if (!skillTree.EnoughSkillPoint(skillCost))
            return false;

        foreach (var node in neededNodes)
        {
            if (!node.isUnlocked)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }

        return true;
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Unlock();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.tooltip.ShowTooltip(true, rect, this);

        if (!isLocked || !isUnlocked)
            ToggleNodeHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.tooltip.ShowTooltip(false, rect);

        if (!isLocked || !isUnlocked)
            ToggleNodeHighlight(false);
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightedColor = Color.white * 0.9f;
        highlightedColor.a = 1;

        Color colorApply = highlight ? highlightedColor : lastColor;

        UpdateIconColor(colorApply);
    }

    private void OnValidate()
    {
        if (skillData != null)
        {
            skillName = skillData.displayName;
            if (skillIcon != null)
                skillIcon.sprite = skillData.icon;
            skillCost = skillData.cost;
            gameObject.name = skillName + " Node";
        }
    }
}
