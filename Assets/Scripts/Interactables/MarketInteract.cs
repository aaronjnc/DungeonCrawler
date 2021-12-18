using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Vendor))]
public class MarketInteract : InteractableTile
{
    public TextAsset textFile;
    public override void Interact()
    {
        manager.assignTextFile(textFile);
        SaveSystem.Save();
        SceneLoader.LoadScene(2);
    }

}
