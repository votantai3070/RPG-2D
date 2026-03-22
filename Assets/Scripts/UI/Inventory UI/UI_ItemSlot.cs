using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player inventory;
    protected UI ui;
    protected RectTransform rect;

    [Header("UI Slot Setup")]
    [SerializeField] protected GameObject defaultIcon;
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemStackSize;

    protected virtual void Awake()
    {
        inventory = FindAnyObjectByType<Inventory_Player>();
        ui = GetComponentInParent<UI>();
        rect = GetComponentInParent<RectTransform>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        bool alternativeInput = Input.GetKey(KeyCode.LeftControl);

        if (alternativeInput)
        {
            inventory.RemoveOneItem(itemInSlot);
        }
        else
        {
            if (itemInSlot.itemData.itemType == ItemType.Consumable)
            {
                inventory.TryUseItem(itemInSlot);
            }
            else
                inventory.TryEquipItem(itemInSlot);
        }

        if (itemInSlot == null)
            ui.itemTooltip.ShowTooltip(false, null);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        ui.itemTooltip.ShowTooltip(true, rect, itemInSlot);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        ui.itemTooltip.ShowTooltip(false, null);
    }

    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (defaultIcon != null)
            defaultIcon.gameObject.SetActive(itemInSlot == null);

        if (itemInSlot == null)
        {
            itemIcon.color = Color.clear;
            itemStackSize.text = string.Empty;
            return;
        }

        Color color = Color.white;
        color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = itemInSlot.stackSize > 1 ? (itemInSlot.stackSize).ToString() : "";
    }
}
