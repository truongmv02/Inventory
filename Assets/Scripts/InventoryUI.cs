using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    int slotCount = 20;
    public CharacterEquipmentUI characterEquipmentUI;
    public SlotUI slotUIPrefab;
    public ItemUI itemUIPrefab;
    public Transform inventoryContent;
    public Image itemIcon;

    RectTransform rectTransform;
    public List<SlotUI> slots = new List<SlotUI>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        for (int i = 0; i < slotCount; i++)
        {
            var slotUI = Instantiate(slotUIPrefab);
            slotUI.transform.SetParent(inventoryContent);
            slots.Add(slotUI);
            slotUI.OnSlotDrag = OnSlotDrag;
            slotUI.OnSlotUp = OnSlotUp;
        }
        AddItem(items[0]);
        AddItem(items[1]);
        AddItem(items[2]);
        AddItem(items[3]);

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

    public SlotUI GetSlotEmpty()
    {
        foreach (var slot in slots)
        {
            if (slot.itemUI == null) return slot;
        }
        return null;
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
        var equipSlot = characterEquipmentUI.GetSlot(position);
        if (equipSlot != null)
        {
            if (slot.itemUI.info.type == ItemType.Equipment)
            {
                if (equipSlot.itemUI != null)
                {
                    var itemEquipInfo = equipSlot.itemUI.info;
                    equipSlot.itemUI.SetInfo(slot.itemUI.info);
                    slot.itemUI.SetInfo(itemEquipInfo);
                }
                else
                {
                    characterEquipmentUI.AddItem(slot.itemUI.info, equipSlot);
                    RemoveItem(slot);
                }
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
            if (slotNew.itemUI == null) // v? trí m?i
            {
                SetSlotInfo(slotNew, slot.itemUI);
                slot.itemUI = null;
            }
            else // ??i ch?
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
    public void SetSlotInfo(SlotUI slot, ItemUI item)
    {
        slot.itemUI = item;
        item.transform.SetParent(slot.transform);
        item.transform.localPosition = Vector2.zero;
    }

    //TEST
    public List<ItemInfoSO> items;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddItem(items[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddItem(items[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddItem(items[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddItem(items[3]);
        }

    }

}
