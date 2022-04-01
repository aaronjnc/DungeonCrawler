using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [Tooltip("Name of item")]
    public string itemName;
    [Tooltip("Item sprite")]
    public Sprite itemSprite;
    [Tooltip("Max item stack size")]
    public byte stackSize;
    [Tooltip("Starting item durabiliy")]
    public byte baseDurability;
    [Tooltip("Damage done by item")]
    public byte damage;
    [Tooltip("Item level")]
    public int level;
    [Tooltip("Cost to buy item")]
    public int buyCost;
    [Tooltip("Cost to craft item")]
    public int craftCost;
    [Tooltip("Identifying number of item")]
    public byte itemID;
    [Tooltip("List of items used in crafting")]
    public List<InventoryItem> ingredients = new List<InventoryItem>();
    [Tooltip("List of counts for ingredients")]
    public List<int> ingredientCount = new List<int>();
    //different types of items
    public enum ItemType
    {
        Consumable,
        Tool,
        Mineral,
    }
    [Tooltip("Type of item")]
    public ItemType itemType;
}