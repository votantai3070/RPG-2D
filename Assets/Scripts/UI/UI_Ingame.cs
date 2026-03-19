using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    private Player player;
    private Inventory_Player inventory;
    private UI_SkillSlot[] skillSlots;

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Quick Item Slots")]
    [SerializeField] private float yOffsetQuickItemParent = 150;
    [SerializeField] private Transform quickItemOptionParent;
    private UI_QuickItemSlotOption[] quickItemSlotOptions;
    private UI_QuickItemSlot[] quickItemSlots;

    private void Start()
    {
        quickItemSlots = GetComponentsInChildren<UI_QuickItemSlot>();
        player = FindFirstObjectByType<Player>();
        inventory = player.inventory;

        player.health.OnHealthChange += UpdateHealthBar;
        UpdateHealthBar();
        inventory.OnQuickSlotUsed += UpdateQuickSlotsUI;
    }

    public void UpdateQuickSlotsUI(int slotNumber, Inventory_Item itemInSlot)
    {
        quickItemSlots[slotNumber].UpdateQuickSlotUI(itemInSlot);
    }

    public void OpenQuickItemOptions(UI_QuickItemSlot quickItemSlot, RectTransform targetRect)
    {
        if (quickItemSlotOptions == null)
            quickItemSlotOptions = GetComponentsInChildren<UI_QuickItemSlotOption>(true);

        List<Inventory_Item> consumables = inventory.itemList.FindAll(item => item.itemData.itemType == ItemType.Consumable);

        for (int i = 0; i < quickItemSlotOptions.Length; i++)
        {
            if (i < consumables.Count)
            {
                quickItemSlotOptions[i].gameObject.SetActive(true);
                quickItemSlotOptions[i].SetupOption(quickItemSlot, consumables[i]);
            }
            else
                quickItemSlotOptions[i].gameObject.SetActive(false);
        }

        quickItemOptionParent.position = targetRect.position + Vector3.up * yOffsetQuickItemParent;
    }

    public void HideQuickItemOptions() => quickItemOptionParent.position = new(0, 999);

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
