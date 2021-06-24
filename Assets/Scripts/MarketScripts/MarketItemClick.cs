using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MarketItemClick : MonoBehaviour, IPointerDownHandler
{
    MarketPlace marketScript;
    Vector2Int pos = Vector2Int.zero;
    public void SetUp(MarketPlace marketPlace, Vector2Int arrayPos)
    {
        marketScript = marketPlace;
        pos = arrayPos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        marketScript.ChooseItem(pos);
    }
}
