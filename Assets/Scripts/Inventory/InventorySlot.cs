using System;

[Serializable]
public class InventorySlot
{
    private int index;
    private InventoryItem item;

    public InventorySlot(int index)
    {
        this.index = index;
    }
    public int GetIndex() { return index; }
    public InventoryItem GetItem()
    {
        return item;
    }

    public void SetItem(InventoryItem item)
    {
        this.item = item;
    }

    public void RemoveItem()
    {
        item = InventoryItem.Null;
    }

    public bool IsEmpty()
    {
        return item == InventoryItem.Null;
    }

}