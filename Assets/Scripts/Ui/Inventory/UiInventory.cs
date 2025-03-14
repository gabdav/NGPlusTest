using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UiInventory : MonoBehaviour
{
    private Inventory _inventory;
    [SerializeField] private Transform inventoryItemContainer;
    [SerializeField] private UiSlotController inventoryItem;
    [SerializeField] private UiDragDropHandler handler;
    [SerializeField] private UiTooltip tooltip;
    [SerializeField] private TextMeshProUGUI consumedText;
    private List<UiSlotController> spanwedInventoryCells = new();
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
        foreach (Transform c in inventoryItemContainer)
            Destroy(c.gameObject);
        foreach (InventorySlot inventorySlot in _inventory.GetInventorySlots())
        {
            Transform itemSpawnLocation = inventoryItemContainer;

            UiSlotController spawnedItem = Instantiate(inventoryItem, itemSpawnLocation).GetComponent<UiSlotController>();
            spawnedItem.TriggerVisuals(false);
            spanwedInventoryCells.Add(spawnedItem);
        }

    }
    private void RefreshInventoryItems()
    {
        foreach (InventorySlot inventorySlot in _inventory.GetInventorySlots())
        {
            spanwedInventoryCells[inventorySlot.GetIndex()].GetSlot().InitHandler(handler);
            if (!inventorySlot.IsEmpty())
            {
                spanwedInventoryCells[inventorySlot.GetIndex()].TriggerVisuals(true);
                spanwedInventoryCells[inventorySlot.GetIndex()].InitVisual(inventorySlot.GetItem());
                spanwedInventoryCells[inventorySlot.GetIndex()].GetSlot().Init(inventorySlot,
                    () => { SplitStack(inventorySlot); },
                    () =>
                    {
                        _inventory.MergeItem(handler.GetItem(), inventorySlot, handler.GetFromSlot());
                        handler.Hide();
                    },null, OnConsumeItem);
            }
            else
            {
                spanwedInventoryCells[inventorySlot.GetIndex()].GetSlot().SetEmpty();
                spanwedInventoryCells[inventorySlot.GetIndex()].TriggerVisuals(false);
            }
            spanwedInventoryCells[inventorySlot.GetIndex()].GetSlot().InitTooltip(tooltip.DisplayTooltip);

            spanwedInventoryCells[inventorySlot.GetIndex()].GetSlot().SetOnDropAction(() =>
            {
                _inventory.TransferItem(handler.GetItem(), inventorySlot, handler.GetFromSlot());
                handler.Hide();
            });
        }
    }
    private void OnConsumeItem(InventoryItem item)
    {
        InventoryItem tempItem = new()
        {
            item = item.item,
            quantity = 1
        };
        _inventory.SubstractFromItem(tempItem);
        Debug.Log("Consume " + item.item.type);
        consumedText.text = item.item.type + " used.";
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
