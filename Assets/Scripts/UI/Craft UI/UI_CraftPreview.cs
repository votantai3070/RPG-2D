using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;
    private UI_CraftPreviewSlot[] slots;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI craftButtonText;

    public void ConfirmCraft()
    {
        if (itemToCraft == null)
        {
            craftButtonText.text = "Pick an item!";
            return;
        }

        if (storage.CanCraftItem(itemToCraft))
            storage.CraftItem(itemToCraft);

        UpdateCraftPreviewSlots();
    }

    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;

        slots = GetComponentsInChildren<UI_CraftPreviewSlot>();

        foreach (var slot in slots)
            slot.gameObject.SetActive(false);
    }

    public void UpdateCraftPreview(ItemDataSO itemData)
    {
        itemToCraft = new(itemData);

        itemIcon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();
        UpdateCraftPreviewSlots();
    }

    private void UpdateCraftPreviewSlots()
    {
        foreach (var slot in slots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRecipe[i];

            int availiableAmount = storage.GetAvailiableAmountOf(requiredItem.itemData);
            int requireAmount = requiredItem.stackSize;

            if (i < slots.Length)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].SetupMaterialSlot(requiredItem.itemData, availiableAmount, requireAmount);
            }
        }
    }
}
