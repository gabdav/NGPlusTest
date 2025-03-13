using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiItemSlotInteractor : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private UiDragDropHandler handler;
    private InventoryItem item;
    private InventorySlot fromSlot;
    private Action OnSplitCall = delegate { };
    private Action<InventoryItem> OnConsumeCall = delegate { };
    private Action OnMergeCall = delegate { };
    private Action OnDropAction = delegate { };
    private Action OnEmptyCall = delegate { };
    private Action<bool,string> OnMouseOverCall = delegate { };
    public void Init(InventorySlot slotedItem, Action OnSplit, Action OnMerge, Action OnEmpty, Action<InventoryItem> OnConsume)
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        fromSlot = slotedItem;
        item = slotedItem.GetItem();
        OnSplitCall = OnSplit;
        OnMergeCall = OnMerge;
        OnEmptyCall = OnEmpty;
        OnConsumeCall = OnConsume;
    }
    public void InitTooltip(Action<bool, string> OnShowTooltip)
    {
        OnMouseOverCall = OnShowTooltip;
    }
    public void SetEmpty()
    {
        if (fromSlot == null)
            return;
        fromSlot.RemoveItem();
        item = fromSlot.GetItem();
        OnEmptyCall?.Invoke();
    }
    public void InitHandler(UiDragDropHandler handler)
    {
        this.handler = handler;
    }
    public void SetOnDropAction(Action onDropAction)
    {
        this.OnDropAction = onDropAction;
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
                OnMergeCall?.Invoke();
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
            OnSplitCall?.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnConsumeCall?.Invoke(item);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bool hasItem = item != InventoryItem.Null;
        string description = hasItem ? item.item.description : "";
        OnMouseOverCall?.Invoke(hasItem, description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseOverCall?.Invoke(false, "");
    }
}
