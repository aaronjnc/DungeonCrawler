using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class ItemRotator : MonoBehaviour
{
    Sprite fullRotator;
    //public ItemReference[] items = new ItemReference[5];
    public ItemSlot[] itemSlots = new ItemSlot[5];
    public List<Image> images = new List<Image>();
    //public ItemReference chosenItem;
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
        swapRotators = GetComponentInParent<SwapRotators>();
        foreach (Image img in images)
        {
            img.gameObject.SetActive(false);
        }
        for (int i = 0; i < 5; i++)
        {
            itemSlots[i] = new ItemSlot();
        }
        controls.Inventory.ItemRotator.performed += ExpandRotator;
        controls.Inventory.ItemRotator.canceled += MinimizeRotator;
        controls.Inventory.ItemRotator.Enable();
        controls.Movement.MousePosition.Enable();
        fullRotator = swapRotators.fullRotators[current];
        centralImage.sprite = itemSlots[current].getSprite();
        if (swapRotators.current != rotator)
            gameObject.SetActive(false);
        started = true;
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
                centralImage.sprite = itemSlots[current].getSprite();
                fullRotator = swapRotators.fullRotators[current];
                rotatorImage.sprite = fullRotator;
                if (itemSlots[current].getSprite() != null)
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
        if (!started)
            StartMethod();
        for (int i = 0; i < 5; i++)
        {
            Vector2Int chosenItemPos = inv.chosenPos[rotator, i];
            if (chosenItemPos == new Vector2Int(10, 10))
                continue;
            itemSlots[i].addExisting(inv.getItemSlot(rotator, chosenItemPos.x, chosenItemPos.y));
            images[i].sprite = itemSlots[i].getSprite();
            if (images[i].sprite != null)
                images[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            else
                images[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        centralImage.sprite = itemSlots[current].getSprite();
        fullRotator = swapRotators.fullRotators[0];
        if (itemSlots[current].getSprite() != null)
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
        manager.currentItem.addExisting(itemSlots[current]);
        if (!itemSlots[current].isEmpty())
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
                    manager.currentTileID = itemSlots[current].getItemId();
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
