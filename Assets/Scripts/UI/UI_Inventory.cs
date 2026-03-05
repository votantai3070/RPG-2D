using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private Inventory_Player inventory;

    public UI_ItemSlot[] itemSlots { get; private set; }
    public UI_EquipSlot[] equipSlot { get; private set; }

    [SerializeField] private Transform itemTransformParent;
    [SerializeField] private Transform equipTransformParent;

    private void Awake()
    {
        itemSlots = itemTransformParent.GetComponentsInChildren<UI_ItemSlot>(true);
        equipSlot = equipTransformParent.GetComponentsInChildren<UI_EquipSlot>(true);

        inventory = FindAnyObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateEquipSlot();
        UpdateInventorySlot();
    }

    private void UpdateEquipSlot()
    {
        List<Inventory_EquipmentSlot> equipList = inventory.equipList;

        for (int i = 0; i < equipSlot.Length; i++)
        {
            if (equipList[i].HasItem() == false)
                equipSlot[i].UpdateSlot(null);
            else
                equipSlot[i].UpdateSlot(equipList[i].equipedItem);
        }
    }

    public void UpdateInventorySlot()
    {
        List<Inventory_Item> itemList = inventory.itemList;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < inventory.itemList.Count)
            {
                itemSlots[i].UpdateSlot(itemList[i]);
            }
            else
                itemSlots[i].UpdateSlot(null);
        }
    }

}
