using System.Collections;
using UnityEngine;

public enum ItemType
{
    None,
    Equipment
}

[CreateAssetMenu(fileName = "Item", menuName = "ScripableObjects/Item")]
public class ItemInfoSO : ScriptableObject
{
    public Sprite sprite;
    public ItemType type;
}
