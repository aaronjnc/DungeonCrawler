using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySave
{
    public byte[,] inventory = new byte[5, 7];
    public byte[,] stackSize = new byte[5, 7];
    public byte[,] durability = new byte[5, 7];
    public int[,] chosenItems = new int[7, 2];
    public int playerMoney;
    public int currentChoice;
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

    private void UpdateInventory(ItemSlot[,] items)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (!items[i, j].isEmpty())
                {
                    inventory[i, j] = items[i, j].getItemId();
                    stackSize[i, j] = items[i, j].getCurrentCount();
                    durability[i, j] = items[i, j].getDurability();
                }
                else
                {
                    inventory[i, j] = 127;
                }
            }
        }
    }
    public byte[,] GetItems()
    {
        return inventory;
    }
    public byte[,] GetStackSizes()
    {
        return stackSize;
    }
    public byte[,] GetDurabilities()
    {
        return durability;
    }
    public int[,] GetChosenItems()
    {
        return chosenItems;
    }
    public int GetMoney()
    {
        return playerMoney;
    }
    public int GetCurrentChoice()
    {
        return currentChoice;
    }
}
