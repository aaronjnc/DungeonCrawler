using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Dissects the dialog script and outputs information, attached to dialog box
/// </summary>
public class DialogAsset : MonoBehaviour
{
    //reference to stall which is using this script
    public StallManager stall;
    //game object which displays all dialog
    public GameObject dialogBox;
    //manager reference
    GameManager manager;
    //string containing all the text in the text asset
    string fullText;
    //current text options
    string[] currentGroupings;
    //integer value referencing line chosen in current groupings
    int currentLine = 0;
    //current line options
    List<string> currentOptions;
    //strings containing commands that must be performed
    List<string> commandStrings = new List<string>();
    //name of npc you are talking to
    public string npcName;
    /// <summary>
    /// gets the text asset and sets up lines
    /// </summary>
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        fullText = manager.fullText;
        npcName = fullText.Split('\n')[0];
        fullText = string.Join("\n", Regex.Split(fullText, "\n").Skip(1).ToArray());
        getLines(fullText);
    }
    /// <summary>
    /// takes the large string and separates current options
    /// </summary>
    /// <param name="text"> text asset for dialog
    /// </param> 
    void getLines(string text)
    {
        currentOptions = new List<string>();
        currentGroupings = text.Split(currentLine.ToString()[0]);
        for (int i = 1; i < currentGroupings.Length; i++)
        {
            currentOptions.Add(currentGroupings[i].Split('\n')[0]);
            checkForCommand(currentOptions[i-1]);
        }
    }
    /// <summary>
    /// choose line at given position
    /// </summary>
    /// <param name="choice"> line number
    /// </param> 
    public void chooseLine(int choice)
    {
        choice++;
        var newLines = Regex.Split(currentGroupings[choice], "\n").Skip(1);
        string currentGroup = string.Join("\n", newLines.ToArray());
        currentLine++;
        performCommand(currentOptions[choice-1]);
        if (!currentGroup.Contains(currentLine.ToString()))
        {
            currentLine = 0;
            getLines(fullText);
        } 
        else
        {
            getLines(currentGroup);
        }
    }
    /// <summary>
    /// Checks the line to see if it contains a command string
    /// </summary>
    /// <param name="lineChoice"> line to check
    /// </param> 
    void checkForCommand(string lineChoice)
    {
        if (lineChoice.Contains("[") && lineChoice.Contains("]"))
        {
            commandStrings.Add(lineChoice);
        }
    }
    /// <summary>
    /// Performs command on chosen line
    /// </summary>
    /// <param name="chosenLine"> line chosen with command
    /// </param> 
    void performCommand(string chosenLine)
    {
        if (commandStrings.Count == 0)
            return;
        foreach (string command in commandStrings)
        {
            if (command.Contains(chosenLine))
            {
                int index1 = command.IndexOf("[");
                int index2 = command.IndexOf("]");
                string[] commands = command.Substring(index1 + 1, index2 - index1 - 1).Split(',');
                foreach (string c in commands)
                {
                    switch (c)
                    {
                        case "Leave":
                            manager.loadWorld(manager.GetGameInformation());
                            break;
                        case "Buy":
                            stall.AddItems(manager.GetStallItems());
                            dialogBox.SetActive(false);
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }
    }
    /// <summary>
    /// returns the list of line options
    /// </summary>
    /// <returns> string list of options
    /// </returns> 
    public List<string> getLineOptions()
    {
        List<string> lineOptions = new List<string>();
        foreach (string option in currentOptions)
        {
            lineOptions.Add(option);
        }
        return lineOptions;
    }
}
