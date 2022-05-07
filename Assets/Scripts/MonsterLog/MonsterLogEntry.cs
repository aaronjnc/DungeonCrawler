using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterLogEntry : MonoBehaviour
{
    private Image logEntry;
    [SerializeField]
    private List<Sprite> foundImages = new List<Sprite>();
    [SerializeField]
    private List<Sprite> hiddenImages = new List<Sprite>();
    private List<bool> MonsterFound = new List<bool>();
    [SerializeField]
    private bool right;
    private void Awake()
    {
        logEntry = GetComponent<Image>();
    }
    public void AddMonster(MonsterInfo monster)
    {
        MonsterFound.Add(false);
    }
    public void Show(int page)
    {
        if (page >= foundImages.Count || page >= hiddenImages.Count)
        {
            logEntry.enabled = false;
            return;
        }
        if (!logEntry.isActiveAndEnabled)
            logEntry.enabled = true;
        if (IsFound(page))
        {
            logEntry.sprite = foundImages[page];
        }
        else
        {
            logEntry.sprite = hiddenImages[page];
        }
    }
    public bool IsFound(int page)
    {
        if (page >= MonsterFound.Count)
        {
            return false;
        }
        return MonsterFound[page];
    }
    public void Find(int id, int page)
    {
        int val = id / 5;
        if (right)
        {
            val -= 1;
        }
        MonsterFound[val] = true;
        if (page == val)
        {
            logEntry.sprite = foundImages[page];
        }
    }

    public int GetCount()
    {
        return MonsterFound.Count;
    }
}
