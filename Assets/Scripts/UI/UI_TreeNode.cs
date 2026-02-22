using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Skill_SO skillData;
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;

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
    }

    private void Start()
    {

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

            Debug.Log("Node unlocked: " + gameObject.name);
        }
        else
            Debug.Log("Cannot unlock this node.");
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
        UpdateIconColor(Color.white * .9f);
        ui.tooltip.ShowTooltip(true, rect, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdateIconColor(lastColor);
        ui.tooltip.ShowTooltip(false, rect);
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
