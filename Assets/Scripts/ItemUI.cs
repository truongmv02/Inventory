using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public ItemInfoSO info;
    public Image icon;

    public void SetInfo(ItemInfoSO info)
    {
        this.info = info;
        icon.sprite = info.sprite;
        icon.SetNativeSize();
    }




}
