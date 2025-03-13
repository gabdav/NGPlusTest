using System;
using Unity.VisualScripting;
using UnityEngine;


public class UiInventoryDropableItem : MonoBehaviour
{
    [SerializeField] private UiDragDropableItem drag;
    [SerializeField] protected RectTransform ownTransform;
    [SerializeField] private UiItemVisualPreview itemVisuals;
    public void Init(InventoryItem item)
    {
        
        itemVisuals.UpdateItemVisual(item.item.icon, item.quantity);
    }
    public void InitHandler(UiDragDropHandler handler)
    {
        drag.InitHandler(handler);
    }
    public RectTransform GetTransform() { return ownTransform; }
    public void TriggerVisuals(bool trigger)
    {
        itemVisuals.gameObject.SetActive(trigger);
    }
    public void SetupDrag(InventorySlot slotedItem, Action OnSplit, Action OnMerge)
    {
        drag.Init(slotedItem, OnSplit, OnMerge);
    }
    public void SetOnDropAction(Action OnDrop)
    {
        drag.SetOnDropAction(OnDrop);
    }
    public void SetOnEmpty(Action OnEmpty)
    {
        drag.SetOnEmpty(OnEmpty);
    }
    public void SetEmpty()
    {
        drag.SetEmpty();
    }
}
