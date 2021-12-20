using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    private byte currentStack = 0;
    private InventoryItem itemRef;
    private byte durability;
    public void addItem(InventoryItem itemRef)
    {
        this.itemRef = itemRef;
        currentStack = 1;
        durability = itemRef.baseDurability;
    }
    public void addItem(InventoryItem itemRef, byte count, byte durability)
    {
        this.itemRef = itemRef;
        currentStack = count;
        this.durability = durability;
    }
    public bool isEmpty()
    {
        if (itemRef == null)
        {
            return true;
        }
        if ((durability <= 0 && itemRef.baseDurability != 0) || currentStack <= 0)
        {
            emptySlot();
            return true;
        } else
        {
            return false;
        }
    }
    public void emptySlot()
    {
        itemRef = null;
        currentStack = 0;
        durability = 0;
    }
    public int addToStack(byte stackAmount)
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
    public void increaseDurability(byte fixAmount)
    {
        durability = (byte)Mathf.Clamp(durability + fixAmount, 0, itemRef.baseDurability);
    }
    public bool isFull()
    {
        return currentStack == itemRef.stackSize;
    }
    public void reduceStack(byte reduceAmount)
    {
        currentStack -= reduceAmount;
    }
    public void reduceDurability(byte reduceAmount)
    {
        durability -= reduceAmount;
    }
    public Sprite getSprite()
    {
        if (itemRef == null)
            return null;
        return itemRef.itemSprite;
    }
    public bool fighting()
    {
        if (itemRef == null)
            return false;
        return itemRef.fighting;
    }
    public int getCost()
    {
        if (itemRef == null)
            return 0;
        return itemRef.cost;
    }
    public byte getItemId()
    {
        if (itemRef == null)
            return 127;
        return itemRef.itemID;
    }
    public InventoryItem getItem()
    {
        return itemRef;
    }
    public byte getCurrentCount()
    {
        return currentStack;
    }
    public byte getDurability()
    {
        return durability;
    }
    public void addExisting(ItemSlot newInfo)
    {
        currentStack = newInfo.getCurrentCount();
        itemRef = newInfo.getItem();
        durability = newInfo.getDurability();
    }
    public byte getDamage()
    {
        if (itemRef == null)
            return 0;
        return itemRef.damage;
    } 
    public WeaponInterface<Transform, PlayerFight> getWeaponScript()
    {
        if (itemRef == null)
            return null;
        return itemRef.weaponScript;
    }
}
