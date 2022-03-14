using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySave
{
    [Tooltip("Array of bytes representing inventory")]
    private byte[,] inventory = new byte[5, 7];
    [Tooltip("Array of bytes representing stack size of inventory slots")]
    private byte[,] stackSize = new byte[5, 7];
    [Tooltip("Array of bytes representing durability of inventory slots")]
    private byte[,] durability = new byte[5, 7];
    [Tooltip("Array of integers representing chosen item positions")]
    private int[,] chosenItems = new int[7, 2];
    [Tooltip("Player money")]
    private int playerMoney;
    [Tooltip("Current chosen item")]
    private int currentChoice;
    /// <summary>
    /// Saves inventory information
    /// </summary>
    /// <param name="inv"></param>
    public InventorySave(Inventory inv)
    {
        UpdateInventory(inv.GetInventory());
        for (int i = 0; i < 7; i++)
        {
            chosenItems[i, 0] = inv.chosenItems[i].x;
            chosenItems[i, 1] = inv.chosenItems[i].y;
        }
        currentChoice = inv.itemRotator.current;
        playerMoney = inv.GetMoney();
    }
    /// <summary>
    /// Update inventory save with item slots
    /// </summary>
    /// <param name="items"></param>
    private void UpdateInventory(ItemSlot[,] items)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (!items[i, j].IsEmpty())
                {
                    inventory[i, j] = items[i, j].GetItemID();
                    stackSize[i, j] = items[i, j].GetCount();
                    durability[i, j] = items[i, j].GetDurability();
                }
                else
                {
                    inventory[i, j] = 127;
                }
            }
        }
    }
    /// <summary>
    /// Returns array of items
    /// </summary>
    /// <returns></returns>
    public byte[,] GetItems()
    {
        return inventory;
    }
    /// <summary>
    /// Returns array of stack sizes
    /// </summary>
    /// <returns></returns>
    public byte[,] GetStackSizes()
    {
        return stackSize;
    }
    /// <summary>
    /// Returns array of durabilities
    /// </summary>
    /// <returns></returns>
    public byte[,] GetDurabilities()
    {
        return durability;
    }
    /// <summary>
    /// Returns array of chosen item positions
    /// </summary>
    /// <returns></returns>
    public int[,] GetChosenItems()
    {
        return chosenItems;
    }
    /// <summary>
    /// Returns player money
    /// </summary>
    /// <returns></returns>
    public int GetMoney()
    {
        return playerMoney;
    }
    /// <summary>
    /// Returns current chosen item
    /// </summary>
    /// <returns></returns>
    public int GetCurrentChoice()
    {
        return currentChoice;
    }
}
