using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithInteract : InteractableTile
{
    //text asset containing lines for blacksmith
    public TextAsset text;
    /// <summary>
    /// method called when object is interacted with, opening new scene
    /// </summary>
    public override void Interact()
    {
        GameObject.Find("GameController").GetComponent<GameManager>().AddStallItems(GetComponent<Stall>().stallItems);
        manager.assignTextFile(text);
        SceneLoader.LoadScene(2);
    }
}
