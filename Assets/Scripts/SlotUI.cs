using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public ItemUI itemUI;
    public Action<Vector2, SlotUI> OnSlotDown;
    public Action<Vector2, SlotUI> OnSlotDrag;
    public Action<Vector2, SlotUI> OnSlotUp;

    public RectTransform rectTransform;


    public void OnDrag(PointerEventData eventData)
    {
        if (itemUI == null) return;
        OnSlotDrag?.Invoke(eventData.position, this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemUI == null) return;
        OnSlotDown?.Invoke(eventData.position, this);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (itemUI == null) return;
        OnSlotUp?.Invoke(eventData.position, this);
    }

}
