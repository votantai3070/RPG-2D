using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    private Inventory_Player playerInventory;

    private UI_CraftPreview craftPreview;
    private UI_CraftListButton[] craftListButton;
    private UI_CraftSlot[] craftSlots;

    public void SetupCraftUI(Inventory_Storage storage)
    {
        playerInventory = storage.playerInventory;
        playerInventory.OnInventoryChange += UpdateUI;
        UpdateUI();

        craftPreview = GetComponentInChildren<UI_CraftPreview>();
        craftPreview.SetupCraftPreview(storage);
        SetupCraftListButton();
    }
    private void SetupCraftListButton()
    {
        craftListButton = GetComponentsInChildren<UI_CraftListButton>();
        craftSlots = GetComponentsInChildren<UI_CraftSlot>();

        foreach (var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (var button in craftListButton)
        {
            button.SetCraftSlots(craftSlots);
        }
    }

    private void UpdateUI() => inventoryParent.UpdateSlots(playerInventory.itemList);
}
