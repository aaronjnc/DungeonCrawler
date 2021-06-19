using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwitchMarketTab : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public Image marketTab;
    public Sprite[] tabSprites = new Sprite[5];
    int currentNum = 0;
    public bool right;
    public bool tabChange;
    public MarketPlace marketScript;
    int max;
    public float[] sizex = new float[5];
    // Start is called before the first frame update
    void Start()
    {
        if (tabChange)
            max = 4;
        else
            max = 2;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        marketScript.RefreshImage(marketScript.chosenImage, null);
        if (tabChange)
            currentNum = marketScript.currentTab;
        else
            currentNum = marketScript.currentPage;
        if (right)
            currentNum++;
        else
            currentNum--;
        if (currentNum < 0)
            currentNum = max;
        else if (currentNum > max)
            currentNum = 0;
        if (tabChange)
        {
            marketScript.currentTab = currentNum;
            marketTab.sprite = tabSprites[currentNum];
            RectTransform rect = marketTab.GetComponent<RectTransform>();
            marketTab.GetComponent<RectTransform>().sizeDelta = new Vector2(sizex[currentNum] * rect.sizeDelta.y, rect.sizeDelta.y);
        }
        else
            marketScript.currentPage = currentNum;
        marketScript.ChangeItems();
    }
    
}
