using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTile
{
    GameManager manager;
   public enum TileType
    {
        Market,
    }
    public TileType type;
    Vendor vendor;
    List<GameObject> enableObjects = new List<GameObject>();
    public void SetUp(byte ID)
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        switch(ID)
        {
            case 7:
                type = TileType.Market;
                vendor = new Vendor();
                enableObjects.Add(GameObject.Find("Market"));
                enableObjects.Add(GameObject.Find("PlayerMarket"));
                break;
        }
    }
    public void Interact()
    {
        switch(type)
        {
            case TileType.Market:
                enableObjects[0].GetComponent<MarketPlace>().SetVendor(vendor);
                manager.PauseGame();
                break;
        }
        foreach(GameObject enableObject in enableObjects)
        {
            enableObject.SetActive(true);
        }
    }
}
