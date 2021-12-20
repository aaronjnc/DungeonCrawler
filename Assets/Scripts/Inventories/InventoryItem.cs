using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public Sprite itemSprite;
    public byte stackSize;
    public byte baseDurability;
    public byte durability;
    public byte currentStack = 1;
    public byte damage;
    public bool fighting;
    public bool empty = true;
    public int level;
    public int cost;
    public byte itemID;
    public WeaponInterface<Transform,PlayerFight> weaponScript { get { return (WeaponInterface<Transform, PlayerFight>)gameObject.GetComponent(typeof(WeaponInterface<Transform, PlayerFight>)); } }
    public enum Type
    {
        Pickaxe,
        Block,
        Post,
        Sword,
        Bow
    }
}