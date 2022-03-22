using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateWorld : MonoBehaviour
{
    [Tooltip("Input for world name")]
    [SerializeField] private InputField inputbox;
    [Tooltip("Game object for invalid world name")]
    [SerializeField] private GameObject failureText;
    [Tooltip("Array of text objects for loaded worlds")]
    [SerializeField] private Text[] loadedWorlds;
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
        GameManager.Instance.worldName = worldName;
        GameManager.Instance.loadFromFile = false;
        SceneManager.LoadScene(1);
    }
}
