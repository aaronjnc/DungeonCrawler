using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryChange : MonoBehaviour, IPointerDownHandler
{
    public GameManager manager;
    public InventoryChange[] otherInv = new InventoryChange[4];
    public Sprite clicked;
    public Sprite unclicked;
    Inventory inventory;
    public Inventory.InventoryType invType;
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        inventory = manager.inv;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = clicked;
        foreach(InventoryChange inv in otherInv)
        {
            inv.ChangeSprite();
        }
        inventory.ChangeInventory(invType);
    }
    public void ChangeSprite()
    {
        GetComponent<Image>().sprite = unclicked;
    }
}
