using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class DialogManager : MonoBehaviour
{
    PlayerControls controls;
    DialogAsset asset;
    Text textbox;
    public Text namebox;
    bool player = false;
    List<string> lines = new List<string>();
    int selectedLine = 0;
    private void Start()
    {
        controls = new PlayerControls();
        controls.Dialog.ChangeOption.performed += changeLineChoice;
        controls.Dialog.ChangeOption.Enable();
        controls.Dialog.ChooseLine.performed += updateLines;
        controls.Dialog.ChooseLine.Enable();
        textbox = GetComponent<Text>();
        asset = GetComponent<DialogAsset>();
        updateLines();
    }
    void updateLines()
    {
        lines = asset.getLineOptions();
        if (lines[0][1] == 'P')
        {
            namebox.text = "Player";
            player = true;
        }
        else
        {
            namebox.text = asset.npcName;
        }
        trimLines();
        setUpTextbox();
    }
    void setUpTextbox()
    {
        textbox.text = "";
        for (int i = 0; i < lines.Count; i++)
        {
            if (i == selectedLine)
            {
                if (player)
                {
                    string newline = "<b><color=red>" + lines[i] + "</color></b>";
                    textbox.text += newline;
                }
                else
                {
                    textbox.text += lines[i];
                }
            }
            else
            {
                textbox.text += lines[i];
            }
            if (i != lines.Count-1)
            {
                textbox.text += "\n";
            }
        }
    }
    private void changeLineChoice(CallbackContext ctx)
    {
        selectedLine += (int)ctx.ReadValue<float>();
        if (selectedLine < 0)
        {
            selectedLine = lines.Count - 1;
        } 
        else if (selectedLine >= lines.Count)
        {
            selectedLine = 0;
        }
        setUpTextbox();
    }
    void updateLines(CallbackContext ctx)
    {
        asset.chooseLine(selectedLine);
        selectedLine = 0;
        player = false;
        updateLines();
    }
    void trimLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains("["))
            {
                lines[i] = lines[i].Substring(3, lines[i].IndexOf("[")-3).Trim();
            }
            else
            {
                lines[i] = lines[i].Substring(3, lines[i].Length-3).Trim();
            }
        }
    }
    private void OnDestroy()
    {
        controls.Disable();
    }
}
