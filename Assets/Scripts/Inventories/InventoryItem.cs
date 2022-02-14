using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    //name of item
    public string itemName;
    //item sprite
    public Sprite itemSprite;
    //maximum number of items in a stack
    public byte stackSize;
    //maximum durability of item
    public byte baseDurability;
    //damage done by item if tool or weapon
    public byte damage;
    //if item is used for fighting
    public bool fighting;
    //item level
    public int level;
    //cost to buy item
    public int cost;
    //cost to craft item
    public int craftcost;
    //identifying number of item
    public byte itemID;
    //list of items that are used to craft the item
    public List<InventoryItem> ingredients = new List<InventoryItem>();
    //list of number relating to number of ingredients used to craft the item
    public List<int> ingredientCount = new List<int>();
    // script related to weapon if a weapon
    public WeaponInterface weaponScript { get { return (WeaponInterface)gameObject.GetComponent(typeof(WeaponInterface)); } }
    //different types of items
    public enum ItemType
    {
        Weapon,
        Consumable,
        Tool,
        Mineral,
    }
    //type of item
    public ItemType itemType;
}