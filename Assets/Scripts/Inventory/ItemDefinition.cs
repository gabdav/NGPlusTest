using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Definition", fileName = "ItemDefinition", order = 1)]
public class ItemDefinition : ScriptableObject
{
    public string title;
    public Sprite icon;
    public GameObject prefab;
    public string description;
    public ItemType type;
}
[Serializable]
public enum ItemType
{
    Consumable,
    Equipable
}