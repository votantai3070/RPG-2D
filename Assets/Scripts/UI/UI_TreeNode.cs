using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Skill_SO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;

    [SerializeField] private Color lockColor;
    private Color lastColor;

    private bool isLocked;
    private bool isUnlocked;

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
        Debug.Log("Tooltip: " + gameObject.name);
        UpdateIconColor(Color.white * .9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Hide Tooltip");
        UpdateIconColor(lastColor);
    }
}
