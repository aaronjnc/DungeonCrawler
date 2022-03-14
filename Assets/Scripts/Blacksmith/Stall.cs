using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles all stall information, is attached to interactable object
/// </summary>
public class Stall : MonoBehaviour
{
    //refernce to manager script
    private GameManager manager;
    //type of stall
    public enum StallType
    {
        Blacksmith,
        Wizard,
    }
    //the type of stall this stall is
    public StallType stallType;
    //list of items in this stall
    public List<ItemSlot> stallItems = new List<ItemSlot>();
    /// <summary>
    /// call methods based on type of stall upon awake
    /// </summary>
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
    /// <summary>
    /// Used when a blacksmith stall is created, adds minerals, weapons, and tools to stall
    /// </summary>
    private void AwakeBlacksmith()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        foreach (InventoryItem item in manager.GetItemScripts())
        {
            if (item.itemType == InventoryItem.ItemType.Consumable)
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
