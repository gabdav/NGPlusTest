using UnityEngine;
using System.Collections.Generic;
using System;
using static UnityEditor.Progress;

[Serializable]
public class Inventory : MonoBehaviour
{
    public Action OnItemListChanged = delegate { };
    public InventorySlot[] inventorySlotsArray;
    [SerializeField] private int maxItems = 10;
    private List<InventoryItem> _itemList;
    public int MaxSlots { get { return maxItems; } }
    public void Init()
    {
        _itemList = new List<InventoryItem>();
        inventorySlotsArray = new InventorySlot[maxItems];
        for (int i = 0; i < inventorySlotsArray.Length; i++)
        {
            inventorySlotsArray[i] = new InventorySlot(i);
        }

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
                IncreaseQuant(i, item);
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
                IncreaseQuant(i, item);
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
                DecreaseQuant(i, item);
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
    public InventorySlot[] GetInventorySlots() { return inventorySlotsArray; }
    public List<InventoryItem> GetItemList() { return _itemList; }
    public void SyncListWithSlots()
    {
        for(int i = 0;i < inventorySlotsArray.Length; i++)
        {
            if (inventorySlotsArray[i].GetItem() != InventoryItem.Null)
            {
                InventoryItem tempItem = inventorySlotsArray[i].GetItem();
                if(_itemList.Count == 0)
                {
                    _itemList.Add(tempItem);
                    continue;
                }
                int index = -1;
                foreach(var item in _itemList)
                {
                    if (item.item == tempItem.item)
                    {
                        index = _itemList.IndexOf(tempItem);
                        break;
                    }                   
                }
                if(index >= 0)
                {
                    IncreaseQuant(index, tempItem);
                }
                else
                {
                    _itemList.Add(tempItem);
                }

                
            }
        }
        OnItemListChanged?.Invoke();
    }
    private void IncreaseQuant(int index , InventoryItem item)
    {
        InventoryItem tempItem = new()
        {
            item = _itemList[index].item,
            quantity = _itemList[index].quantity + item.quantity
        };
        _itemList[index] = tempItem;
    }
    private void DecreaseQuant(int index, InventoryItem item)
    {
        InventoryItem tempItem = new()
        {
            item = _itemList[index].item,
            quantity = _itemList[index].quantity - item.quantity
        };
        _itemList[index] = tempItem;
    }
}
