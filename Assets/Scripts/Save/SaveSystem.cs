using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static void Save()
    {
        GameObject manager = GameObject.Find("GameController");
        GameInformation info = new GameInformation(manager);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saves/player.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, info);
        stream.Close();
        Debug.Log("Saved");
    }
    public static void Load(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameInformation info = formatter.Deserialize(stream) as GameInformation;
            GameManager manager = GameObject.Find("GameController").GetComponent<GameManager>();
            manager.loadWorld(info);
            stream.Close();
        }
    }
}
