using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class ItemRotator : MonoBehaviour
{
    Sprite fullRotator;
    public ItemSlot[] itemSlots = new ItemSlot[7];
    public List<Image> images = new List<Image>();
    public Sprite[] fullRotators = new Sprite[7];
    public Sprite smallRotator;
    public Image centralImage;
    PlayerControls controls;
    bool open = false;
    public FreePlayerMove playerMovement;
    public int current = 0;
    Image rotatorImage;
    public Inventory inv;
    GameManager manager;
    [HideInInspector]
    public int rotator = 0;
    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!started)
            StartMethod();
    }

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
        fullRotator = fullRotators[current];
        centralImage.sprite = itemSlots[current].getSprite();
        started = true;
        current = 0;
    }
    /// <summary>
    /// Expands item rotator when 'Tab' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void ExpandRotator(CallbackContext ctx)
    {
        rotatorImage.sprite = fullRotator;
        centralImage.gameObject.SetActive(false);
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
        rotatorImage.sprite = smallRotator;
        centralImage.gameObject.SetActive(true);
        open = false;
    }
    void FixedUpdate()
    {
        if (open)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            float angleRad = Mathf.Atan2(mousePos.y-transform.position.y, mousePos.x-transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            if (angleDeg <= 90 && angleDeg > 52)
            {
                current = 0;
            }
            else if (angleDeg > -13 && angleDeg < 52)
            {
                current = 1;
            }
            else if (angleDeg <= -13 && angleDeg > -64)
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
            fullRotator = fullRotators[current];
            rotatorImage.sprite = fullRotator;
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
        fullRotator = fullRotators[current];
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
