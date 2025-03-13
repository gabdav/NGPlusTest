using System;
using Unity.VisualScripting;
using UnityEngine;


public class UiSlotController : MonoBehaviour
{
    [SerializeField] private UiItemSlotInteractor itemSlot;
    [SerializeField] private UiItemVisualPreview itemVisuals;
    public UiItemSlotInteractor GetSlot() {  return itemSlot; }
    public void InitVisual(InventoryItem item)
    {      
        itemVisuals.UpdateItemVisual(item.item.icon, item.quantity);
    }
    public void TriggerVisuals(bool trigger)
    {
        itemVisuals.gameObject.SetActive(trigger);
    }

}
