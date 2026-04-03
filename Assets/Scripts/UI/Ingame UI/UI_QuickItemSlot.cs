using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuickItemSlot : UI_ItemSlot
{
    private Button button;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private int slotNumber;

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    public void SimilateButtonFeedback()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
        ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }

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
        ui.ingameUI.OpenQuickItemOptions(this, rect);
    }
}
