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
    public GameObject[] loadPanels;
    public GameObject[] createPanels;
    public Text[] textBoxes;
    public Text[] hours;
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
        FileInfo[] fileInfo = f.GetFiles();
        for (int i = 0; i < 3; i++)
        {
            if (i >= fileInfo.Length)
            {
                createPanels[i].SetActive(true);
                loadPanels[i].SetActive(false);
                continue;
            }
            createPanels[i].SetActive(false);
            string fileName = fileInfo[i].Name;
            fileName = fileName.Replace(".txt", "");
            textBoxes[i].text = fileName;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/saves/" + fileName + ".txt", FileMode.Open);
            GameInformation info = formatter.Deserialize(stream) as GameInformation;
            hours[i].text = Math.Round(info.playHours, 2) + " hrs";
            loadPanels[i].SetActive(true);
            stream.Close();
        }
    }
    /// <summary>
    /// loads world with given name
    /// </summary>
    /// <param name="worldFile">textbox of world name</param>
    public void LoadWorldFile(Text worldFile)
    {
        string path = Application.persistentDataPath + "/saves/" + worldFile.text + ".txt";
        if (File.Exists(path))
        {
            GameManager manager = GameObject.Find("GameController").GetComponent<GameManager>();
            manager.worldName = worldFile.text;
            SaveSystem.Load(path);
        }
    }
    /// <summary>
    /// Deletes world with given name
    /// </summary>
    /// <param name="worldFile">textbox containing world name</param>
    public void DeleteWorld(Text worldFile)
    {
        string path = Application.persistentDataPath + "/saves/" + worldFile.text + ".txt";
        if (File.Exists(path))
        {
            File.Delete(path);
            AssetDatabase.Refresh();
            Debug.Log("Deleted");
            LoadScreen();
        }
    }
}
