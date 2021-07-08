using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor
{
    public ItemReference[,,,] vendorItems = new ItemReference[5, 3, 3, 5];
    GameManager manager;
    List<InventoryItem>[] separatedItems = new List<InventoryItem>[5];
    float[] chance = new float[] { .8f,.1f,.01f,.001f,.0001f};
    int[] itemCount = new int[] { 0, 0, 0, 0, 0 };
    bool[] full = new bool[] { false, false, false, false, false };
    /// <summary>
    /// Sets up vendor script and generates new items
    /// </summary>
    public Vendor()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        for (int i = 0; i < 5; i++)
        {
            separatedItems[i] = new List<InventoryItem>();
        }
        Restock();
    }
    /// <summary>
    /// Refreshes item list separated by level
    /// </summary>
    void RefreshItemLevels()
    {
        List<InventoryItem> invItems = manager.items;
        foreach (InventoryItem item in invItems)
        {
            separatedItems[item.level].Add(item);
        }
    }
    /// <summary>
    /// Clears vendor items
    /// </summary>
    void RefreshItems()
    {
        for (int tab = 0; tab < 5; tab++)
        {
            for (int page = 0; page < 3; page++)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        vendorItems[tab, page, row, col] = new ItemReference();
                    }
                }
            }
        }
    }
    /// <summary>
    /// Updates vendor item with new item script
    /// </summary>
    /// <param name="itemRef">New item script</param>
    /// <param name="itemPos">Item position</param>
    public void UpdateItem(ItemReference itemRef, Vector4 itemPos)
    {
        vendorItems[(int)itemPos.x, (int)itemPos.y, (int)itemPos.z, (int)itemPos.w].ChangeValues(itemRef);
        if (vendorItems[(int)itemPos.x, (int)itemPos.y, (int)itemPos.z, (int)itemPos.w].itemSprite == null)
        {
            itemCount[(int)itemPos.x]--;
            full[(int)itemPos.x] = false;
        }
    }
    /// <summary>
    /// Clear vendor inventory and generate all new items
    /// </summary>
    public void Restock()
    {
        RefreshItemLevels();
        RefreshItems();
        List<InventoryItem> vendorList = new List<InventoryItem>();
        for (int itemCount = 0; itemCount < 20; itemCount++)
        {
            float rand = Random.value;
            for (int i = 4; i>=0;i--)
            {
                if (rand < chance[i])
                {
                    int random = Random.Range(0, separatedItems[i].Count);
                    if (random < separatedItems[i].Count)
                    {
                        vendorList.Add(separatedItems[i][random]);
                        separatedItems[i].RemoveAt(random);
                    }
                }
            }
        }
        foreach (InventoryItem vendorItem in vendorList)
        {
            AddItem(vendorItem);
        }
    }
    /// <summary>
    /// Adds items to inventory given item script
    /// </summary>
    /// <param name="itemRef">New item script</param>
    public void AddItem(ItemReference itemRef)
    {
        int tab = 0;
        switch (itemRef.invType)
        {
            case Inventory.InventoryType.Weapons:
                tab = 0;
                break;
            case Inventory.InventoryType.Blocks:
                tab = 1;
                break;
            case Inventory.InventoryType.Tools:
                tab = 2;
                break;
            case Inventory.InventoryType.Food:
                tab = 3;
                break;
            case Inventory.InventoryType.Armor:
                tab = 4;
                break;
        }
        int tabCount = itemCount[tab];
        int page = (int)(tabCount / 15);
        int marketrow = (tabCount % 15) / 5;
        int marketcol = (tabCount % 15) % 5;
        vendorItems[tab, page, marketrow, marketcol].ChangeValues(itemRef);
        if (!full[tab])
            itemCount[tab]++;
        if (itemCount[tab] == 35)
            full[tab] = true;
    }
    /// <summary>
    /// Converts inventoryitem script to add
    /// </summary>
    /// <param name="newItem">New item script</param>
    public void AddItem(InventoryItem newItem)
    {
        ItemReference itemRef = new ItemReference();
        itemRef.SetValues(newItem);
        AddItem(itemRef);  
    }
}
