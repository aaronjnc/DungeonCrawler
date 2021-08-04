using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAsset
{
    public List<string> lines = new List<string>();
    int dialogNum;
    public DialogAsset(int i)
    {
        dialogNum = i;
        foreach(string line in Resources.LoadAll<TextAsset>("DialogScripts")[i].text.Split('\n'))
        {
            lines.Add(line);
        }
    }
}
