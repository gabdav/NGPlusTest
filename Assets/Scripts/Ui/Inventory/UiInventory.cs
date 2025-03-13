using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiInventory : MonoBehaviour
{
    private Inventory _inventory;
    [SerializeField] private Transform inventoryItemContainer;
    [SerializeField] private UiInventoryDropableItem inventoryItem;
    [SerializeField] private UiDragDropHandler handler;
    private List<UiInventoryDropableItem> spanwedInventoryCells = new();
    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
        GenerateSlots();
    }
    public void Init()
    {
        RefreshInventoryItems();

        _inventory.OnItemListChanged += RefreshInventoryItems;
    }
    private void GenerateSlots()
    {
        foreach (Transform c in inventoryItemContainer)//TEMP
            Destroy(c.gameObject);
        foreach (InventorySlot inventorySlot in _inventory.GetInventorySlots())
        {
            Transform itemSpawnLocation = inventoryItemContainer;

            UiInventoryDropableItem spawnedItem = Instantiate(inventoryItem, itemSpawnLocation).GetComponent<UiInventoryDropableItem>();
            spawnedItem.TriggerVisuals(false);
            spanwedInventoryCells.Add(spawnedItem);
        }

    }
    private void RefreshInventoryItems()
    {
        foreach (InventorySlot inventorySlot in _inventory.GetInventorySlots())
        {
            spanwedInventoryCells[inventorySlot.GetIndex()].InitHandler(handler);
            if (!inventorySlot.IsEmpty())
            {
                spanwedInventoryCells[inventorySlot.GetIndex()].TriggerVisuals(true);
                spanwedInventoryCells[inventorySlot.GetIndex()].Init(inventorySlot.GetItem());
                spanwedInventoryCells[inventorySlot.GetIndex()].SetupDrag(inventorySlot,
                    () => { SplitStack(inventorySlot); },
                    () =>
                    {
                        _inventory.MergeItem(handler.GetItem(), inventorySlot, handler.GetFromSlot());
                        handler.Hide();
                    });
            }
            else
            {
                spanwedInventoryCells[inventorySlot.GetIndex()].SetEmpty();
                spanwedInventoryCells[inventorySlot.GetIndex()].TriggerVisuals(false);
            }
            spanwedInventoryCells[inventorySlot.GetIndex()].SetOnDropAction(() =>
            {
                _inventory.TransferItem(handler.GetItem(), inventorySlot, handler.GetFromSlot());
                handler.Hide();
            });
        }
    }
    private void SplitStack(InventorySlot inventorySlot)
    {
        if (inventorySlot.GetItem().quantity <= 1)
            return;
        if (!_inventory.IsInventorySpaceAvailable)
            return;
        int half = (int)inventorySlot.GetItem().quantity / 2;
        InventoryItem tempItem = new()
        {
            item = inventorySlot.GetItem().item,
            quantity = half
        };
        _inventory.SubstractFromItemAtSlot(inventorySlot.GetIndex(), tempItem);
        _inventory.UpdateItem(tempItem);
    }
}
