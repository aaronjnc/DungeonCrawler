using System;
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
        List<MonsterInfo> monsterInfo = GameManager.Instance.GetMonsters();
        found = new bool[logs.Count][];
        for (int i = 0; i < logs.Count; i++)
            found[i] = new bool[Enum.GetNames(typeof(MonsterInfo.MonsterType)).Length];
        foreach (MonsterInfo m in monsterInfo)
        {
            found[m.id % 10][m.id / 5] = logs[m.id % 10].IsFound(m.id / 5);
        }
    }
    public bool[][] GetFound()
    {
        return found;
    }
}
