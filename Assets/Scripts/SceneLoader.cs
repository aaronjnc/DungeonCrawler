using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Load new scene
    /// </summary>
    /// <param name="level">Scene name</param>
    public static void LoadScene(int level)
    {
        AssetDatabase.Refresh();
        SceneManager.LoadScene(level);
    }
    /// <summary>
    /// Exit game
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
