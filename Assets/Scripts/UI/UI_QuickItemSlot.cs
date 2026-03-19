using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlot : UI_ItemSlot
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private int slotNumber;

    public void SetupQuickSlotItem(Inventory_Item itemToPass)
    {
        inventory.SetQuickItemInSlot(slotNumber, itemToPass);
    }

    public void UpdateQuickSlotUI(Inventory_Item currentItemSlot)
    {
        if (currentItemSlot == null || currentItemSlot.itemData == null)
        {
            itemIcon.sprite = defaultSprite;
            itemStackSize.text = "";
            return;
        }

        itemIcon.sprite = currentItemSlot.itemData.itemIcon;
        itemStackSize.text = currentItemSlot.stackSize.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.ingame.OpenQuickItemOptions(this, rect);
    }
}
