using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyandSell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite clickedSprite;
    Sprite unclickedSprite;
    public bool buy;
    public MarketPlace marketPlace;
    public ItemSlot currentItem;
    public Inventory inventory;
    public MarketPlace otherMarket;
    GameManager manager;
    void Start()
    {
        currentItem = new ItemSlot();
        unclickedSprite = GetComponent<Image>().sprite;
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = clickedSprite;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = unclickedSprite;
        Vector4 loc = new Vector4(marketPlace.currentTab, marketPlace.currentLoc.x, marketPlace.currentLoc.y, marketPlace.currentLoc.z);
        if (buy)
        {
            manager.gold -= currentItem.getCost();
            inventory.AddItem(currentItem.getItemId());
            marketPlace.UpdateImage(new ItemSlot(), loc);
            marketPlace.vendor.UpdateItem(new ItemSlot(), loc);
            marketPlace.RefreshImage(marketPlace.chosenImage, null);
            otherMarket.UpdateItems();
        }
        else
        {
            manager.gold += currentItem.getCost();
            int pos = marketPlace.currentLoc.x * 15 + marketPlace.currentLoc.y * 5 + marketPlace.currentLoc.z;
            //inventory.RemoveChosenItem(marketPlace.currentTab, pos);
            marketPlace.UpdateImage(new ItemSlot(), loc);
            marketPlace.RefreshImage(marketPlace.chosenImage, null);
            otherMarket.vendor.AddItem(currentItem);
            otherMarket.UpdateVendor();
        }
    }
}
