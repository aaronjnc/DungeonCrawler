using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithInteract : InteractableTile
{
    public override void Interact()
    {
        GameObject.Find("GameController").GetComponent<GameManager>().AddStallItems(GetComponent<Stall>().stallItems);
        SceneLoader.LoadScene(2);
    }
}
