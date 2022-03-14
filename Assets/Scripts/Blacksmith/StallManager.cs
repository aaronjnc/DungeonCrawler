using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
/// <summary>
/// Manages the stall in the Dialog scene
/// </summary>
public class StallManager : MonoBehaviour
{
    private PlayerControls controls;
    //the type of stall this is
    public Stall.StallType stallType;
    //array of images to hold items for sale
    public Image[] images;
    //list of items in the stall
    public List<ItemSlot> inventoryItems = new List<ItemSlot>();
    //reference to chosen item script for when item is chosen
    public ChosenItem chosenItem;
    //inventory gameobject
    public GameObject inventory;
    //dialog gameobject
    public GameObject dialog;
    /// <summary>
    /// Adds new item slots to inventory and deactivates image slots
    /// </summary>
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Interact.Inventory.performed += EnableInventory;
        controls.Interact.Inventory.Enable();
        for (int i = 0; i < images.Length; i++)
        {
            inventoryItems.Add(new ItemSlot());
            images[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Enables inventory when 'i' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    private void EnableInventory(CallbackContext ctx)
    {
        if (inventory.activeInHierarchy)
            inventory.SetActive(false);
        else
            inventory.SetActive(true);
    }
    /// <summary>
    /// Adds list of item slots to inventory items and enables the images
    /// </summary>
    /// <param name="items"></param> items to add to the stall
    public void AddItems(List<ItemSlot> items)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (i < items.Count)
            {
                inventoryItems[i].AddExisting(items[i]);
                images[i].sprite = inventoryItems[i].GetSprite();
                images[i].gameObject.SetActive(true);
            }
            else
            {
                images[i].sprite = null;
                images[i].gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Method to choose item with given index
    /// </summary>
    /// <param name="idx"></param> index of chosen item
    public void ChooseItem(int idx)
    {
        chosenItem.ChooseItem(inventoryItems[idx]);
    }
    /// <summary>
    /// Reopens the dialog box
    /// </summary>
    public void ReopenDialog()
    {
        dialog.SetActive(true);
    }
    /// <summary>
    /// Disable controls
    /// </summary>
    private void OnDestroy()
    {
        if (controls != null)
            controls.Disable();
    }
}
