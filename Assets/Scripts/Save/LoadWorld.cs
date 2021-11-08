using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadWorld : MonoBehaviour
{
    public GameObject[] loadButtons;
    public Text[] textBoxes;
    // Start is called before the first frame update
    void Start()
    {
        var f = new DirectoryInfo(Application.persistentDataPath + "/saves");
        FileInfo[] fileInfo = f.GetFiles();
        for (int i = 0; i < 3; i++)
        {
            if (i >= fileInfo.Length)
            {
                loadButtons[i].SetActive(false);
                continue;
            } 
            string fileName = fileInfo[i].Name;
            fileName = fileName.Replace(".txt", "");
            textBoxes[i].text = fileName;
            loadButtons[i].SetActive(true);
        }
    }

    public void LoadWorldFile(Text worldFile)
    {
        string path = Application.persistentDataPath + "/saves/" + worldFile.text + ".txt";
        if (File.Exists(path))
        {
            SaveSystem.Load(path);
        }
    }
}
