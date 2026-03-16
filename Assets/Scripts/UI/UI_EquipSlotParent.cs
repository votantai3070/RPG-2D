using System.Collections.Generic;
using UnityEngine;

public class UI_EquipSlotParent : MonoBehaviour
{
    private UI_EquipSlot[] equipSlots;

    public void UpdateEquipmentSlots(List<Inventory_EquipmentSlot> equipList)
    {
        if (equipSlots == null)
            equipSlots = GetComponentsInChildren<UI_EquipSlot>();

        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipList[i].HasItem() == false)
                equipSlots[i].UpdateSlot(null);
            else
                equipSlots[i].UpdateSlot(equipList[i].equipedItem);
        }
    }
}
