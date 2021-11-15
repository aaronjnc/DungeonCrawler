using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class Inventory : MonoBehaviour
{
    public float xdistance;
    public float xstart;
    public float invy;
    public float ydistance;
    GameManager manager;
    public GameObject[,] images = new GameObject[5, 7];
    //public ItemReference[,,] invItems = new ItemReference[5,5,7];
    private ItemSlot[,,] itemSlots = new ItemSlot[5, 5, 7];
    public MonoBehaviour[,] spells = new MonoBehaviour[5, 7];
    public Transform mainInv;
    //public ItemReference[,] chosenItems = new ItemReference[5,5];
    public MonoBehaviour[] chosenSpells = new MonoBehaviour[5];
    public Image[] chosenImages = new Image[5];
    //public ItemReference emptyitem;
    public Vector2Int[,] chosenPos = new Vector2Int[5,5];
    public SwapRotators swapRotators;
    public Magic magic;
    public GameObject invImage;
    // Start is called before the first frame update
    public enum InventoryType
    {
        Weapons,
        Blocks,
        Tools,
        Food,
        Armor,
        Spells,
    }
    public InventoryType invType = InventoryType.Weapons;
    public int currentInv = 0;
    void Awake()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        manager.LoadWorld();
        manager.inv = this;
        for(int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 7;col++)
            {
                images[row, col] = Instantiate(invImage,mainInv);
                images[row, col].name = "InvImage";
                images[row, col].GetComponent<ImageMover>().SetArrayPos(new Vector2Int(row, col));
                images[row, col].GetComponent<Image>().color = new Color(255,255,255,0);
                RectTransform imgtransform = images[row, col].GetComponent<RectTransform>();
                imgtransform.localPosition = new Vector3(xstart + xdistance * col, invy - ydistance * row, -1);
                images[row, col].SetActive(true);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int row = 0; row < 5; row++)
            {
                chosenPos[i, row] = new Vector2Int(10, 10);
                for (int col = 0; col < 7; col++)
                {
                    itemSlots[i, row, col] = new ItemSlot();
                }
            }
        }
        if (manager.loadFromFile)
        {
            loadFromFile(manager.GetGameInformation());
        } else
        {
            InventoryItem item = manager.GetItem("BasePickaxe");
            AddItem(item,1,item.baseDurability);
            item = manager.GetItem("ExtendoSword");
            AddItem(item,1, item.baseDurability);
        }
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Adds item of ID to inventory
    /// </summary>
    /// <param name="ID">Item ID to add to inventory</param>
    public void AddItem(byte ID)
    {
        InventoryItem newItem = manager.GetItem(ID);
        AddItem(newItem, 1, newItem.baseDurability);
    }
    /// <summary>
    /// Adds item based on item script
    /// </summary>
    /// <param name="itemRef">Script of item to add to inventory</param>
    public void AddItem(InventoryItem itemRef, int currentStack, int durability)
    {
        InventoryType type = itemRef.invType;
        bool found = false;
        Vector2Int spot = Vector2Int.zero;
        Vector2Int empty = Vector2Int.zero;
        int invNum;
        bool emptySpot = false;
        switch(type)
        {
            default:
            case InventoryType.Weapons:
                invNum = 0;
                break;
            case InventoryType.Blocks:
                invNum = 1;
                break;
            case InventoryType.Tools:
                invNum = 2;
                break;
            case InventoryType.Food:
                invNum = 3;
                break;
            case InventoryType.Armor:
                invNum = 4;
                break;
        }
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                if (itemRef.itemID == itemSlots[invNum, row, col].getItemId() && !itemSlots[invNum, row, col].isFull())
                {
                    spot = new Vector2Int(row, col);
                    found = true;
                    break;
                }
                if (itemSlots[invNum, row,col].isEmpty() && !emptySpot)
                {
                    empty = new Vector2Int(row, col);
                    emptySpot = true;
                }
            }
        }
        if (found)
        {
            int leftOver = itemSlots[invNum, spot.x, spot.y].addToStack((byte)currentStack);
            if (leftOver > 0)
                AddItem(itemRef, leftOver, durability);
        }
        else if (emptySpot)
        {
            itemSlots[invNum, empty.x, empty.y].addItem(itemRef, (byte)currentStack, (byte)durability);
            if (invType == type)
            {
                UpdateImage(new Vector2Int(empty.x, empty.y), itemRef.itemSprite);
            }
        }
    }
    /// <summary>
    /// Updates image at given position
    /// </summary>
    /// <param name="newPos">Image position</param>
    /// <param name="itemSprite">New sprite</param>
    void UpdateImage(Vector2Int newPos, Sprite itemSprite)
    {
        images[newPos.x, newPos.y].GetComponent<Image>().sprite = itemSprite;
        if (itemSprite != null)
            images[newPos.x, newPos.y].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        else
            images[newPos.x, newPos.y].GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }
    /// <summary>
    /// Update chosen item image
    /// </summary>
    /// <param name="pos">Chosen item position</param>
    /// <param name="itemSprite">New item sprite</param>
    void UpdateChosen(int pos, Sprite itemSprite)
    {
        chosenImages[pos].GetComponent<Image>().sprite = itemSprite;
        if (itemSprite != null)
            chosenImages[pos].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        else
            chosenImages[pos].GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }
    /// <summary>
    /// Switch between inventory types
    /// </summary>
    /// <param name="type">New inventory page type</param>
    public void ChangeInventory(InventoryType type)
    {
        switch (type)
        {
            default:
            case InventoryType.Weapons:
                currentInv = 0;
                break;
            case InventoryType.Blocks:
                currentInv = 1;
                break;
            case InventoryType.Tools:
                currentInv = 2;
                break;
            case InventoryType.Food:
                currentInv = 3;
                break;
            case InventoryType.Armor:
                currentInv = 4;
                break;
        }
        invType = type;
        for(int row = 0; row < 5; row++)
        {
            Vector2Int currentInvPos = chosenPos[currentInv, row];
            if (currentInvPos != new Vector2Int(10, 10))
            {
                UpdateChosen(row, itemSlots[currentInv, currentInvPos.x, currentInvPos.y].getSprite());
            } else
            {
                UpdateChosen(row, null);
            }
            for (int col = 0; col < 7; col++)
            {
                UpdateImage(new Vector2Int(row, col), itemSlots[currentInv, row, col].getSprite());
            }
        }
    }
    /// <summary>
    /// Removes item at position
    /// </summary>
    /// <param name="pos">Item position</param>
    public void RemoveItem(Vector2Int pos)
    {
        Vector2Int chosenItemPos = chosenPos[pos.x, pos.y];
        itemSlots[currentInv, chosenItemPos.x, chosenItemPos.y].emptySlot();
        chosenPos[pos.x, pos.y] = new Vector2Int(10, 10);
        if (pos.x == currentInv)
        {
            UpdateImage(chosenPos[pos.x, pos.y], null);
            UpdateChosen(pos.y, null);
        }
        swapRotators.UpdateRotator(pos.x);
    }
    /// <summary>
    /// Reduce durability of item
    /// </summary>
    /// <param name="pos">Chosen item position</param>
    public void reduceDurability(Vector2Int pos)
    {
        Vector2Int chosenItemPos = chosenPos[pos.x, pos.y];
        itemSlots[currentInv, chosenItemPos.x, chosenItemPos.y].reduceDurability(1);
        bool empty = itemSlots[currentInv, chosenItemPos.x, chosenItemPos.y].isEmpty();
    }
    /// <summary>
    /// Reduce stack count
    /// </summary>
    /// <param name="pos">Chosen item position</param>
    public void reduceStack(Vector2Int pos)
    {
        Vector2Int chosenItemPos = chosenPos[pos.x, pos.y];
        itemSlots[currentInv, chosenItemPos.x, chosenItemPos.y].reduceStack(1);
        bool empty = itemSlots[currentInv, chosenItemPos.x, chosenItemPos.y].isEmpty();
        if (empty)
        {
            swapRotators.UpdateRotator(swapRotators.current);
        }
    }
    /// <summary>
    /// Removes chosen item
    /// </summary>
    /// <param name="tab">Current inventory page</param>
    /// <param name="i">Chosen position</param>
    public void RemoveChosenItem(int tab, int i)
    {
        int invrow = (i / 7);
        int invcol = (i % 7);
        itemSlots[currentInv, invrow, invcol].emptySlot();
        if (tab == currentInv)
            UpdateImage(new Vector2Int(invrow, invcol), null);
        for (int spot = 0; spot < 5; spot++)
        {
            if (chosenPos[tab,i].Equals(new Vector2Int(invrow,invcol)))
            {
                chosenPos[tab, i] = new Vector2Int(10, 10);
                if (tab == currentInv)
                    UpdateChosen(spot, null);
                swapRotators.UpdateRotator(tab);
            }
        }
    }
    /// <summary>
    /// Moves item to new location in inventory
    /// </summary>
    /// <param name="arrayPos">Original position of item</param>
    /// <param name="dropPos">New position of item</param>
    public void DropItem(Vector2Int arrayPos, Vector3 dropPos)
    {
        int row = Mathf.RoundToInt((invy -dropPos.y)/ydistance);
        int col = Mathf.RoundToInt((dropPos.x-xstart) / xdistance);
        if (row < 5 && col < 7)
        {
            if (isEmpty(new Vector2Int(row, col)))
            {
                for (int i = 0; i < 5;i++)
                {
                    if (chosenPos[currentInv, i].Equals(arrayPos))
                    {
                        chosenPos[currentInv, i] = new Vector2Int(row, col);
                        break;
                    }
                }
                ItemSlot item = itemSlots[currentInv, arrayPos.x, arrayPos.y];
                itemSlots[currentInv, row, col].addItem(item.getItem(), item.getCurrentCount(), item.getDurability());
                itemSlots[currentInv, arrayPos.x, arrayPos.y].emptySlot();
                images[arrayPos.x, arrayPos.y].GetComponent<Image>().sprite = null;
                UpdateImage(arrayPos, null);
                UpdateImage(new Vector2Int(row, col), itemSlots[currentInv, row, col].getSprite());
            }
        }
        else if (row >= 5)
        {
            dropPos.z = 0;
            int chosen = 6;
            for (int i = 0; i < 5; i++)
            {
                Bounds newBounds = new Bounds();
                newBounds.center = -chosenImages[i].gameObject.transform.InverseTransformPoint(transform.position);
                Rect rectangle = chosenImages[i].gameObject.GetComponent<RectTransform>().rect;
                Vector2 center = new Vector2(newBounds.center.x, newBounds.center.y);
                newBounds.max = center + rectangle.max;
                newBounds.min = center + rectangle.min;
                if (newBounds.Contains(dropPos))
                {
                    chosen = i;
                    break;
                }
            }
            if (chosen != 6)
            {
                for (int i = 0; i<5;i++)
                {
                    if (i != chosen && chosenPos[currentInv,i].Equals(arrayPos))
                    {
                        chosenPos[currentInv, i] = new Vector2Int(10,10);
                        UpdateChosen(i, null);
                        break;
                    }
                }
                UpdateChosen(chosen, itemSlots[currentInv, arrayPos.x, arrayPos.y].getSprite());
                chosenPos[currentInv, chosen] = arrayPos;
                swapRotators.UpdateRotator(currentInv);
            }
        }       
    }
    /// <summary>
    /// Determines if inventory slot is empty
    /// </summary>
    /// <param name="arrayPos">Inventory position</param>
    /// <returns></returns>
    public bool isEmpty(Vector2Int arrayPos)
    {
        if (images[arrayPos.x, arrayPos.y].GetComponent<Image>().sprite == null)
            return true;
        else
            return false;
    }

    private void loadFromFile(GameInformation info)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 7; k++)
                {
                    byte infoItem = info.inventory[i, j, k];
                    if (infoItem != 127)
                    {
                        InventoryItem item = manager.GetItem(infoItem);
                        AddItem(item, info.stackSize[i,j,k], info.durability[i,j,k]);
                    }
                }
                if (info.chosenPos[i, j, 0] != int.MaxValue)
                {
                    chosenPos[i, j] = new Vector2Int(info.chosenPos[i, j, 0], info.chosenPos[i, j, 1]);
                }
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (chosenPos[0,i] != new Vector2Int(10,10))
            {
                Vector2Int chosenItemPos = chosenPos[0, i];
                if (chosenItemPos != new Vector2Int(10,10))
                    UpdateChosen(i, itemSlots[currentInv, chosenItemPos.x, chosenItemPos.y].getSprite());
            }
        }
        swapRotators.UpdateRotator(info.rotator);
    }
    public ItemSlot getItemSlot(int invNum, int row, int col)
    {
        return itemSlots[invNum, row, col];
    }
}
