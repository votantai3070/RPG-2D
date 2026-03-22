using TMPro;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_EquipSlotParent equipSlotsParent;
    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;
    [SerializeField] private TextMeshProUGUI goldNumberText;

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory_Player>();

        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotsParent.UpdateSlots(inventory.itemList);
        equipSlotsParent.UpdateEquipmentSlots(inventory.equipList);
        goldNumberText.text = inventory.gold.ToString("N0") + "g.";

    }
}
