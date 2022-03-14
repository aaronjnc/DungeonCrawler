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
    [Tooltip("Player controls")]
    private PlayerControls controls;
    [Tooltip("Stall type")]
    public Stall.StallType stallType;
    [Tooltip("Stall images")]
    [SerializeField] private Image[] images;
    [Tooltip("List of items in the stall")]
    [SerializeField] private List<ItemSlot> stallitems = new List<ItemSlot>();
    [Tooltip("Chosen item")]
    [SerializeField] private ChosenItem chosenItem;
    [Tooltip("Inventory object")]
    [SerializeField] private GameObject inventory;
    [Tooltip("Dialog object")]
    [SerializeField] private GameObject dialog;
    /// <summary>
    /// Adds new item slots to stall and deactivates image slots
    /// </summary>
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Interact.Inventory.performed += EnableInventory;
        controls.Interact.Inventory.Enable();
        for (int i = 0; i < images.Length; i++)
        {
            stallitems.Add(new ItemSlot());
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
                stallitems[i].AddExisting(items[i]);
                images[i].sprite = stallitems[i].GetSprite();
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
        chosenItem.ChooseItem(stallitems[idx]);
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
