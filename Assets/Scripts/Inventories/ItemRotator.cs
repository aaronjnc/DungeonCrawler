using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class ItemRotator : MonoBehaviour
{
    Sprite fullRotator;
    public ItemReference[] items = new ItemReference[5];
    public List<Image> images = new List<Image>();
    public ItemReference chosenItem;
    public Image centralImage;
    PlayerControls controls;
    bool open = false;
    public FreePlayerMove playerMovement;
    public int current = 0;
    int previous = 0;
    Image rotatorImage;
    SwapRotators swapRotators;
    public Inventory inv;
    GameManager manager;
    [HideInInspector]
    public int rotator = 0;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        rotatorImage = GetComponent<Image>();
        controls = new PlayerControls();
        swapRotators = GetComponentInParent<SwapRotators>();
        foreach(Image img in images)
        {
            img.gameObject.SetActive(false);
        }
        for(int i = 0; i < 5; i++)
        {
            items[i] = new ItemReference();
        }
        controls.Inventory.ItemRotator.performed += ExpandRotator;
        controls.Inventory.ItemRotator.canceled += MinimizeRotator;
        controls.Inventory.ItemRotator.Enable();
        controls.Movement.MousePosition.Enable();
        fullRotator = swapRotators.fullRotators[current];
        chosenItem = items[current];
        centralImage.sprite = chosenItem.itemSprite;
        if (swapRotators.current != rotator)
            gameObject.SetActive(false);
    }
    /// <summary>
    /// Expands item rotator when 'Tab' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void ExpandRotator(CallbackContext ctx)
    {
        rotatorImage.sprite = fullRotator;
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
        open = false;
    }
    float timechange = 0f;
    void FixedUpdate()
    {
        if (open)
        {
            timechange += Time.deltaTime;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            float angleRad = Mathf.Atan2(mousePos.y-transform.position.y, mousePos.x-transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            if (angleDeg >= 23 && angleDeg < 90)
            {
                current = 1;
            }
            else if (angleDeg >= 90 && angleDeg < 160)
            {
                current = 0;
            }
            else if (angleDeg < 23 && angleDeg > -45)
            {
                current = 3;
            }
            else if (angleDeg <= -45 && angleDeg > -136)
            {
                current = 4;
            }
            else
            {
                current = 2;
            }
            if (current != previous)
            {
                swapRotators.chosen = current;
                chosenItem.ChangeValues(items[current]);
                chosenItem.empty = items[current].empty;
                centralImage.sprite = chosenItem.itemSprite;
                fullRotator = swapRotators.fullRotators[current];
                rotatorImage.sprite = fullRotator;
                if (chosenItem.itemSprite != null)
                    centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                else
                    centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                CurrentItem();
            }
            previous = current;
        }
    }
    /// <summary>
    /// Updates items within item rotator given inventory
    /// </summary>
    public void UpdateItems()
    {
        for (int i = 0; i < 5; i++)
        {
            items[i].ChangeValues(inv.chosenItems[rotator,i]);
            images[i].sprite = items[i].itemSprite;
            if (items[i].itemSprite != null)
                images[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            else
                images[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        if (items[0].empty)
            chosenItem = new ItemReference();
        else
            chosenItem.ChangeValues(items[0]);
        centralImage.sprite = chosenItem.itemSprite;
        fullRotator = swapRotators.fullRotators[0];
        if (chosenItem.itemSprite != null)
            centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        else
            centralImage.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        if (swapRotators.current == rotator)
            CurrentItem();
    }
    /// <summary>
    /// Sets certain values given currently selected item
    /// </summary>
    void CurrentItem()
    {
        manager.currentItem.ChangeValues(chosenItem);
        if (!chosenItem.empty)
        {
            DisableBlockPlacing();
            manager.fighting = false;
            switch (rotator)
            {
                case 0:
                    manager.fighting = true;
                    break;
                case 1:
                    manager.blockplacing = true;
                    manager.placing = true;
                    manager.currentTileID = chosenItem.itemID;
                    break;
                case 2:
                    manager.blockplacing = true;
                    manager.placing = false;
                    break;
                case 3:
                    //food
                    break;
                default:
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
    /// Resets everything when block placing is disabled
    /// </summary>
    public void DisableBlockPlacing()
    {
        manager.ResetPreviousTile();
        manager.blockplacing = false;
    }
}
