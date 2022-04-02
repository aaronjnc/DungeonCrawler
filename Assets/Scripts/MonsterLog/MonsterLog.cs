using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLog : MonoBehaviour
{
    Dictionary<string, MonsterLogEntry> monsterLogs = new Dictionary<string, MonsterLogEntry>();
    [SerializeField]
    private GameObject monsterLogEntry;
    [SerializeField]
    private float spacing;
    [SerializeField]
    private float top;
    private void Awake()
    {
        LoadEnemies();
        gameObject.SetActive(false);
    }
    private void LoadEnemies()
    {
        List<GameObject> enemies = GameManager.Instance.GetMonsters();
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject newLog = Instantiate(monsterLogEntry, transform);
            newLog.transform.localPosition = new Vector3(0, top - i * spacing, 0);
            MonsterLogEntry log = newLog.GetComponent<MonsterLogEntry>();
            log.SetValues(enemies[i]);
            monsterLogs.Add(enemies[i].name, log);
        }
    }
    public void Register(string monsterName)
    {
        MonsterLogEntry log = monsterLogs[monsterName];
        if (!log.IsFound())
        {
            log.Show();
        }
    }
}
