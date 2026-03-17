using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    private Player player;

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        player.health.OnHealthChange += UpdateHealthBar;
        UpdateHealthBar();
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
