using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogAsset : MonoBehaviour
{
    GameManager manager;
    string fullText;
    string[] currentGroupings;
    int currentLine = 0;
    List<string> removeSections = new List<string>();
    List<string> currentOptions;
    List<string> commandStrings = new List<string>();
    public string npcName;
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        fullText = manager.fullText;
        npcName = fullText.Split('\n')[0];
        fullText = string.Join("\n", Regex.Split(fullText, "\n").Skip(1).ToArray());
        getLines(fullText);
    }
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
            for (int i = 0; i < removeSections.Count; i++)
            {
                if (removeSections[i].Contains(currentGroupings[choice]))
                {
                    fullText = fullText.Replace(removeSections[i], "");
                }
            }
            removeSections.Clear();
            getLines(fullText);
        } 
        else
        {
            getLines(currentGroup);
        }
    }
    void checkForCommand(string lineChoice)
    {
        if (lineChoice.Contains("[") && lineChoice.Contains("]"))
        {
            commandStrings.Add(lineChoice);
        }
    }
    void performCommand(string chosenLine)
    {
        Debug.Log("performed");
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
                        case "Remove":
                            removeSections.Add(currentLine.ToString() + command);
                            break;
                        case "Leave":
                            string path = Application.persistentDataPath + "/saves/" + manager.worldName + ".txt";
                            SaveSystem.Load(path);
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }
    }
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
