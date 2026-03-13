using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player playerInventory { get; private set; }
    public List<Inventory_Item> materialStash { get; private set; } = new();

    public void ConsumeMaterials(Inventory_Item itemToCraft)
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

    public bool HasEnoughMaterials(Inventory_Item itemToCraft)
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
            materialStash.Add(itemToAdd);

        TriggerUpdateUI();
    }

    public Inventory_Item StackableInStash(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = materialStash?.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackable in stackableItems)
        {
            if (stackable.CanStackSize())
                return stackable;
        }

        return null;
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
}
