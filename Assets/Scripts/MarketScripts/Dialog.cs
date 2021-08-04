using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    TextAsset dialog;
    string[] lines;
    // Start is called before the first frame update
    void Start()
    {
        lines = dialog.text.Split('\n');
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
