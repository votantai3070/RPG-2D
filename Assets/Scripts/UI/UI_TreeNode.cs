using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Skill_SO skillData;
    private UI ui;
    private RectTransform rect;

    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;

    [SerializeField] private Color lockColor;
    private Color lastColor;

    private bool isLocked;
    private bool isUnlocked;


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
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
            Debug.Log("Node unlocked: " + gameObject.name);
        }
        else
            Debug.Log("Cannot unlock this node.");
    }

    private bool CanBeUnlock()
    {
        if (isLocked || isUnlocked)
            return false;

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
        ui.tooltip.ShowTooltip(true, rect, skillData);
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
            gameObject.name = skillName + " Node";
        }
    }
}
