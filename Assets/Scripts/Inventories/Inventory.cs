﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class Inventory : Singleton<Inventory>
{
    [Tooltip("Amount of money player has")]
    private int playerMoney = 0;
    [Tooltip("Array of inventory item game objects")]
    private GameObject[,] images = new GameObject[5, 7];
    [Tooltip("Array of chosen item game objects")]
    [SerializeField] private GameObject[] chosenImages = new GameObject[7];
    [Tooltip("Array of inventory item slots")]
    private ItemSlot[,] itemSlots = new ItemSlot[5, 7];
    [Tooltip("Inventory positions of chosen items")]
    [HideInInspector] public List<Vector2Int> chosenItems = new List<Vector2Int>();
    [Tooltip("Item rotator script")]
    public ItemRotator itemRotator;
    [Tooltip("Inventory money text boxes")]
    [SerializeField] private Text[] moneyObjects;
    /// <summary>
    /// Sets up inventory
    /// </summary>
    void Awake()
    {
        base.Awake();
        chosenItems.Capacity = 7;
        for (int i = 0; i < 7; i++)
        {
            chosenItems.Add(new Vector2Int(int.MaxValue, int.MaxValue));
        }
        GameManager.Instance.SetValues();
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
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                itemSlots[row, col] = new ItemSlot();
            }
        }
        if (GameManager.Instance.reopen)
        {
            Reopen();
        }
        else if (GameManager.Instance.loadFromFile)
        {
            LoadFromFile();
        } 
        else
        {
            InventoryItem item = GameManager.Instance.GetItem("Base Pickaxe");
            AddItem(item,1,item.baseDurability);
            AddMoney(150);
        }
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Adds item of ID to inventory
    /// </summary>
    /// <param name="ID">Item ID to add to inventory</param>
    public void AddItem(byte ID)
    {
        InventoryItem newItem = GameManager.Instance.GetItem(ID);
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
                if (itemRef.itemID == itemSlots[row, col].GetItemID() && !itemSlots[row, col].IsFull())
                {
                    spot = new Vector2Int(row, col);
                    found = true;
                    int leftOver = itemSlots[spot.x, spot.y].AddToStack((byte)currentStack);
                    images[spot.x, spot.y].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[spot.x, spot.y].GetCount());
                    if (leftOver > 0)
                        AddItem(itemRef, leftOver, durability);
                    break;
                }
                if (itemSlots[row,col].IsEmpty() && !emptySpot)
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
            itemSlots[empty.x, empty.y].AddItem(itemRef, (byte)currentStack, (byte)durability);
            images[empty.x, empty.y].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[empty.x, empty.y].GetCount());
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
    /// <summary>
    /// Update chosen image
    /// </summary>
    /// <param name="pos">chosen image to update</param>
    /// <param name="itemSprite">new sprite</param>
    private void UpdateChosen(int pos, Sprite itemSprite)
    {
        chosenImages[pos].GetComponent<Image>().sprite = itemSprite;
        if (itemSprite != null)
            chosenImages[pos].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        else
            chosenImages[pos].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        itemRotator.UpdateItems();
    }
    /// <summary>
    /// reduce the count of chosen item
    /// </summary>
    /// <param name="pos">position of chosen item</param>
    public void ReduceChosen(int pos)
    {
        Vector2Int chosenItemPos = chosenItems[pos];
        if (itemSlots[chosenItemPos.x, chosenItemPos.y].GetItemType() == InventoryItem.ItemType.Consumable)
        {
            itemSlots[chosenItemPos.x, chosenItemPos.y].ReduceStack(1);
            images[chosenItemPos.x, chosenItemPos.y].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[chosenItemPos.x, chosenItemPos.y].GetCount());
            bool empty = itemSlots[chosenItemPos.x, chosenItemPos.y].IsEmpty();
            if (empty)
            {
                RemoveChosen(pos);
            }
        } 
        else
        {
            itemSlots[chosenItemPos.x, chosenItemPos.y].ReduceDurability(1);
            bool empty = itemSlots[chosenItemPos.x, chosenItemPos.y].IsEmpty();
            if (empty)
            {
                RemoveChosen(pos);
            }
        }
    }
    /// <summary>
    /// removes a chosen item from the inventory
    /// </summary>
    /// <param name="pos">position of chosen item</param>
    private void RemoveChosen(int pos)
    {
        Vector2Int chosenItemPos = chosenItems[pos];
        itemSlots[chosenItemPos.x, chosenItemPos.y].EmptySlot();
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
            int chosenSpot = dropPos.y;
            switch(slot.GetItemType())
            {
                case InventoryItem.ItemType.Mineral:
                    return;
                default:
                    if (chosenItems.Contains(arrayPos))
                    {
                        int previousChosen = chosenItems.IndexOf(arrayPos);
                        chosenItems[previousChosen] = new Vector2Int(int.MaxValue, int.MaxValue);
                        UpdateChosen(previousChosen, null);
                    }
                    chosenItems[chosenSpot] = arrayPos;
                    UpdateChosen(chosenSpot, slot.GetSprite());
                    break;
            }
            return;
        }
        if (IsEmpty(dropPos))
        {
            itemSlots[dropPos.x, dropPos.y].AddExisting(itemSlots[arrayPos.x, arrayPos.y]);
            itemSlots[arrayPos.x, arrayPos.y].EmptySlot();
            images[dropPos.x, dropPos.y].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[dropPos.x, dropPos.y].GetCount());
            images[arrayPos.x, arrayPos.y].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[arrayPos.x, arrayPos.y].GetCount());
            UpdateImage(dropPos, itemSlots[dropPos.x, dropPos.y].GetSprite());
            UpdateImage(arrayPos, null);
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
    /// <summary>
    /// loads inventory from file
    /// </summary>
    /// <param name="info">GameInformation file to get inventory from</param>
    private void LoadFromFile()
    {
        InventorySave inv = GameInformation.Instance.LoadInventory();
        byte[,] items = inv.GetItems();
        byte[,] sizes = inv.GetStackSizes();
        byte[,] durabilities = inv.GetDurabilities();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                byte infoItem = items[i, j];
                if (infoItem != 127)
                {
                    InventoryItem item = GameManager.Instance.GetItem(infoItem);
                    AddItem(item, sizes[i, j], durabilities[i, j]);
                }
            }
        }
        int[,] chosen = inv.GetChosenItems();
        for (int i = 0; i < 7; i++)
        {
            chosenItems[i] = new Vector2Int(chosen[i, 0], chosen[i, 1]);
            if (chosenItems[i] != new Vector2Int(int.MaxValue, int.MaxValue))
                UpdateChosen(i, itemSlots[chosenItems[i].x, chosenItems[i].y].GetSprite());
        }
        itemRotator.UpdateItems();
        AddMoney(inv.GetMoney());
        UpdateMoney();
    }
    /// <summary>
    /// returns ItemSlot at given position
    /// </summary>
    /// <param name="row">array x pos</param>
    /// <param name="col">array y pos</param>
    /// <returns>ItemSlot at position</returns>
    public ItemSlot getItemSlot(int row, int col)
    {
        return itemSlots[row, col];
    }
    /// <summary>
    /// Adds given amount to player money
    /// </summary>
    /// <param name="num"></param>
    public void AddMoney(int num)
    {
        playerMoney += num;
        UpdateMoney();
    }
    /// <summary>
    /// Returns amount of money
    /// </summary>
    /// <returns></returns>
    public int GetMoney()
    {
        return playerMoney;
    }
    /// <summary>
    /// Updates the money text objects
    /// </summary>
    public void UpdateMoney()
    {
        int emerald = playerMoney / 1000;
        int leftOver = playerMoney % 1000;
        int gold = leftOver / 100;
        leftOver %= 100;
        int silver = leftOver / 10;
        leftOver %= 10;
        int bronze = leftOver;
        moneyObjects[0].text = emerald + "";
        moneyObjects[1].text = gold + "";
        moneyObjects[2].text = silver + "";
        moneyObjects[3].text = bronze + "";
    }
    /// <summary>
    /// Returns array of inventory items
    /// </summary>
    /// <returns></returns>
    public ItemSlot[,] GetInventory()
    {
        return itemSlots;
    }
    /// <summary>
    /// Reopen inventory, loads from game manager
    /// </summary>
    public void Reopen()
    {
        InventorySave inv = GameInformation.Instance.LoadInventory();
        ItemSlot[,] slot = GameManager.Instance.GetInventory();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                itemSlots[i, j].AddExisting(slot[i, j]);
                images[i, j].gameObject.GetComponent<ImageMover>().UpdateCount(itemSlots[i, j].GetCount());
                UpdateImage(new Vector2Int(i, j), itemSlots[i, j].GetSprite());
            }
        }
        int[,] chosen = inv.GetChosenItems();
        for (int i = 0; i < 7; i++)
        {
            chosenItems[i] = new Vector2Int(chosen[i, 0], chosen[i, 1]);
            if (chosenItems[i] != new Vector2Int(int.MaxValue, int.MaxValue))
                UpdateChosen(i, itemSlots[chosenItems[i].x, chosenItems[i].y].GetSprite());
        }
        itemRotator.UpdateItems();
        AddMoney(GameManager.Instance.GetMoney());
        UpdateMoney();
        GameManager.Instance.reopen = false;
    }
    /// <summary>
    /// Method called when inventory is destroyed (scene change)
    /// </summary>
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StoreInventory(itemSlots);
            GameManager.Instance.SetMoney(playerMoney);
        }
    }
}
