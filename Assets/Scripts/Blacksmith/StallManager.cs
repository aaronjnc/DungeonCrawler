using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StallManager : MonoBehaviour
{
    GameManager manager;
    public Stall.StallType stallType;
    public Image[] images;
    public List<ItemSlot> inventoryItems = new List<ItemSlot>();
    public ChosenItem chosenItem;
    private void Awake()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        for (int i = 0; i < images.Length; i++)
        {
            inventoryItems.Add(new ItemSlot());
            images[i].gameObject.SetActive(false);
        }
    }
    public void AddItems(List<ItemSlot> items)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (i < items.Count)
            {
                inventoryItems[i].addExisting(items[i]);
                images[i].sprite = inventoryItems[i].getSprite();
                images[i].gameObject.SetActive(true);
            }
            else
            {
                images[i].sprite = null;
                images[i].gameObject.SetActive(false);
            }
        }
    }
    public void ChooseItem(int i)
    {
        chosenItem.ChooseItem(inventoryItems[i]);
    }
}
