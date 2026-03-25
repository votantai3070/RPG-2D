using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player playerInventory { get; private set; }
    public List<Inventory_Item> materialStash { get; private set; } = new();

    public void CraftItem(Inventory_Item itemToCraft)
    {
        ConsumeMaterials(itemToCraft);
        playerInventory.AddItem(itemToCraft);
    }

    public bool CanCraftItem(Inventory_Item itemToCraft)
    {
        return HasEnoughMaterials(itemToCraft) && playerInventory.CanAddItem(itemToCraft);
    }

    private void ConsumeMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredItem in itemToCraft.itemData.craftRecipe)
        {
            int amountToConsume = requiredItem.stackSize;

            amountToConsume -= ConsumedMaterialsAmount(playerInventory.itemList, requiredItem);

            if (amountToConsume > 0)
                amountToConsume -= ConsumedMaterialsAmount(itemList, requiredItem);

            if (amountToConsume > 0)
                amountToConsume -= ConsumedMaterialsAmount(materialStash, requiredItem);
        }
    }

    private int ConsumedMaterialsAmount(List<Inventory_Item> itemList, Inventory_Item neededItem)
    {
        int amountNeeded = neededItem.stackSize;
        int consumeAmount = 0;

        foreach (var item in itemList)
        {
            if (item.itemData != neededItem.itemData)
                continue;

            int removeAmount = Mathf.Min(item.stackSize, amountNeeded - consumeAmount);
            item.stackSize -= removeAmount;
            consumeAmount += removeAmount;

            if (item.stackSize <= 0)
                itemList.Remove(item);

            if (consumeAmount >= removeAmount)
                break;
        }

        return consumeAmount;
    }

    private bool HasEnoughMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredMaterial in itemToCraft.itemData.craftRecipe)
        {
            if (GetAvailiableAmountOf(requiredMaterial.itemData) < requiredMaterial.stackSize)
                return false;
        }

        return true;
    }

    public int GetAvailiableAmountOf(ItemDataSO requiredItem)
    {
        int amount = 0;

        foreach (var item in playerInventory.itemList)
        {
            if (item.itemData == requiredItem)
                amount += item.stackSize;
        }

        foreach (var item in itemList)
        {
            if (item.itemData == requiredItem)
                amount += item.stackSize;
        }


        foreach (var item in materialStash)
        {
            if (item.itemData == requiredItem)
                amount += item.stackSize;
        }

        return amount;
    }

    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);

        if (stackableItem != null)
            stackableItem.AddStack();
        else
        {
            var newItemToAdd = new Inventory_Item(itemToAdd.itemData);
            materialStash.Add(newItemToAdd);
        }

        TriggerUpdateUI();
        materialStash = materialStash.OrderByDescending(item => item.itemData.name).ToList();
    }

    public Inventory_Item StackableInStash(Inventory_Item itemToAdd)
    {
        return materialStash?.Find(item => item.itemData == itemToAdd.itemData && item.CanAddStack());


    }

    public void SetInventory(Inventory_Player inventory) => playerInventory = inventory;

    public void FromPlayerToStorage(Inventory_Item item, bool transferToStack)
    {
        float amountToAdd = transferToStack ? item.stackSize : 1;

        for (int i = 0; i < amountToAdd; i++)
        {
            if (CanAddItem(item))
            {
                Inventory_Item itemToAdd = new(item.itemData);

                playerInventory.RemoveOneItem(item);
                AddItem(itemToAdd);
            }
        }

        TriggerUpdateUI();
    }

    public void FromStorageToPlayer(Inventory_Item item, bool transferToStack)
    {
        float amountToAdd = transferToStack ? item.stackSize : 1;

        for (int i = 0; i < amountToAdd; i++)
        {
            if (playerInventory.CanAddItem(item))
            {
                Inventory_Item itemToAdd = new(item.itemData);

                RemoveOneItem(item);
                playerInventory.AddItem(itemToAdd);
            }
        }


        TriggerUpdateUI();
    }

    public override void SaveData(ref GameData data)
    {
        data.storageItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveId;

                if (data.storageItems.ContainsKey(saveId) == false)
                    data.storageItems[saveId] = 0;

                data.storageItems[saveId] += item.stackSize;
            }
        }

        data.storageMaterials.Clear();

        foreach (var item in materialStash)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveId;

                if (data.storageMaterials.ContainsKey(saveId) == false)
                    data.storageMaterials[saveId] = 0;

                data.storageMaterials[saveId] += item.stackSize;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        itemList.Clear();
        materialStash.Clear();

        foreach (var entry in data.storageItems)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }


            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new(itemData);
                AddItem(itemToLoad);
            }
        }

        foreach (var entry in data.storageMaterials)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }


            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new(itemData);

                AddMaterialToStash(itemToLoad);
            }
        }
    }
}
