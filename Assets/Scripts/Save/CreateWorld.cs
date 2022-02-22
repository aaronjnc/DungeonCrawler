﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateWorld : MonoBehaviour
{
    public InputField inputbox;
    public GameObject failureText;
    public Text[] loadedWorlds;
    /// <summary>
    /// Creates new world with input information
    /// </summary>
    public void CreateNewWorld()
    {
        failureText.SetActive(false);
        string worldName = inputbox.text.Trim();
        if (worldName == null || worldName == "")
        {
            failureText.SetActive(true);
            return;
        }
        char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            if (worldName.Contains(c+""))
            {
                failureText.SetActive(true);
                return;
            }
        }
        foreach (Text t in loadedWorlds)
        {
            if (t.gameObject.activeInHierarchy)
            {
                if (t.text.Trim().Equals(worldName))
                {
                    failureText.SetActive(true);
                    return;
                }
            }
        }
        GameManager manager = GameObject.Find("GameController").GetComponent<GameManager>();
        manager.worldName = worldName;
        manager.loadFromFile = false;
        GameInformation gameInfo = new GameInformation(manager.gameObject);
        manager.SetGameInfo(gameInfo);
        SaveSystem.Save();
        SceneManager.LoadScene(1);
    }
}
