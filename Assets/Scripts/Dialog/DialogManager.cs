using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    PlayerControls controls;
    DialogAsset asset;
    Text textbox;
    List<string[]> lines = new List<string[]>();
    private void Start()
    {
        controls = new PlayerControls();
        textbox = GetComponent<Text>();
    }
    void SetAsset(DialogAsset assetref)
    {
        asset = assetref;
        foreach(string line in asset.lines)
        {
            lines.Add(line.Split('|'));
        }
    }
}
