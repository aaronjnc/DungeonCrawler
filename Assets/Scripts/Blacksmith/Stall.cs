using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stall : MonoBehaviour
{
    GameManager manager;
    public enum StallType
    {
        Blacksmith,
    }
    public StallType stallType;
    public List<ItemSlot> stallItems = new List<ItemSlot>();
    private void Awake()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        switch (stallType)
        {
            case StallType.Blacksmith:
                AwakeBlacksmith();
                break;
        }
    }
    private void AwakeBlacksmith()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        foreach (InventoryItem item in manager.GetItemScripts())
        {
            if (item.itemType == InventoryItem.ItemType.Mineral)
                continue;
            items.Add(item);
        }
        for (int i = 0; i < 7; i++)
        {
            stallItems.Add(new ItemSlot());
            foreach (InventoryItem item in items)
            {
                
                stallItems[i].AddItem(item);
                items.Remove(item);
                break;
            }
        }
    }
}
