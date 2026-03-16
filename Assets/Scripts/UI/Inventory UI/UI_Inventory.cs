using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_EquipSlotParent equipSlotsParent;
    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;

    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory_Player>();

        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotsParent.UpdateSlots(inventory.itemList);
        equipSlotsParent.UpdateEquipmentSlots(inventory.equipList);
    }
}
