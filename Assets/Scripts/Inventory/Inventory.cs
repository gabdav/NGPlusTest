using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Inventory : MonoBehaviour
{
    public Action OnItemListChanged = delegate { };
    public InventorySlot[] inventorySlotsArray;
    [SerializeField] private int maxItems = 10;
    private List<InventoryItem> _itemList;
    [SerializeField]  UiInventory inventory;
    [SerializeField] List<InventoryItem> testItems;
    public int MaxSlots { get { return maxItems; } }
    public void Start()
    {
        _itemList = new List<InventoryItem>();
        inventorySlotsArray = new InventorySlot[maxItems];
        for (int i = 0; i < inventorySlotsArray.Length; i++)
        {
            inventorySlotsArray[i] = new InventorySlot(i);
        }
        inventory.SetInventory(this);
        inventory.Init();
        InitInventory(testItems);
    }
    public void InitInventory(List<InventoryItem> testItems)
    {
        foreach (InventoryItem item in testItems)
        {
            AddItem(item);
        }
    }
    public InventorySlot GetEmptyInventorySlot()
    {
        foreach (InventorySlot inventorySlot in inventorySlotsArray)
        {
            if (inventorySlot.IsEmpty())
                return inventorySlot;
        }
        Debug.LogError("No empty inventory slot");
        return null;
    }
    public InventorySlot GetInUseInventorySlot(InventoryItem item)
    {
        foreach (InventorySlot inventorySlot in inventorySlotsArray)
        {
            if (inventorySlot.GetItem() == item)
                return inventorySlot;
        }
        Debug.LogError("Cannot find item");
        return null;
    }
    public void UpdateItem(InventoryItem item)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].Equals(item))
            {
                InventoryItem tempItem = new()
                {
                    item = _itemList[i].item,
                    quantity = _itemList[i].quantity + item.quantity
                };
                _itemList[i] = tempItem;
                GetEmptyInventorySlot().SetItem(item);
                OnItemListChanged?.Invoke();
                return;
            }
        }
    }
    public void AddNewItem(InventoryItem item)
    {
        if (_itemList.Count >= MaxSlots)
        {
            Debug.Log("Inv full");
            return;
        }
        _itemList.Add(item);    
        GetEmptyInventorySlot().SetItem(item);
        OnItemListChanged?.Invoke();
    }
    public void AddItem(InventoryItem item)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].Equals(item))
            {
                InventoryItem tempItem = new()
                {
                    item = _itemList[i].item,
                    quantity = _itemList[i].quantity + item.quantity
                };
                _itemList[i] = tempItem;
                GetInUseInventorySlot(item).SetItem(_itemList[i]);
                OnItemListChanged?.Invoke();
                return;
            }
        }
        AddNewItem(item);
    }
    public void MergeItem(InventoryItem itemToMerge, InventorySlot toSlot, InventorySlot fromSlot)
    {
        if (!itemToMerge.Equals(toSlot.GetItem()))
            return;
        InventoryItem presentItem = toSlot.GetItem();
        InventoryItem newItem = new()
        {
            item = itemToMerge.item,
            quantity = itemToMerge.quantity + presentItem.quantity
        };
        TransferItem(newItem, toSlot, fromSlot);
    }
    public void TransferItem(InventoryItem item, InventorySlot toSlot, InventorySlot fromSlot)
    {
        if (fromSlot == null || toSlot == null || item == InventoryItem.Null)
            return;
        fromSlot.RemoveItem();
        toSlot.SetItem(item);
        OnItemListChanged?.Invoke();
    }
    public void SuspendItem(InventoryItem item)
    {
        GetInUseInventorySlot(item).RemoveItem();
        _itemList.Remove(item);
    }
    public void RemoveItem(InventoryItem item)
    {
        SuspendItem(item);
        OnItemListChanged?.Invoke();
    }
    public bool SubstractFromItemAtSlot(int slot, InventoryItem item)
    {
        if (slot >= inventorySlotsArray.Length)
            return false;
        if (!inventorySlotsArray[slot].GetItem().Equals(item))
        {
            Debug.LogError("Wrong item at slot");
            return false;
        }
        float remainingItems = inventorySlotsArray[slot].GetItem().quantity - item.quantity;
        if (remainingItems <= 0)
        {
            RemoveItem(item); return true;
        }
        InventoryItem tempItem = new()
        {
            item = inventorySlotsArray[slot].GetItem().item,
            quantity = remainingItems
        };
        inventorySlotsArray[slot].SetItem(tempItem);
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].Equals(item))
            {
                InventoryItem tempItem2 = new()
                {
                    item = item.item,
                    quantity = _itemList[i].quantity - item.quantity
                };
                _itemList[i] = tempItem2;
            }
        }

        OnItemListChanged?.Invoke();
        return true;
    }

    public void SubstractFromItem(InventoryItem item)
    {
        for (int i = 0; i < inventorySlotsArray.Length; i++)
        {
            if (inventorySlotsArray[i].GetItem().Equals(item))
            {
                if (SubstractFromItemAtSlot(i, item))
                    return;
            }
        }
    }
    public bool IsInventorySpaceAvailable => _itemList.Count < MaxSlots;
    public List<InventoryItem> GetItemList() { return _itemList; }
    public InventorySlot[] GetInventorySlots() { return inventorySlotsArray; }
    public InventoryItem FindItem(InventoryItem item)
    {
        InventoryItem requestedItem = InventoryItem.Null;
        foreach (InventoryItem itemInInv in _itemList)
        {
            if (itemInInv == item)
            {
                if (requestedItem != item)
                {
                    requestedItem = new()
                    {
                        item = itemInInv.item,
                        quantity = itemInInv.quantity
                    };
                }
                else
                {
                    requestedItem.quantity += itemInInv.quantity;
                }
            }
        }
        return requestedItem;
    }
}
