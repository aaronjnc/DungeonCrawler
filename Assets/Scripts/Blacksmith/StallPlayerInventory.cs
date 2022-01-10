using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StallPlayerInventory : MonoBehaviour
{
    public GameObject inventoryObj;
    private GameObject[,] images = new GameObject[5, 7];
    private ItemSlot[,] inv = new ItemSlot[5, 7];
    private GameManager manager;
    public Image chosenImage;
    private Vector2Int chosenItem;
    private bool setUp = false;
    // Start is called before the first frame update
    private void SetUpInventory()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        int imgnum = 0;
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            int row = imgnum / 7;
            int col = imgnum % 7;
            images[row, col] = image.gameObject;
            imgnum++;
        }
        SetUpInv();
        setUp = true;
        inventoryObj.SetActive(false);
    }
    private void SetUpInv()
    {
        ItemSlot[,] slots = manager.GetInventory();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                inv[i, j] = new ItemSlot();
                inv[i, j] = slots[i, j];
                if (inv[i,j].isEmpty())
                {
                    images[i,j].SetActive(false);
                }    
                else
                {
                    images[i, j].GetComponent<Image>().sprite = inv[i, j].getSprite();
                    images[i, j].SetActive(true);
                }
            }
        }
    }
    public void ChooseItem(int pos)
    {
        int row = pos / 7;
        int col = pos % 7;
        chosenImage.sprite = inv[row, col].getSprite();
        chosenItem = new Vector2Int(row, col);
        chosenImage.gameObject.SetActive(true);
    }
    public List<string[]> GetMinerals()
    {
        if (!setUp)
            SetUpInventory();
        List<string> minerals = new List<string>();
        List<int> mineralCount = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (inv[i, j].isEmpty())
                    continue;
                if (inv[i,j].GetItemType() == InventoryItem.ItemType.Mineral)
                {
                    if (!minerals.Contains(inv[i, j].GetItemName()))
                    {
                        minerals.Add(inv[i,j].GetItemName());
                        mineralCount.Add(inv[i, j].getCurrentCount());
                    }
                    else
                    {
                        mineralCount[minerals.IndexOf(inv[i, j].GetItemName())] += inv[i, j].getCurrentCount();
                    }
                }
            }
        }
        List<string[]> mineral = new List<string[]>();
        for (int i = 0; i < minerals.Count; i++)
        {
            mineral.Add(new string[2]);
            mineral[i][0] = minerals[i];
            mineral[i][1] = mineralCount[i].ToString();
        }
        return mineral;
    }
}
