using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    private Player player;
    private UI_SkillSlot[] skillSlots;

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();

        player.health.OnHealthChange += UpdateHealthBar;
        UpdateHealthBar();
    }

    public UI_SkillSlot GetSkillSlot(SkillType skillType)
    {
        if (skillSlots == null)
            skillSlots = GetComponentsInChildren<UI_SkillSlot>(true);

        foreach (var slot in skillSlots)
        {
            if (skillType == slot.skillType)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }

        return null;
    }


    private void UpdateHealthBar()
    {
        int currentHealth = Mathf.FloorToInt(player.health.GetCurrentHealth());
        float maxHealth = player.playerStats.GetMaxHealth();
        float sizeDiff = Mathf.Abs(maxHealth - healthRect.sizeDelta.x);

        if (sizeDiff > .1f)
            healthRect.sizeDelta = new Vector2(maxHealth, healthRect.sizeDelta.y);

        healthText.text = $"{currentHealth} / {maxHealth}";
        healthSlider.value = player.health.GetHealthPercent();
    }
}
