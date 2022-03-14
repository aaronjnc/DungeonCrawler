using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class LoadWorld : MonoBehaviour
{
    [Tooltip("Array of load world panel GameObjects")]
    [SerializeField] private GameObject[] loadPanels;
    [Tooltip("Array of create new world panel GameObjects")]
    [SerializeField] private GameObject[] createPanels;
    [Tooltip("World name text boxes")]
    [SerializeField] private Text[] textBoxes;
    [Tooltip("Hours played text boxes")]
    [SerializeField] private Text[] hours;
    // Start is called before the first frame update
    void Start()
    {
        LoadScreen();
    }
    /// <summary>
    /// Sets up load screen with world saves
    /// </summary>
    private void LoadScreen()
    {
        var f = new DirectoryInfo(Application.persistentDataPath + "/saves");
        DirectoryInfo[] dirInfo = f.GetDirectories();
        for (int i = 0; i < 3; i++)
        {
            if (i >= dirInfo.Length)
            {
                createPanels[i].SetActive(true);
                loadPanels[i].SetActive(false);
                continue;
            }
            createPanels[i].SetActive(false);
            string worldName = dirInfo[i].Name;
            textBoxes[i].text = worldName;
            BinaryFormatter formatter = new BinaryFormatter();
            string infoPath = Path.Combine(Application.persistentDataPath, "saves", worldName, "worldInfo.txt");
            if (!File.Exists(infoPath))
            {
                Debug.Log("Deleted " + dirInfo[i].FullName);
                Directory.Delete(dirInfo[i].FullName);
                createPanels[i].SetActive(true);
                loadPanels[i].SetActive(false);
                continue;
            }
            FileStream fs = new FileStream(infoPath, FileMode.Open);
            WorldInfo info = (WorldInfo)formatter.Deserialize(fs);
            hours[i].text = Math.Round(info.GetPlayTime(), 2) + " hrs";
            loadPanels[i].SetActive(true);
            fs.Close();
        }
    }
    /// <summary>
    /// loads world with given name
    /// </summary>
    /// <param name="worldFile">textbox of world name</param>
    public void LoadWorldFile(Text worldFile)
    {
        GameManager manager = GameObject.Find("GameController").GetComponent<GameManager>();
        manager.worldName = worldFile.text;
        SaveSystem.Load();
    }
    /// <summary>
    /// Deletes world with given name
    /// </summary>
    /// <param name="worldFile">textbox containing world name</param>
    public void DeleteWorld(Text worldFile)
    {
        string path = Application.persistentDataPath + "/saves/" + worldFile.text;
        if (Directory.Exists(path))
        {
            DeleteContents(path);
            Directory.Delete(path);
            AssetDatabase.Refresh();
            Debug.Log("Deleted");
            LoadScreen();
        }
    }
    /// <summary>
    /// Helper method to delte contents of directories
    /// </summary>
    /// <param name="directory"></param>
    private void DeleteContents(string directory)
    {
        string[] dirs = Directory.GetDirectories(directory);
        for (int i = 0; i < dirs.Length; i++)
        {
            DeleteContents(dirs[i]);
            Directory.Delete(dirs[i]);
        }
        string[] files = Directory.GetFiles(directory);
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }
    }
}
