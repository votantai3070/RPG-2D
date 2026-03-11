using System.Collections.Generic;

public class Inventory_Storage : Inventory_Base
{
    private Inventory_Player playerInventory;
    public List<Inventory_Item> materialStash { get; private set; } = new();

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
