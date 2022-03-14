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
        string path = Path.Combine(Application.persistentDataPath, "saves", GameManager.Instance.worldName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        GameInformation.Instance.SetLocation(path);
        GameInformation.Instance.SaveAll();
    }
    /// <summary>
    /// loads world with given path
    /// </summary>
    public static void Load()
    {
        GameManager.Instance.loadFromFile = true;
        string path = Path.Combine(Application.persistentDataPath, "saves", GameManager.Instance.worldName);
        GameInformation.Instance.SetLocation(path);
        GameManager.Instance.LoadFromFile();
    }
}
