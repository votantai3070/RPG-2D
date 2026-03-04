using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    public UI_ItemSlot[] itemSlots { get; private set; }
    [SerializeField] private Inventory_Base inventory;

    private void Awake()
    {
        itemSlots = GetComponentsInChildren<UI_ItemSlot>(true);
        inventory = FindAnyObjectByType<Inventory_Base>();
        inventory.OnInventoryChange += UpdateInventorySlot;

        UpdateInventorySlot();
    }

    public void UpdateInventorySlot()
    {
        List<Inventory_Item> itemList = inventory.itemList;

        Debug.Log("item count " + itemList.Count);

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

    private void OnDestroy()
    {
        inventory.OnInventoryChange -= UpdateInventorySlot;
    }
}
