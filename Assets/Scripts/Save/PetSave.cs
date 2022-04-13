using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PetSave
{
    [Tooltip("Monsters have been found")]
    private bool[][] found;
    
    public PetSave(MonsterLog log)
    {
        List<MonsterLogEntry> logs = log.GetLogs();
        found = new bool[logs.Count][];
        for (int i = 0; i < logs.Count; i++)
        {
            MonsterLogEntry logEntry = logs[i];
            //found
        }
    }
}
