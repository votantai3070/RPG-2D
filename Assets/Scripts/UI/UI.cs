using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillTooltip tooltip;

    private void Awake()
    {
        tooltip = GetComponentInChildren<UI_SkillTooltip>();
    }
}
