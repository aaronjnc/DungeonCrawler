using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReference 
{
    public Sprite itemSprite;
    public byte stackSize;
    public byte baseDurability;
    public byte durability;
    public byte currentStack = 1;
    public byte damage;
    public bool fighting;
    public Inventory.InventoryType invType;
    public bool empty = true;
    public int level;
    public int cost;
    public string itemName;
    public enum Type
    {
        Pickaxe,
        Block,
        Post,
        Sword,
        Bow
    }
    public void ChangeValues(ItemReference newScript)
    {
        itemSprite = newScript.itemSprite;
        stackSize = newScript.stackSize;
        baseDurability = newScript.baseDurability;
        durability = newScript.durability;
        currentStack = newScript.currentStack;
        damage = newScript.damage;
        fighting = newScript.fighting;
        invType = newScript.invType;
        empty = newScript.empty;
        level = newScript.level;
        cost = newScript.cost;
    }
    public void SetValues(InventoryItem newScript)
    {
        itemSprite = newScript.itemSprite;
        stackSize = newScript.stackSize;
        baseDurability = newScript.baseDurability;
        durability = newScript.durability;
        currentStack = newScript.currentStack;
        damage = newScript.damage;
        fighting = newScript.fighting;
        invType = newScript.invType;
        level = newScript.level;
        cost = newScript.cost;
    }
}
