using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    /// <summary>
    /// Saves the game using world name kept in game manager
    /// </summary>
    public static void Save()
    {
        GameManager manager = GameObject.Find("GameController").GetComponent<GameManager>();
        string path = Path.Combine(Application.persistentDataPath, "saves", manager.worldName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        GameInformation.Instance.SetLocation(path);
        GameInformation.Instance.SaveAll(manager);
    }
    /// <summary>
    /// loads world with given path
    /// </summary>
    /// <param name="path">path of world to load</param>
    public static void Load()
    {
        GameManager manager = GameObject.Find("GameController").GetComponent<GameManager>();
        manager.loadFromFile = true;
        string path = Path.Combine(Application.persistentDataPath, "saves", manager.worldName);
        GameInformation.Instance.SetLocation(path);
        manager.loadWorld();
    }
}
