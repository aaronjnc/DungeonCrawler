using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    /// <summary>
    /// Sets up script given tile ID
    /// </summary>
    /// <param name="ID">ID for interactable tile</param>
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
    /// <summary>
    /// Interacts with tile given type of interactable tile
    /// </summary>
    public void Interact()
    {
        switch(type)
        {
            case TileType.Market:
                //enableObjects[0].GetComponent<MarketPlace>().SetVendor(vendor);
                //manager.PauseGame();
                manager.assignTextFile("Market");
                SceneManager.LoadScene(2);
                break;
        }
        foreach(GameObject enableObject in enableObjects)
        {
            //enableObject.SetActive(true);
        }
    }
}
