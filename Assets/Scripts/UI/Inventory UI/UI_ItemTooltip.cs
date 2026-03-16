using TMPro;
using UnityEngine;

public class UI_ItemTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private TextMeshProUGUI merchantInfo;

    public void ShowTooltip(bool show, RectTransform target, Inventory_Item itemToShow, bool buyPrice = false, bool showMechantInfo = false)
    {
        base.ShowTooltip(show, target);

        merchantInfo.gameObject.SetActive(showMechantInfo);

        int price = buyPrice ? itemToShow.itemData.itemPrice : Mathf.FloorToInt(itemToShow.sellPrice);
        int totalPrice = price * itemToShow.stackSize;

        string fullStackPrice = $"Price: {price}x {itemToShow.stackSize} - {totalPrice}g.";
        string singleStackPrice = $"Price: {price}g";

        itemPrice.text = itemToShow.stackSize > 1 ? fullStackPrice : singleStackPrice;
        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = itemToShow.GetItemInfo();
    }
}
