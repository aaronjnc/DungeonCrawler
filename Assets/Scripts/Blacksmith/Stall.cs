using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles all stall information, is attached to interactable object
/// </summary>
public class Stall : MonoBehaviour
{
    //type of stall
    public enum StallType
    {
        Blacksmith,
        Wizard,
    }
    [Tooltip("Stall type")]
    public StallType stallType;
    [Tooltip("Items in stall")]
    public List<ItemSlot> stallItems = new List<ItemSlot>();
    /// <summary>
    /// call methods based on type of stall upon awake
    /// </summary>
    private void Awake()
    {
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
        foreach (InventoryItem item in GameManager.Instance.GetItemScripts())
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
