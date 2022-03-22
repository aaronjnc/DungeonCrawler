using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    [Tooltip("Number of items in slot")]
    private byte currentStack = 0;
    [Tooltip("Reference to inventory item in slot")]
    private InventoryItem itemRef;
    [Tooltip("Current durability of item in slot")]
    private byte durability;
    /// <summary>
    /// Adds item to slot using given item reference with stack of 1
    /// </summary>
    /// <param name="itemRef">InventoryItem reference to put in slot</param>
    public void AddItem(InventoryItem itemRef)
    {
        this.itemRef = itemRef;
        currentStack = 1;
        durability = itemRef.baseDurability;
    }
    /// <summary>
    /// Adds item to slot using given item reference, with given count and durability
    /// </summary>
    /// <param name="itemRef">reference to item info</param>
    /// <param name="count">number of items</param>
    /// <param name="durability">durability of item</param>
    public void AddItem(InventoryItem itemRef, byte count, byte durability)
    {
        this.itemRef = itemRef;
        currentStack = count;
        this.durability = durability;
    }
    /// <summary>
    /// Determines of slot is empty
    /// </summary>
    /// <returns>true if empty</returns>
    public bool IsEmpty()
    {
        if (itemRef == null)
        {
            return true;
        }
        if ((durability <= 0 && itemRef.baseDurability != 0) || currentStack <= 0)
        {
            EmptySlot();
            return true;
        } else
        {
            return false;
        }
    }
    /// <summary>
    /// Empties the slot removing the info
    /// </summary>
    public void EmptySlot()
    {
        itemRef = null;
        currentStack = 0;
        durability = 0;
    }
    /// <summary>
    /// Adds same item to stack
    /// </summary>
    /// <param name="stackAmount">amount to add to stack</param>
    /// <returns></returns>
    public int AddToStack(byte stackAmount)
    {
        currentStack += stackAmount;
        if (currentStack > itemRef.stackSize)
        {
            int count = currentStack - itemRef.stackSize;
            currentStack = itemRef.stackSize;
            return count;
        }
        return 0;
    }
    /// <summary>
    /// Increase durability of item
    /// </summary>
    /// <param name="fixAmount">adds this number of current durability</param>
    public void IncreaseDurability(byte fixAmount)
    {
        durability = (byte)Mathf.Clamp(durability + fixAmount, 0, itemRef.baseDurability);
    }
    /// <summary>
    /// Determines if item slot is full
    /// </summary>
    /// <returns></returns>
    public bool IsFull()
    {
        return currentStack == itemRef.stackSize;
    }
    /// <summary>
    /// reduces the number of items in slot
    /// </summary>
    /// <param name="reduceAmount">amount to reduce by</param>
    public void ReduceStack(byte reduceAmount)
    {
        currentStack -= reduceAmount;
    }
    /// <summary>
    /// reduces durability of item
    /// </summary>
    /// <param name="reduceAmount">amount to reduce by</param>
    public void ReduceDurability(byte reduceAmount)
    {
        durability -= reduceAmount;
    }
    /// <summary>
    /// returns the sprite for item in the slot
    /// </summary>
    /// <returns></returns>
    public Sprite GetSprite()
    {
        if (itemRef == null)
            return null;
        return itemRef.itemSprite;
    }
    /// <summary>
    /// returns true if slot contains a weapon
    /// </summary>
    /// <returns></returns>
    public bool IsWeapon()
    {
        if (itemRef == null)
            return false;
        return itemRef.weapon;
    }
    /// <summary>
    /// returns cost to buy item
    /// </summary>
    /// <returns></returns>
    public int GetBuyCost()
    {
        if (itemRef == null)
            return 0;
        return itemRef.buyCost;
    }
    /// <summary>
    /// Returns identifying number of item
    /// </summary>
    /// <returns></returns>
    public byte GetItemID()
    {
        if (itemRef == null)
            return 127;
        return itemRef.itemID;
    }
    /// <summary>
    /// returns InventoryItem reference of item in slot
    /// </summary>
    /// <returns></returns>
    public InventoryItem GetItemRef()
    {
        return itemRef;
    }
    /// <summary>
    /// returns number of items in this slot
    /// </summary>
    /// <returns></returns>
    public byte GetCount()
    {
        return currentStack;
    }
    /// <summary>
    /// returns durability of the item in this slot
    /// </summary>
    /// <returns></returns>
    public byte GetDurability()
    {
        return durability;
    }
    /// <summary>
    /// transfers information from another itemSlot into this one
    /// </summary>
    /// <param name="newInfo">ItemSlot to transfer info from</param>
    public void AddExisting(ItemSlot newInfo)
    {
        currentStack = newInfo.GetCount();
        itemRef = newInfo.GetItemRef();
        durability = newInfo.GetDurability();
    }
    /// <summary>
    /// returns the damage done by this item
    /// </summary>
    /// <returns></returns>
    public byte GetDamage()
    {
        if (itemRef == null)
            return 0;
        return itemRef.damage;
    } 
    /// <summary>
    /// returns the weapon script associated with this item
    /// </summary>
    /// <returns></returns>
    public WeaponInterface GetWeaponScript()
    {
        if (itemRef == null)
            return null;
        return itemRef.weaponScript;
    }
    /// <summary>
    /// returns the item type
    /// </summary>
    /// <returns></returns>
    public InventoryItem.ItemType GetItemType()
    {
        return itemRef.itemType;
    }
    /// <summary>
    /// returns list of ingredients used to craft this item
    /// </summary>
    /// <returns></returns>
    public List<InventoryItem> GetIngredients()
    {
        return itemRef.ingredients;
    }
    /// <summary>
    /// returns list of amount of ingredients used to craft this item
    /// </summary>
    /// <returns></returns>
    public List<int> GetIngredientCount()
    {
        return itemRef.ingredientCount;
    }
    /// <summary>
    /// returns cost to craft item
    /// </summary>
    /// <returns></returns>
    public int GetCraftCost()
    {
        return itemRef.craftCost;
    }
    /// <summary>
    /// returns item name
    /// </summary>
    /// <returns></returns>
    public string GetItemName()
    {
        return itemRef.itemName;
    }
}
