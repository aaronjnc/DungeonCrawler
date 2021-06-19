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
    public ItemReference[,,] invItems = new ItemReference[5,5,7];
    public MonoBehaviour[,] spells = new MonoBehaviour[5, 7];
    public Transform mainInv;
    public ItemReference[,] chosenItems = new ItemReference[5,5];
    public MonoBehaviour[] chosenSpells = new MonoBehaviour[5];
    public Image[] chosenImages = new Image[5];
    public ItemReference emptyitem;
    Vector2Int[,] chosenPos = new Vector2Int[5,5];
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
                chosenItems[i,row] = new ItemReference();
                for (int col = 0; col < 7; col++)
                {
                    invItems[i, row, col] = new ItemReference();
                }
            }
        }
        ItemReference item = new ItemReference();
        item.SetValues(manager.GetItem("BasePickaxe"));
        AddItem(item);
        item.SetValues(manager.GetItem("Sword"));
        AddItem(item);
        gameObject.SetActive(false);
    }
    public void AddItem(string itemName)
    {
        ItemReference item = new ItemReference();
        item.SetValues(manager.GetItem(itemName));
        AddItem(item);
    }
    public void AddItem(ItemReference itemRef)
    {
        InventoryType type = itemRef.invType;
        ItemReference item = null;
        bool found = false;
        Vector2Int spot = Vector2Int.zero;
        Vector2 empty = Vector2.zero;
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
                item = invItems[invNum, row, col];
                if (!item.empty && item.currentStack != item.stackSize)
                {
                    spot = new Vector2Int(row, col);
                    found = true;
                    break;
                }
                if (item.empty && !emptySpot)
                {
                    empty = new Vector2(row, col);
                    emptySpot = true;
                }
            }
        }
        if (found)
        {
            invItems[invNum, spot.x, spot.y].currentStack++;
        }
        else if (emptySpot)
        {
            invItems[invNum, (int)empty.x, (int)empty.y].ChangeValues(itemRef);
            invItems[invNum, (int)empty.x, (int)empty.y].empty = false;
            if (invType == type)
            {
                UpdateItem(new Vector2Int((int)empty.x, (int)empty.y), itemRef.itemSprite);
            }
        }
    }
    public void MoveItem(Vector2Int newPos, Vector2Int oldPos)
    {
        ItemReference item = invItems[currentInv,oldPos.x, oldPos.y];
        item.empty = true;
        invItems[currentInv, newPos.x, newPos.y].ChangeValues(item);
        UpdateItem(newPos, item.itemSprite);
    }
    void UpdateItem(Vector2Int newPos, Sprite itemSprite)
    {
        images[newPos.x, newPos.y].GetComponent<Image>().sprite = itemSprite;
        if (itemSprite != null)
            images[newPos.x, newPos.y].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        else
            images[newPos.x, newPos.y].GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }
    void UpdateChosen(int pos, Sprite itemSprite)
    {
        chosenImages[pos].GetComponent<Image>().sprite = itemSprite;
        if (itemSprite != null)
            chosenImages[pos].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        else
            chosenImages[pos].GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }
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
            UpdateChosen(row, chosenItems[currentInv, row].itemSprite);
            for (int col = 0; col < 7; col++)
            {
                UpdateItem(new Vector2Int(row, col), invItems[currentInv, row, col].itemSprite);
            }
        }
    }
    public int currentStack(Vector2Int pos)
    {
        return chosenItems[pos.x, pos.y].currentStack;
    }
    public int currentDurability(Vector2Int pos)
    {
        return chosenItems[pos.x, pos.y].durability;
    }
    public void removeItem(Vector2Int pos)
    {
        invItems[currentInv, chosenPos[pos.x, pos.y].x, chosenPos[pos.x, pos.y].y] = new ItemReference();
        chosenItems[pos.x, pos.y] = new ItemReference();
        if (pos.x == currentInv)
        {
            UpdateItem(chosenPos[pos.x, pos.y], null);
            UpdateChosen(pos.y, null);
        }
        swapRotators.UpdateRotator(pos.x);
    }
    public void reduceDurability(Vector2Int pos)
    {
        invItems[currentInv, chosenPos[pos.x, pos.y].x, chosenPos[pos.x, pos.y].y].durability--;
        if (invItems[currentInv, chosenPos[pos.x, pos.y].x, chosenPos[pos.x, pos.y].y].durability == 0)
            removeItem(pos);
    }
    public void reduceStack(Vector2Int pos)
    {
        invItems[currentInv, chosenPos[pos.x, pos.y].x, chosenPos[pos.x, pos.y].y].currentStack--;
        if (invItems[currentInv, chosenPos[pos.x, pos.y].x, chosenPos[pos.x, pos.y].y].currentStack == 0)
            removeItem(pos);
    }
    public void RemoveItem(int tab, int i)
    {
        int invrow = (i / 7);
        int invcol = (i % 7);
        invItems[tab, invrow, invcol].ChangeValues(new ItemReference());
        if (tab == currentInv)
            UpdateItem(new Vector2Int(invrow, invcol), invItems[tab, invrow, invcol].itemSprite);
        for (int spot = 0; spot < 5; spot++)
        {
            if (chosenPos[tab,i].Equals(new Vector2Int(invrow,invcol)))
            {
                chosenPos[tab, i] = new Vector2Int(10, 10);
                chosenItems[tab, i] = new ItemReference();
                if (tab == currentInv)
                    UpdateChosen(spot, null);
                swapRotators.UpdateRotator(tab);
            }
        }
    }
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
                invItems[currentInv, row, col].empty = false;
                invItems[currentInv, arrayPos.x, arrayPos.y] = new ItemReference();
                images[arrayPos.x, arrayPos.y].GetComponent<Image>().sprite = null;
                UpdateItem(arrayPos, null);
                invItems[currentInv, row, col].ChangeValues(invItems[currentInv, arrayPos.x, arrayPos.y]);
                UpdateItem(new Vector2Int(row, col), invItems[currentInv, row, col].itemSprite);
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
                        chosenItems[currentInv, i] = new ItemReference();
                        UpdateChosen(i, null);
                        break;
                    }
                }
                chosenItems[currentInv, chosen].ChangeValues(invItems[currentInv, arrayPos.x, arrayPos.y]);
                UpdateChosen(chosen, chosenItems[currentInv, chosen].itemSprite);
                chosenPos[currentInv, chosen] = arrayPos;
                chosenItems[currentInv, chosen].empty = false;
                swapRotators.UpdateRotator(currentInv);
            }
        }       
    }
    public bool isEmpty(Vector2Int arrayPos)
    {
        if (images[arrayPos.x, arrayPos.y].GetComponent<Image>().sprite == null)
            return true;
        else
            return false;
    }
}
