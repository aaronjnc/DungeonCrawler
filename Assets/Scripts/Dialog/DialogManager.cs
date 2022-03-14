using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class DialogManager : MonoBehaviour
{
    //controls reference
    private PlayerControls controls;
    //reference to dialog asset script associated with this manager
    private DialogAsset asset;
    //textbox to display text
    private Text textbox;
    //name box to display name
    public Text namebox;
    // boolean to determine if player is speaking
    private bool player = false;
    //lines to display
    private List<string> lines = new List<string>();
    //selected line
    private int selectedLine = 0;
    //whether start method has been called
    private bool started = false;
    /// <summary>
    /// sets ups controls and updates lines with retrieved asset
    /// </summary>
    private void Start()
    {
        controls = new PlayerControls();
        controls.Dialog.ChangeOption.performed += ChangeLineChoice;
        controls.Dialog.ChangeOption.Enable();
        controls.Dialog.ChooseLine.canceled += UpdateLines;
        controls.Dialog.ChooseLine.Enable();
        textbox = GetComponent<Text>();
        asset = GetComponent<DialogAsset>();
        UpdateLines();
        started = true;
    }
    /// <summary>
    /// Sets up the textbox when enabled
    /// </summary>
    private void OnEnable()
    {
        if (started)
        {
            selectedLine = 0;
            player = false;
            UpdateLines();
        }
    }
    /// <summary>
    /// updates textbox text with lines list
    /// </summary>
    void UpdateLines()
    {
        lines = asset.GetLineOptions();
        if (lines[0][1] == 'P')
        {
            namebox.text = "Player";
            player = true;
        }
        else
        {
            namebox.text = asset.npcName;
        }
        TrimLines();
        SetupTextbox();
    }
    /// <summary>
    /// sets up textbox with given lines, and highlights chosen line
    /// </summary>
    void SetupTextbox()
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
    /// <summary>
    /// updates line choice and highlights new line when key is pressed
    /// </summary>
    /// <param name="ctx"></param>
    private void ChangeLineChoice(CallbackContext ctx)
    {
        selectedLine -= (int)ctx.ReadValue<float>();
        if (selectedLine < 0)
        {
            selectedLine = lines.Count - 1;
        } 
        else if (selectedLine >= lines.Count)
        {
            selectedLine = 0;
        }
        SetupTextbox();
    }
    /// <summary>
    /// updates lines when line is chosen
    /// </summary>
    /// <param name="ctx"></param>
    void UpdateLines(CallbackContext ctx)
    {
        asset.ChooseLine(selectedLine);
    }
    /// <summary>
    /// removes brackets at end of strings
    /// </summary>
    void TrimLines()
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
    /// <summary>
    /// disable controls
    /// </summary>
    private void OnDestroy()
    {
        controls.Disable();
    }
}
