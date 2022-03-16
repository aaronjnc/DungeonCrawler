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
    [Tooltip("Stall manager")]
    [SerializeField] private StallManager stall;
    [Tooltip("Dialog box object")]
    [SerializeField] private GameObject dialogBox;
    [Tooltip("String containing all the text in the text asset")]
    private string fullText;
    [Tooltip("Current text options")]
    private string[] currentGroupings;
    [Tooltip("Chosen line number")]
    private int currentLine = 0;
    [Tooltip("List of line options")]
    List<string> currentOptions;
    [Tooltip("Lines containing commands")]
    private List<string> commandStrings = new List<string>();
    [Tooltip("Name of npc")]
    [HideInInspector] public string npcName;
    /// <summary>
    /// gets the text asset and sets up lines
    /// </summary>
    void Start()
    {
        fullText = GameManager.Instance.fullText;
        npcName = fullText.Split('\n')[0];
        fullText = string.Join("\n", Regex.Split(fullText, "\n").Skip(1).ToArray());
        GetLines(fullText);
    }
    /// <summary>
    /// takes the large string and separates current options
    /// </summary>
    /// <param name="text"> text asset for dialog
    /// </param> 
    void GetLines(string text)
    {
        currentOptions = new List<string>();
        currentGroupings = text.Split(currentLine.ToString()[0]);
        for (int i = 1; i < currentGroupings.Length; i++)
        {
            currentOptions.Add(currentGroupings[i].Split('\n')[0]);
            CheckForCommand(currentOptions[i-1]);
        }
    }
    /// <summary>
    /// choose line at given position
    /// </summary>
    /// <param name="choice"> line number
    /// </param> 
    public void ChooseLine(int choice)
    {
        choice++;
        var newLines = Regex.Split(currentGroupings[choice], "\n").Skip(1);
        string currentGroup = string.Join("\n", newLines.ToArray());
        currentLine++;
        PerformCommand(currentOptions[choice-1]);
        if (!currentGroup.Contains(currentLine.ToString()))
        {
            currentLine = 0;
            GetLines(fullText);
        } 
        else
        {
            GetLines(currentGroup);
        }
    }
    /// <summary>
    /// Checks the line to see if it contains a command string
    /// </summary>
    /// <param name="lineChoice"> line to check
    /// </param> 
    void CheckForCommand(string lineChoice)
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
    void PerformCommand(string chosenLine)
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
                            GameManager.Instance.reopen = true;
                            GameManager.Instance.LoadFromFile();
                            return;
                        case "Buy":
                            stall.AddItems(GameManager.Instance.GetStallItems());
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
    public List<string> GetLineOptions()
    {
        List<string> lineOptions = new List<string>();
        foreach (string option in currentOptions)
        {
            lineOptions.Add(option);
        }
        return lineOptions;
    }
}
