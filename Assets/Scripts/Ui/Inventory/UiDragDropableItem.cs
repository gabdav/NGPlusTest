using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiDragDropableItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private UiDragDropHandler handler;
    private InventoryItem item;
    private InventorySlot fromSlot;
    private Action OnPointerDownCall = delegate { };
    private Action OnMerge = delegate { };
    private Action OnDropAction = delegate { };
    private Action OnEmpty = delegate { };
    public void Init(InventorySlot slotedItem, Action OnPointerDown, Action OnMerge)
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        fromSlot = slotedItem;
        this.item = slotedItem.GetItem();
        OnPointerDownCall = OnPointerDown;
        this.OnMerge = OnMerge;
    }
    public void SetEmpty()
    {
        if (fromSlot == null)
            return;
        fromSlot.RemoveItem();
        item = fromSlot.GetItem();
        OnEmpty?.Invoke();
    }
    public void InitHandler(UiDragDropHandler handler)
    {
        this.handler = handler;
    }
    public void SetOnDropAction(Action onDropAction)
    {
        this.OnDropAction = onDropAction;
    }
    public void SetOnEmpty(Action OnEmpty)
    {
        this.OnEmpty = OnEmpty;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.Equals(InventoryItem.Null))
            return;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = .55f;
        handler.Show(item);
        handler.SetSlot(fromSlot);
    }

    public void OnDrag(PointerEventData eventData)
    {
        handler.UpdatePosition();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventoryItem itm = handler.GetItem();
            if (itm == item && fromSlot != handler.GetFromSlot())
                OnMerge?.Invoke();
            else
                OnDropAction?.Invoke();
        }

    }
    private void OnDisable()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        handler.Hide();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            OnPointerDownCall?.Invoke();
        }
    }

}
