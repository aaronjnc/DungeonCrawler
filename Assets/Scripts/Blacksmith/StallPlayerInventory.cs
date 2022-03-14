using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StallPlayerInventory : MonoBehaviour
{
    [Tooltip("Inventory object")]
    [SerializeField] private GameObject inventoryObj;
    [Tooltip("Array of inventory images")]
    private GameObject[,] images = new GameObject[5, 7];
    [Tooltip("Array of inventory slots")]
    private ItemSlot[,] inv = new ItemSlot[5, 7];
    [Tooltip("Chosen image")]
    [SerializeField] private Image chosenImage;
    [Tooltip("Player money")]
    private int money;
    [Tooltip("Inventory position of chosen item")]
    private Vector2Int chosenItem;
    [Tooltip("Inventory is setup")]
    private bool setUp = false;
    /// <summary>
    /// sets up the inventory
    /// </summary>
    private void SetUpInventory()
    {
        money = GameManager.Instance.GetMoney();
        int imgnum = 0;
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            int row = imgnum / 7;
            int col = imgnum % 7;
            images[row, col] = image.gameObject;
            imgnum++;
        }
        SetUpItemSlots();
        setUp = true;
        inventoryObj.SetActive(false);
    }
    /// <summary>
    /// sets up inventory slots
    /// </summary>
    private void SetUpItemSlots()
    {
        ItemSlot[,] slots = GameManager.Instance.GetInventory();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                inv[i, j] = new ItemSlot();
                inv[i, j] = slots[i, j];
                if (inv[i,j].IsEmpty())
                {
                    images[i,j].SetActive(false);
                }    
                else
                {
                    images[i, j].GetComponent<Image>().sprite = inv[i, j].GetSprite();
                    images[i, j].SetActive(true);
                }
            }
        }
    }
    /// <summary>
    /// Choose clicked item
    /// </summary>
    /// <param name="pos"></param>
    public void ChooseItem(int pos)
    {
        int row = pos / 7;
        int col = pos % 7;
        chosenImage.sprite = inv[row, col].GetSprite();
        chosenItem = new Vector2Int(row, col);
        chosenImage.gameObject.SetActive(true);
    }
    /// <summary>
    /// Returns Dictionary containing minerals and their counts
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, int> GetMinerals()
    {
        if (!setUp)
            SetUpInventory();
        Dictionary<string, int> minerals = new Dictionary<string, int>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (inv[i, j].IsEmpty())
                    continue;
                if (inv[i,j].GetItemType() == InventoryItem.ItemType.Mineral)
                {
                    if (!minerals.ContainsKey(inv[i, j].GetItemName()))
                    {
                        minerals.Add(inv[i,j].GetItemName(), inv[i, j].GetCount());
                    }
                    else
                    {
                        minerals[inv[i, j].GetItemName()] += inv[i, j].GetCount();
                    }
                }
            }
        }
        return minerals;
    }
    /// <summary>
    /// Spends given amount of money
    /// </summary>
    /// <param name="num"></param>
    public void SpendMoney(int num)
    {
        money = Mathf.Clamp(money - num, 0, int.MaxValue);
    }
    /// <summary>
    /// Returns money count
    /// </summary>
    /// <returns></returns>
    public int GetMoney()
    {
        return money;
    }
    /// <summary>
    /// Adds item to inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemSlot item)
    {
        bool found = false;
        Vector2Int spot = Vector2Int.zero;
        Vector2Int empty = Vector2Int.zero;
        bool emptySpot = false;
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                if (item.GetItemID() == inv[row, col].GetItemID() && !inv[row, col].IsFull())
                {
                    spot = new Vector2Int(row, col);
                    found = true;
                    int leftOver = inv[spot.x, spot.y].AddToStack((byte)item.GetCount());
                    if (leftOver > 0)
                    {
                        ItemSlot newItem = new ItemSlot();
                        newItem.AddExisting(item);
                        newItem.ReduceStack((byte)(item.GetCount() - leftOver));
                        GameManager.Instance.StoreInventory(inv);
                        AddItem(newItem);
                    }
                    break;
                }
                if (inv[row, col].IsEmpty() && !emptySpot)
                {
                    empty = new Vector2Int(row, col);
                    emptySpot = true;
                }
            }
            if (found)
                break;
        }
        if (emptySpot && !found)
        {
            inv[empty.x, empty.y].AddExisting(item);
            UpdateImage(new Vector2Int(empty.x, empty.y), item.GetSprite());
            GameManager.Instance.StoreInventory(inv);
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
        {
            images[newPos.x, newPos.y].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            images[newPos.x, newPos.y].SetActive(true);
        }
        else
        {
            images[newPos.x, newPos.y].GetComponent<Image>().color = new Color(255, 255, 255, 0);
            images[newPos.x, newPos.y].SetActive(false);
        }
    }
}
