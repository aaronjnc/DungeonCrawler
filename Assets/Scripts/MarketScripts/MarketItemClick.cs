using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MarketItemClick : MonoBehaviour, IPointerDownHandler
{
    MarketPlace marketScript;
    Vector2Int pos = Vector2Int.zero;
    /// <summary>
    /// Sets up item click script
    /// </summary>
    /// <param name="marketPlace">Market Place script</param>
    /// <param name="arrayPos">Item position</param>
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
