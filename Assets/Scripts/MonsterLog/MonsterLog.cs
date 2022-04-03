using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterLog : MonoBehaviour
{
    [SerializeField]
    private List<MonsterLogEntry> logs = new List<MonsterLogEntry>();
    [SerializeField]
    private Image leftTitle;
    [SerializeField]
    private Image rightTitle;
    [SerializeField]
    private List<Sprite> leftTitleSprites = new List<Sprite>();
    [SerializeField]
    private List<Sprite> rightTitleSprites = new List<Sprite>();
    private int currentPage = 0;
    [SerializeField]
    private int pages = 2;
    [SerializeField]
    private GameObject leftClick;
    [SerializeField]
    private GameObject rightClick;
    private void Start()
    {
        LoadMonsters();
        ShowPage();
        gameObject.SetActive(false);
    }
    private void LoadMonsters()
    {
        List<MonsterInfo> monsters = GameManager.Instance.GetMonsters();
        foreach (MonsterInfo monster in monsters)
        {
            logs[monster.id % 10].AddMonster(monster);
        }
    }
    public void Register(MonsterInfo monster)
    {
        logs[monster.id % 10].Find(monster.id, currentPage);
    }
    public void ChangePage(int i)
    {
        currentPage += i;
        ShowPage();
    }
    private void ShowPage()
    {
        leftTitle.sprite = leftTitleSprites[currentPage];
        if (currentPage < rightTitleSprites.Count)
        {
            rightTitle.enabled = true;
            rightTitle.sprite = rightTitleSprites[currentPage];
        }
        else
        {
            rightTitle.enabled = false;
        }
        if (currentPage == 0)
        {
            leftClick.SetActive(false);
        }
        else if (!leftClick.activeInHierarchy)
        {
            leftClick.SetActive(true);
        }
        if (currentPage == pages - 1)
        {
            rightClick.SetActive(false);
        }
        else if (!rightClick.activeInHierarchy)
        {
            rightClick.SetActive(true);
        }
        foreach (MonsterLogEntry log in logs)
        {
            log.Show(currentPage);
        }
    }
}
