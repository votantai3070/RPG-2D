using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviewSlot : MonoBehaviour
{
    [SerializeField] private Image materialIcon;
    [SerializeField] private TextMeshProUGUI materialText;

    public void SetupMaterialSlot(ItemDataSO itemData, int availiableAmount, int requireAmount)
    {
        materialIcon.sprite = itemData.itemIcon;
        materialText.text = itemData.itemName + " - " + availiableAmount + " / " + requireAmount;
    }
}
