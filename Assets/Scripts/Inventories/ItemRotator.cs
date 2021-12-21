using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class ItemRotator : MonoBehaviour
{
    Sprite fullRotator;
    public ItemSlot[] itemSlots;
    public List<Image> images = new List<Image>();
    //public ItemReference chosenItem;
    public Image centralImage;
    PlayerControls controls;
    bool open = false;
    public FreePlayerMove playerMovement;
    public int current = 0;
    Image rotatorImage;
    SwapRotators swapRotators;
    public Inventory inv;
    GameManager manager;
    [HideInInspector]
    public int rotator = 0;
    bool started = false;
    public enum RotatorType
    {
        Weapon,
        Consumable,
        Tool,
    }
    public RotatorType rotatorType;
    // Start is called before the first frame update
    void Start()
    {
        if (!started)
            StartMethod();
    }

    void StartMethod()
    {
        switch (rotatorType)
        {
            case RotatorType.Consumable:
                itemSlots = new ItemSlot[3];
                break;
            default:
                itemSlots = new ItemSlot[2];
                break;
        }
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        rotatorImage = GetComponent<Image>();
        controls = new PlayerControls();
        swapRotators = GetComponentInParent<SwapRotators>();
        foreach (Image img in images)
        {
            img.gameObject.SetActive(false);
        }
        for (int i = 0; i < (rotatorType == RotatorType.Consumable ? 3 : 2); i++)
        {
            itemSlots[i] = new ItemSlot();
        }
        controls.Inventory.ItemRotator.performed += ExpandRotator;
        controls.Inventory.ItemRotator.canceled += MinimizeRotator;
        controls.Inventory.ItemRotator.Enable();
        controls.Movement.MousePosition.Enable();
        switch (rotatorType)
        {
            case RotatorType.Consumable:
                fullRotator = swapRotators.threeFullRotators[current];
                break;
            default:
                fullRotator = swapRotators.twoFullRotators[current];
                break;
        }
        centralImage.sprite = itemSlots[current].getSprite();
        if (swapRotators.current != rotator)
            gameObject.SetActive(false);
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
        rotatorImage.sprite = swapRotators.smallRotator;
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
            if (itemSlots.Length == 2)
            {
                if (angleDeg > 0)
                {
                    current = 0;
                } else
                {
                    current = 1;
                }
            } else
            {
                if (angleDeg > -30 && angleDeg < 90)
                {
                    current = 1;
                } 
                else if (angleDeg > 90 && angleDeg < 210)
                {
                    current = 0;
                }
                else
                {
                    current = 2;
                }
            }
            swapRotators.chosen = current;
            switch (rotatorType)
            {
                case RotatorType.Consumable:
                    fullRotator = swapRotators.threeFullRotators[current];
                    break;
                default:
                    fullRotator = swapRotators.twoFullRotators[current];
                    break;
            }
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
        for (int i = 0; i < (rotatorType==RotatorType.Consumable ? 3 : 2); i++)
        {
            Vector2Int chosenItemPos = new Vector2Int(int.MaxValue, int.MaxValue);
            switch (rotatorType)
            {
                case RotatorType.Weapon:
                    chosenItemPos = inv.chosenWeapons[i];
                    break;
                case RotatorType.Consumable:
                    chosenItemPos = inv.chosenConsumables[i];
                    break;
                case RotatorType.Tool:
                    chosenItemPos = inv.chosenTools[i];
                    break;
            }
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
        switch (rotatorType)
        {
            case RotatorType.Consumable:
                fullRotator = swapRotators.threeFullRotators[0];
                break;
            default:
                fullRotator = swapRotators.twoFullRotators[0];
                break;
        }
        if (itemSlots[current].getSprite() != null)
        {
            centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
        else
        {
            centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        if (swapRotators.current == rotator)
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
            switch (rotatorType)
            {
                case RotatorType.Weapon:
                    manager.fighting = true;
                    break;
                case RotatorType.Consumable:
                    //consumable
                    break;
                case RotatorType.Tool:
                    manager.blockplacing = true;
                    manager.placing = false;
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
        manager.blockplacing = false;
    }

    private void OnDestroy()
    {
        controls.Disable();
    }
}
