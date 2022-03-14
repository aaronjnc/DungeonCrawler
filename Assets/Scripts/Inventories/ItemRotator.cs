using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class ItemRotator : MonoBehaviour
{
    [Tooltip("Image of item rotator")]
    [SerializeField] private Image itemRotator;
    [Tooltip("Item slots of item rotator")]
    [SerializeField] private ItemSlot[] itemSlots = new ItemSlot[7];
    [Tooltip("Sprites for different item rotator stages")]
    [SerializeField] private Sprite[] fullRotators = new Sprite[7];
    [Tooltip("Images for chosen items on rotator")]
    [SerializeField] private List<Image> images = new List<Image>();
    [Tooltip("Image for chosen item")]
    [SerializeField] private Image chosenItem;
    [Tooltip("Player controls")]
    private PlayerControls controls;
    [Tooltip("Item rotator is expanded")]
    private bool open = false;
    [Tooltip("Player movement script")]
    [SerializeField] private FreePlayerMove playerMovement;
    [Tooltip("Index of chosen item in inventory chosen items")]
    public int current = 0;
    [Tooltip("Player inventory")]
    [SerializeField] private Inventory inv;
    [Tooltip("Rotator has been instantiated")]
    private bool started = false;
    [Tooltip("Sprite renderer of player")]
    [SerializeField] private SpriteRenderer playerRenderer;
    /// <summary>
    /// call start method on start up
    /// </summary>
    void Start()
    {
        if (!started)
            StartMethod();
    }
    /// <summary>
    /// sets up item rotator
    /// </summary>
    void StartMethod()
    {
        controls = new PlayerControls();
        foreach (Image img in images)
        {
            img.gameObject.SetActive(false);
        }
        for (int i = 0; i < 7; i++)
        {
            itemSlots[i] = new ItemSlot();
        }
        controls.Inventory.ItemRotator.performed += ExpandRotator;
        controls.Inventory.ItemRotator.canceled += MinimizeRotator;
        controls.Inventory.ItemRotator.Enable();
        controls.Movement.MousePosition.Enable();
        chosenItem.sprite = itemSlots[current].GetSprite();
        itemRotator.gameObject.SetActive(false);
        started = true;
        current = 0;
    }
    /// <summary>
    /// Expands item rotator when 'Tab' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void ExpandRotator(CallbackContext ctx)
    {
        itemRotator.gameObject.SetActive(true);
        foreach(Image img in images)
        {
            img.gameObject.SetActive(true);
        }
        open = true;
    }
    /// <summary>
    /// Minimizes item rotator when 'Tab' is released
    /// </summary>
    /// <param name="ctx"></param>
    void MinimizeRotator(CallbackContext ctx)
    {
        foreach (Image img in images)
        {
            img.gameObject.SetActive(false);
        }
        itemRotator.gameObject.SetActive(false);
        open = false;
    }
    /// <summary>
    /// update for when rotator is open to determine chosen item
    /// </summary>
    void FixedUpdate()
    {
        if (open)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue() - new Vector2(Screen.width / 2, Screen.height / 2);
            float angleDeg = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            if (angleDeg <= 90 && angleDeg > 39)
            {
                current = 0;
            }
            else if (angleDeg > -12 && angleDeg < 39)
            {
                current = 1;
            }
            else if (angleDeg <= -12 && angleDeg > -63)
            {
                current = 2;
            }
            else if (angleDeg <= -64 && angleDeg > -115)
            {
                current = 3;
            }
            else if (angleDeg <= -115 && angleDeg > -166)
            {
                current = 4;
            }
            else if (angleDeg > 90 && angleDeg <= 141)
            {
                current = 6;
            }
            else
            {
                current = 5;
            }
            itemRotator.sprite = fullRotators[current];
            chosenItem.sprite = itemSlots[current].GetSprite();
            if (itemSlots[current].GetSprite() != null)
            {
                chosenItem.GetComponent<Image>().color = new Color(255, 255, 255, 255);

            }
            else
            {
                chosenItem.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }
            CurrentItem();
        }
    }
    /// <summary>
    /// Updates items within item rotator given inventory
    /// </summary>
    public void UpdateItems()
    {
        if (!started)
            StartMethod();
        for (int i = 0; i < 7; i++)
        {
            Vector2Int chosenItemPos = inv.chosenItems[i];
            if (chosenItemPos.x == int.MaxValue)
                continue;
            itemSlots[i].AddExisting(inv.getItemSlot(chosenItemPos.x, chosenItemPos.y));
            images[i].sprite = itemSlots[i].GetSprite();
            if (images[i].sprite != null)
                images[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            else
                images[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        chosenItem.sprite = itemSlots[current].GetSprite();
        itemRotator.sprite = fullRotators[current];
        if (itemSlots[current].GetSprite() != null)
        {
            chosenItem.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
        else
        {
            chosenItem.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        CurrentItem();
    }
    /// <summary>
    /// Sets certain values given currently selected item
    /// </summary>
    void CurrentItem()
    {
        GameManager.Instance.currentItem.AddExisting(itemSlots[current]);
        if (!itemSlots[current].IsEmpty())
        {
            DisableBlockPlacing();
            GameManager.Instance.fighting = false;
            switch (itemSlots[current].GetItemType())
            {
                case InventoryItem.ItemType.Weapon:
                    GameManager.Instance.fighting = true;
                    itemSlots[current].GetWeaponScript().Pickup(playerRenderer);
                    break;
                case InventoryItem.ItemType.Consumable:
                    //consumable
                    break;
                case InventoryItem.ItemType.Tool:
                    GameManager.Instance.blockBreaking = true;
                    break;
            }
        }
        else
        {
            DisableBlockPlacing();
            GameManager.Instance.fighting = false;
        }
    }
    /// <summary>
    /// returns chosen ItemSlot
    /// </summary>
    /// <returns>ItemSlot reference for chosen item</returns>
    public ItemSlot getChosen()
    {
        return itemSlots[current];
    }
    /// <summary>
    /// Resets everything when block placing is disabled
    /// </summary>
    public void DisableBlockPlacing()
    {
        GameManager.Instance.DisableBlockBreaking();
        GameManager.Instance.blockBreaking = false;
    }
    /// <summary>
    /// Disable controls on destroy
    /// </summary>
    private void OnDestroy()
    {
        controls.Disable();
    }
}
