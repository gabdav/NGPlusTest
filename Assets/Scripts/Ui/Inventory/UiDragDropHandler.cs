using System;
using UnityEngine;
using UnityEngine.UI;

public class UiDragDropHandler : MonoBehaviour
{
    public Action OnRelease = delegate { };
    [SerializeField] private RectTransform parent;
    [SerializeField] private Image image;
    private InventoryItem item;
    private InventorySlot fromSlot;

    private void Awake()
    {
        Hide();
    }
    public void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, null, out Vector2 localPoint);
        transform.localPosition = localPoint;
    }
    public InventoryItem GetItem() { return item; }
    public InventorySlot GetFromSlot() { return fromSlot; }
    public void SetItem(InventoryItem item) { this.item = item; }
    public void SetSlot(InventorySlot fromSlot) { this.fromSlot = fromSlot; }
    public void SetSprite(Sprite s) { image.sprite = s; }
    public void Hide()
    {
        SetItem(InventoryItem.Null);
        SetSlot(null);
        SetSprite(null);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        OnRelease?.Invoke();
        Hide();
    }
    public void Show(InventoryItem item)
    {
        gameObject.SetActive(true);
        SetItem(item);
        SetSprite(item.item.icon);
        UpdatePosition();
    }
}