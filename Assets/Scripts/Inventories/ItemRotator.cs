using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class ItemRotator : MonoBehaviour
{
    public Image itemRotator;
    //slots for expanded rotator
    public ItemSlot[] itemSlots = new ItemSlot[7];
    public Sprite[] fullRotators = new Sprite[7];
    //images for expanded rotator
    public List<Image> images = new List<Image>();
    //sprite for minimized rotator
    public Sprite smallRotator;
    //image slot for chosen item
    public Image centralImage;
    //player controls reference
    PlayerControls controls;
    //true if rotator is expanded
    bool open = false;
    //reference to player movement script
    public FreePlayerMove playerMovement;
    //chosen item index
    public int current = 0;
    //image attached to game object in scene for rotator
    Image rotatorImage;
    //inventory reference
    public Inventory inv;
    //game manager reference
    GameManager manager;
    //has rotator been instantiated
    bool started = false;
    public SpriteRenderer playerRenderer;
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
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        rotatorImage = GetComponent<Image>();
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
        centralImage.sprite = itemSlots[current].getSprite();
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
            centralImage.sprite = itemSlots[current].getSprite();
            if (itemSlots[current].getSprite() != null)
            {
                centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);

            }
            else
            {
                centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 0);
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
            itemSlots[i].addExisting(inv.getItemSlot(chosenItemPos.x, chosenItemPos.y));
            images[i].sprite = itemSlots[i].getSprite();
            if (images[i].sprite != null)
                images[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            else
                images[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        centralImage.sprite = itemSlots[current].getSprite();
        itemRotator.sprite = fullRotators[current];
        if (itemSlots[current].getSprite() != null)
        {
            centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
        else
        {
            centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        CurrentItem();
    }
    /// <summary>
    /// Sets certain values given currently selected item
    /// </summary>
    void CurrentItem()
    {
        manager.currentItem.addExisting(itemSlots[current]);
        if (!itemSlots[current].isEmpty())
        {
            DisableBlockPlacing();
            manager.fighting = false;
            switch (itemSlots[current].GetItemType())
            {
                case InventoryItem.ItemType.Weapon:
                    manager.fighting = true;
                    itemSlots[current].getWeaponScript().Pickup(playerRenderer);
                    break;
                case InventoryItem.ItemType.Consumable:
                    //consumable
                    break;
                case InventoryItem.ItemType.Tool:
                    manager.blockBreaking = true;
                    break;
            }
        }
        else
        {
            DisableBlockPlacing();
            manager.fighting = false;
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
        manager.ResetPreviousTile();
        manager.blockBreaking = false;
    }

    private void OnDestroy()
    {
        controls.Disable();
    }
}
