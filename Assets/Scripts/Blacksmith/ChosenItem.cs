using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChosenItem : MonoBehaviour
{
    public Inventory inventory;
    public ItemSlot chosenItem;
    public Image chosenImage;
    public Text itemName;
    public Text ingredient;
    public Text[] ingredients;
    public GameObject buyButton;
    public GameObject craftButton;
    public void ChooseItem(ItemSlot item)
    {
        chosenItem = new ItemSlot();
        chosenItem.addExisting(item);
        chosenImage.sprite = chosenItem.getSprite();
        chosenImage.color = new Color(255, 255, 255, 255);
        itemName.text = "Item: " + chosenItem.GetItemName();
        if (chosenItem.GetItemType() == InventoryItem.ItemType.Mineral)
        {
            ingredient.gameObject.SetActive(false);
        }
        for (int i = 0; i < chosenItem.GetIngredients().Count; i++)
        {
            if (chosenItem.GetItemType() == InventoryItem.ItemType.Mineral || i >= chosenItem.GetIngredients().Count)
            {
                ingredients[i].gameObject.SetActive(false);
            }
            else
            {
                ingredients[i].text = "0/" + chosenItem.GetIngredientCount()[i] + " " + chosenItem.GetIngredients()[i].itemName;
                ingredients[i].gameObject.SetActive(true);
            }
        }
        buyButton.GetComponentInChildren<Text>().text = chosenItem.getCost() + " gold";
        craftButton.GetComponentInChildren<Text>().text = chosenItem.GetCraftCost() + " gold";
    }
}
