using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private UI_ItemSlotParent storageSlotParent;

    public void SetupStorage(Inventory_Player inventory, Inventory_Storage storage)
    {
        this.inventory = inventory;
        this.storage = storage;

        storage.OnInventoryChange += UpdateUI;
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>(true);
        foreach (var slot in storageSlots)
        {
            slot.SetStorage(storage);
        }
    }

    private void UpdateUI()
    {
        storageSlotParent.UpdateSlots(storage.itemList);
        inventorySlotParent.UpdateSlots(inventory.itemList);
    }
}
