using TMPro;
using UnityEngine;

public class UI_ItemTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private TextMeshProUGUI merchantInfo;
    [SerializeField] private TextMeshProUGUI inventoryInfo;

    public void ShowTooltip(bool show, RectTransform target, Inventory_Item itemToShow, bool buyPrice = false, bool showMechantInfo = false, bool showControls = true)
    {
        base.ShowTooltip(show, target);

        if (showControls)
        {
            merchantInfo.gameObject.SetActive(showMechantInfo);
            inventoryInfo.gameObject.SetActive(!showMechantInfo);
        }
        else
        {
            merchantInfo.gameObject.SetActive(false);
            inventoryInfo.gameObject.SetActive(false);
        }


        int price = buyPrice ? itemToShow.itemData.itemPrice : Mathf.FloorToInt(itemToShow.sellPrice);
        int totalPrice = price * itemToShow.stackSize;

        string fullStackPrice = $"Price: {price}x {itemToShow.stackSize} - {totalPrice}g.";
        string singleStackPrice = $"Price: {price}g";

        itemPrice.text = itemToShow.stackSize > 1 ? fullStackPrice : singleStackPrice;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = itemToShow.GetItemInfo();

        string color = GetColorByRarity(itemToShow.itemData.itemRarity);
        itemName.text = GetColoredText(color, itemToShow.itemData.itemName);
    }

    private string GetColorByRarity(int rarity)
    {
        if (rarity <= 100) return "while";
        if (rarity <= 100) return "green";
        if (rarity <= 100) return "blue";
        if (rarity <= 100) return "purple";
        return "orange";
    }
}
