using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    public UI_ItemSlot[] itemSlots { get; private set; }
    public UI_EquipSlot[] equipSlot { get; private set; }

    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;
    [SerializeField] private Transform equipTransformParent;

    private void Awake()
    {
        equipSlot = equipTransformParent.GetComponentsInChildren<UI_EquipSlot>(true);
        inventory = FindAnyObjectByType<Inventory_Player>();

        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateEquipSlot();
        inventorySlotsParent.UpdateSlots(inventory.itemList);
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
}
