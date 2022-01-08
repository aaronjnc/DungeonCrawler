using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithInteract : InteractableTile
{
    public TextAsset text;
    public override void Interact()
    {
        GameObject.Find("GameController").GetComponent<GameManager>().AddStallItems(GetComponent<Stall>().stallItems);
        manager.assignTextFile(text);
        SceneLoader.LoadScene(2);
    }
}
