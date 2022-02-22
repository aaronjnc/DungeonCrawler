using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    /// <summary>
    /// Saves the game
    /// </summary>
    public void SaveGame()
    {
        SaveSystem.Save();
    }
}
