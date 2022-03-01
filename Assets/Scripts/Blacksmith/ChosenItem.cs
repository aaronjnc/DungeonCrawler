using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChosenItem : MonoBehaviour
{
    //reference to chosen item
    public ItemSlot chosenItem;
    //image for chosen item
    public Image chosenImage;
    //textbox for chosen item name
    public Text itemName;
    //textbox above list of ingredients (de-activate if no ingredients)
    public Text ingredient;
    //textboxes to hold ingredients
    public Text[] ingredients;
    //button used to buy item
    public GameObject buyButton;
    //button used to sell item
    public GameObject craftButton;
    public StallPlayerInventory inv;
    private List<string> minerals = new List<string>();
    private List<int> mineralCount = new List<int>();
    public void Awake()
    {
        GetMinerals();
        chosenImage.gameObject.SetActive(false);
        itemName.gameObject.SetActive(false);
        ingredient.gameObject.SetActive(false);
        foreach (Text ing in ingredients)
        {
            ing.gameObject.SetActive(false);
        }
        buyButton.gameObject.SetActive(false);
        craftButton.gameObject.SetActive(false);
    }
    /// <summary>
    /// Choose given item and add its information to top
    /// </summary>
    /// <param name="item"></param> item reference
    public void ChooseItem(ItemSlot item)
    {
        chosenItem = new ItemSlot();
        chosenItem.addExisting(item);
        chosenImage.sprite = chosenItem.getSprite();
        chosenImage.color = new Color(255, 255, 255, 255);
        chosenImage.gameObject.SetActive(true);
        itemName.text = "Item: " + chosenItem.GetItemName();
        itemName.gameObject.SetActive(true);
        ingredient.gameObject.SetActive(chosenItem.GetIngredients().Count > 0);
        bool craftable = true;
        for (int i = 0; i < chosenItem.GetIngredients().Count; i++)
        {
            if (chosenItem.GetItemType() == InventoryItem.ItemType.Mineral || i >= chosenItem.GetIngredients().Count)
            {
                ingredients[i].gameObject.SetActive(false);
            }
            else
            {
                string ingredientName = chosenItem.GetIngredients()[i].itemName;
                if (minerals.Contains(ingredientName))
                {
                    int invCount = mineralCount[minerals.IndexOf(ingredientName)];
                    ingredients[i].text = invCount + "/" + chosenItem.GetIngredientCount()[i] + " " + chosenItem.GetIngredients()[i].itemName;
                    if (invCount < chosenItem.GetIngredientCount()[i])
                    {
                        craftable = false;
                    }
                }
                else
                {
                    ingredients[i].text = "0/" + chosenItem.GetIngredientCount()[i] + " " + chosenItem.GetIngredients()[i].itemName;
                }
                ingredients[i].gameObject.SetActive(true);
            }
        }
        buyButton.GetComponentInChildren<Text>().text = chosenItem.getCost() + " gold";
        craftButton.GetComponentInChildren<Text>().text = chosenItem.GetCraftCost() + " gold";
        if (inv.GetMoney() > chosenItem.getCost())
        {
            buyButton.GetComponent<Button>().interactable= true;
        }
        else
        {
            buyButton.GetComponent<Button>().interactable = false;
        }
        if (inv.GetMoney() > chosenItem.GetCraftCost() && craftable)
        {
            craftButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            craftButton.GetComponent<Button>().interactable = false;
        }
        buyButton.SetActive(true);
        craftButton.SetActive(true);
    }
    private void GetMinerals()
    {
        List<string[]> invMinerals = inv.GetMinerals();
        for (int i = 0; i < invMinerals.Count; i++)
        {
            minerals.Add(invMinerals[i][0]);
            mineralCount.Add(Int32.Parse(invMinerals[i][1]));
        }
    }

    public void BuyItem()
    {
        inv.SpendMoney(chosenItem.getCost());
        inv.AddItem(chosenItem);
    }

    public void CraftItem()
    {
        inv.SpendMoney(chosenItem.GetCraftCost());
        inv.AddItem(chosenItem);
    }
}
