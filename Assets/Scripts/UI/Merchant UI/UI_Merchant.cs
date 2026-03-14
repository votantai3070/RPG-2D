using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Merchant merchant;
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_ItemSlotParent merchantSlots;

    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player inventory)
    {
        this.merchant = merchant;
        this.inventory = inventory;

        merchant.OnInventoryChange += UpdateSlotUI;
        UpdateSlotUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();
        foreach (var slot in merchantSlots)
        {
            slot.SetMerchantUI(merchant);
        }
    }

    private void UpdateSlotUI()
    {
        inventorySlots.UpdateSlots(inventory.itemList);
        merchantSlots.UpdateSlots(merchant.itemList);
    }
}
