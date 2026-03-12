using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSlot : MonoBehaviour
{
    private ItemDataSO itemToCraft;
    [SerializeField] private UI_CraftPreview craftPreview;

    [SerializeField] private Image craftIcon;
    [SerializeField] private TextMeshProUGUI craftName;


    public void SetupButton(ItemDataSO itemToCraft)
    {
        this.itemToCraft = itemToCraft;

        craftIcon.sprite = itemToCraft.itemIcon;
        craftName.text = itemToCraft.itemName;
    }


    public void UpdateCraftPreview()
    {
        craftPreview.UpdateCraftPreview(itemToCraft);
    }
}
