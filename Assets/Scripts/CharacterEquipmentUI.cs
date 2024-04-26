using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEquipmentUI : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public ItemUI itemUIPrefab;
    public Image itemIcon;
    RectTransform rectTransform;
    public List<SlotUI> slots = new List<SlotUI>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        slots = GetComponentsInChildren<SlotUI>().ToList();
        foreach (var slot in slots)
        {
            slot.OnSlotDrag = OnSlotDrag;
            slot.OnSlotUp = OnSlotUp;
        }
    }

    void OnSlotDrag(Vector2 position, SlotUI slot)
    {
        itemIcon.transform.position = position;
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = slot.itemUI.info.sprite;
        itemIcon.SetNativeSize();
    }

    void OnSlotUp(Vector2 position, SlotUI slot)
    {
        itemIcon.gameObject.SetActive(false);

        var invenSlot = inventoryUI.GetSlot(position);
        if (invenSlot != null)
        {
            if (invenSlot.itemUI != null)
            {
                if (invenSlot.itemUI.info.type != ItemType.None)
                {
                    var itemInvenInfo = invenSlot.itemUI.info;
                    invenSlot.itemUI.SetInfo(slot.itemUI.info);
                    slot.itemUI.SetInfo(itemInvenInfo);
                }
            }
            else
            {
                inventoryUI.AddItem(slot.itemUI.info, invenSlot);
                RemoveItem(slot);
            }
            return;
        }

        if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position)) // xóa item
        {
            RemoveItem(slot);
            return;
        }

        SlotUI slotNew = GetSlot(position);

        if (slotNew != null && slotNew != slot)
        {
            if (slotNew.itemUI == null) // vị trí mới
            {
                SetSlotInfo(slotNew, slot.itemUI);
                slot.itemUI = null;
            }
            else // đổi chỗ
            {
                var itemUI = slotNew.itemUI;
                SetSlotInfo(slotNew, slot.itemUI);
                SetSlotInfo(slot, itemUI);

            }
        }
    }

    public SlotUI GetSlot(Vector2 position)
    {
        foreach (var slotUI in slots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(slotUI.rectTransform, position))
            {
                return slotUI;
            }
        }
        return null;
    }

    public void AddItem(ItemInfoSO itemInfo, SlotUI slot = null)
    {
        if (slot == null)
        {
            slot = GetSlotEmpty();
        }
        if (slot == null)
        {
            Debug.Log("Inventory full");
            return;
        }

        ItemUI itemUI = Instantiate(itemUIPrefab, slot.transform);
        itemUI.SetInfo(itemInfo);
        slot.itemUI = itemUI;
    }

    public void RemoveItem(ItemInfoSO itemInfo)
    {
        foreach (var slot in slots)
        {
            if (slot.itemUI != null && slot.itemUI.info == itemInfo)
            {
                RemoveItem(slot);
            }
        }
    }

    public void RemoveItem(SlotUI slot)
    {
        if (slot.itemUI != null)
        {
            Destroy(slot.itemUI.gameObject);
            slot.itemUI = null;
        }
    }

    public void SetSlotInfo(SlotUI slot, ItemUI item)
    {
        slot.itemUI = item;
        item.transform.SetParent(slot.transform);
        item.transform.localPosition = Vector2.zero;
    }
    public SlotUI GetSlotEmpty()
    {
        foreach (var slot in slots)
        {
            if (slot.itemUI == null) return slot;
        }
        return null;
    }


}
