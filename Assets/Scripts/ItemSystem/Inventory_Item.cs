using System;

[Serializable]
public class Inventory_Item
{
    public ItemDataSO itemData;
    public int stackSize = 1;

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
    }

    public bool CanStackSize() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
