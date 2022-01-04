﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class Inventory : MonoBehaviour
{
    private GameManager manager;
    private GameObject[,] images = new GameObject[5, 7];
    public GameObject[] chosenImages = new GameObject[7];
    private ItemSlot[,] itemSlots = new ItemSlot[5, 7];
    [HideInInspector]
    public List<Vector2Int> chosenItems = new List<Vector2Int>();
    public ItemRotator itemRotator;
    void Awake()
    {
        chosenItems.Capacity = 7;
        for (int i = 0; i < 7; i++)
        {
            chosenItems.Add(new Vector2Int(int.MaxValue, int.MaxValue));
        }
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        manager.LoadWorld();
        manager.inv = this;
        int imgnum = 0;
        foreach (ImageMover imageMover in GetComponentsInChildren<ImageMover>())
        {
            if (imgnum >= 35)
                break;
            int row = imgnum / 7;
            int col = imgnum % 7;
            images[row, col] = imageMover.gameObject;
            imageMover.SetArrayPos(new Vector2Int(row, col));
            imgnum++;
        }
        for (int i = 0; i < 5; i++)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    itemSlots[row, col] = new ItemSlot();
                }
            }
        }
        if (manager.loadFromFile)
        {
            LoadFromFile(manager.GetGameInformation());
        } else
        {
            InventoryItem item = manager.GetItem("BasePickaxe");
            AddItem(item,1,item.baseDurability);
            item = manager.GetItem("ExtendoSword");
            AddItem(item,1, item.baseDurability);
        }
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Adds item of ID to inventory
    /// </summary>
    /// <param name="ID">Item ID to add to inventory</param>
    public void AddItem(byte ID)
    {
        InventoryItem newItem = manager.GetItem(ID);
        AddItem(newItem, 1, newItem.baseDurability);
    }
    /// <summary>
    /// Adds item based on item script
    /// </summary>
    /// <param name="itemRef">Script of item to add to inventory</param>
    public void AddItem(InventoryItem itemRef, int currentStack, int durability)
    {
        bool found = false;
        Vector2Int spot = Vector2Int.zero;
        Vector2Int empty = Vector2Int.zero;
        bool emptySpot = false;
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                if (itemRef.itemID == itemSlots[row, col].getItemId() && !itemSlots[row, col].isFull())
                {
                    spot = new Vector2Int(row, col);
                    found = true;
                    int leftOver = itemSlots[spot.x, spot.y].addToStack((byte)currentStack);
                    images[spot.x, spot.y].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[spot.x, spot.y].getCurrentCount());
                    if (leftOver > 0)
                        AddItem(itemRef, leftOver, durability);
                    break;
                }
                if (itemSlots[row,col].isEmpty() && !emptySpot)
                {
                    empty = new Vector2Int(row, col);
                    emptySpot = true;
                }
            }
            if (found)
                break;
        }
        if (emptySpot)
        {
            itemSlots[empty.x, empty.y].addItem(itemRef, (byte)currentStack, (byte)durability);
            images[empty.x, empty.y].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[empty.x, empty.y].getCurrentCount());
            UpdateImage(new Vector2Int(empty.x, empty.y), itemRef.itemSprite);
        }
    }
    /// <summary>
    /// Updates image at given position
    /// </summary>
    /// <param name="newPos">Image position</param>
    /// <param name="itemSprite">New sprite</param>
    void UpdateImage(Vector2Int newPos, Sprite itemSprite)
    {
        images[newPos.x, newPos.y].GetComponent<Image>().sprite = itemSprite;
        if (itemSprite != null)
            images[newPos.x, newPos.y].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        else
            images[newPos.x, newPos.y].GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }
    private void UpdateChosen(int pos, Sprite itemSprite)
    {
        chosenImages[pos].GetComponent<Image>().sprite = itemSprite;
        if (itemSprite != null)
            chosenImages[pos].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        else
            chosenImages[pos].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        itemRotator.UpdateItems();
    }
    public void ReduceChosen(int pos)
    {
        Vector2Int chosenItemPos = chosenItems[pos];
        if (itemSlots[chosenItemPos.x, chosenItemPos.y].GetItemType() == InventoryItem.ItemType.Consumable)
        {
            itemSlots[chosenItemPos.x, chosenItemPos.y].reduceStack(1);
            images[chosenItemPos.x, chosenItemPos.y].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[chosenItemPos.x, chosenItemPos.y].getCurrentCount());
            bool empty = itemSlots[chosenItemPos.x, chosenItemPos.y].isEmpty();
            if (empty)
            {
                RemoveChosen(pos);
            }
        } 
        else
        {
            itemSlots[chosenItemPos.x, chosenItemPos.y].reduceDurability(1);
            bool empty = itemSlots[chosenItemPos.x, chosenItemPos.y].isEmpty();
            if (empty)
            {
                RemoveChosen(pos);
            }
        }
    }
    private void RemoveChosen(int pos)
    {
        Vector2Int chosenItemPos = chosenItems[pos];
        itemSlots[chosenItemPos.x, chosenItemPos.y].emptySlot();
        images[chosenItemPos.x, chosenItemPos.y].gameObject.GetComponent<ImageMover>().UpdateCount(0);
        UpdateImage(chosenItemPos, null);
        UpdateChosen(pos, null);
        chosenItems[pos] = new Vector2Int(int.MaxValue, int.MaxValue);
    }
    /// <summary>
    /// Moves item to new location in inventory
    /// </summary>
    /// <param name="arrayPos">Original position of item</param>
    /// <param name="dropPos">New position of item</param>
    public void DropItem(Vector2Int arrayPos, Vector2Int dropPos)
    {
        if (IsEmpty(arrayPos))
        {
            return;
        }
        if (dropPos.x >= 10)
        {
            ItemSlot slot = itemSlots[arrayPos.x, arrayPos.y];
            int chosenType = dropPos.x / 10;
            int chosenSpot = dropPos.y;
            switch(slot.GetItemType())
            {
                case InventoryItem.ItemType.Mineral:
                    return;
                default:
                    if (chosenItems.Contains(arrayPos))
                    {
                        chosenItems[chosenItems.IndexOf(arrayPos)] = new Vector2Int(int.MaxValue, int.MaxValue);
                    }
                    chosenItems[chosenSpot] = arrayPos;
                    UpdateChosen(chosenSpot, slot.getSprite());
                    break;
            }
            return;
        }
        if (IsEmpty(dropPos))
        {
            itemSlots[dropPos.x, dropPos.y].addExisting(itemSlots[arrayPos.x, arrayPos.y]);
            itemSlots[arrayPos.x, arrayPos.y].emptySlot();
            if (chosenItems.Contains(arrayPos))
            {
                chosenItems[chosenItems.IndexOf(arrayPos)] = dropPos;
            }
        }
    }
    /// <summary>
    /// Determines if inventory slot is empty
    /// </summary>
    /// <param name="arrayPos">Inventory position</param>
    /// <returns></returns>
    public bool IsEmpty(Vector2Int arrayPos)
    {
        if (images[arrayPos.x, arrayPos.y].GetComponent<Image>().sprite == null)
            return true;
        else
            return false;
    }

    private void LoadFromFile(GameInformation info)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                byte infoItem = info.inventory[i, j];
                if (infoItem != 127)
                {
                    InventoryItem item = manager.GetItem(infoItem);
                    AddItem(item, info.stackSize[i, j], info.durability[i, j]);
                }
            }
        }
        for (int i = 0; i < 7; i++)
        {
            chosenItems[i] = new Vector2Int(info.chosenItems[i, 0], info.chosenItems[i, 1]);
            if (chosenItems[i] != new Vector2Int(int.MaxValue, int.MaxValue))
                UpdateChosen(i, itemSlots[chosenItems[i].x, chosenItems[i].y].getSprite());
        }
        itemRotator.UpdateItems();
    }
    public ItemSlot getItemSlot(int row, int col)
    {
        return itemSlots[row, col];
    }
}
