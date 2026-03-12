using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Skill_DataSO skillData;
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
        if (skillData.unlockedByDefault)
            Unlock();

        skillIcon.color = lockColor;
    }

    public void Refund()
    {
        if (skillData.unlockedByDefault || isUnlocked == false)
            return;

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
            LockConflictNodes();

            skillTree.RemoveSkillPoint(skillCost);
            connectHandler.ConnectionImageUnlocked(true);

            skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrades(skillData.upgradeData);

            Debug.Log("Node unlocked: " + gameObject.name);
        }
        else
        {
            ui.skillTooltip.LockedSkillEffect();
            Debug.Log("Cannot unlock this node.");
        }
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNode();
        }
    }

    private void LockChildNode()
    {
        isLocked = true;

        foreach (var node in connectHandler.GetChildNode())
            node.LockChildNode();
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
        ui.skillTooltip.ShowTooltip(true, rect, this);

        if (isLocked || isUnlocked)
            return;

        ToggleNodeHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.ShowTooltip(false, rect);

        if (isLocked || isUnlocked)
            return;

        ToggleNodeHighlight(false);
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightedColor = Color.white * 0.9f;
        highlightedColor.a = 1;

        Color colorApply = highlight ? highlightedColor : lastColor;

        UpdateIconColor(colorApply);
    }

    private void OnEnable()
    {
        if (!isLocked && isUnlocked)
            UpdateIconColor(Color.white);

        if (isLocked && !isUnlocked)
            UpdateIconColor(lockColor);
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
