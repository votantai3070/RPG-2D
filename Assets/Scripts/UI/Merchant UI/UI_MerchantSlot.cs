using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchant;

    public enum MerchantSlotType { MerchantSlot, PlayerSlot }
    public MerchantSlotType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (merchant == null)
            return;

        if (itemInSlot == null)
            return;

        bool leftButton = eventData.button == PointerEventData.InputButton.Left;
        bool rightButton = eventData.button == PointerEventData.InputButton.Right;

        if (slotType == MerchantSlotType.PlayerSlot)
        {
            if (leftButton)
                base.OnPointerDown(eventData);
            else if (rightButton)
            {
                bool fullStack = Input.GetKey(KeyCode.LeftControl);
                merchant.TrySellItem(itemInSlot, fullStack);
            }
        }
        else if (slotType == MerchantSlotType.MerchantSlot)
        {
            if (leftButton)
                return;

            bool fullStack = Input.GetKey(KeyCode.LeftControl);
            merchant.TryBuyItem(itemInSlot, fullStack);
        }

        ui.itemTooltip.ShowTooltip(false, null);

        merchant.TriggerUpdateUI();
    }

    public void SetMerchantUI(Inventory_Merchant merchant) => this.merchant = merchant;
}
