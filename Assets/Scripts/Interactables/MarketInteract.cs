using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Vendor))]
public class MarketInteract : InteractableTile
{
    public override void Interact()
    {
        SceneLoader.LoadScene(2);
    }

}
