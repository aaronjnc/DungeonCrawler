using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterLogEntry : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monsterNameObj;
    [SerializeField]
    private TextMeshProUGUI monsterDescriptionObj;
    [SerializeField]
    private Image monsterImage;
    private Sprite monsterSprite;
    private string monsterName;
    private string monsterDescription;
    private Sprite monsterHiddenSprite;
    private const string hiddenName = "????";
    private bool found = false;
    public void SetValues(GameObject monster)
    {
        monsterName = monster.name;
        MonsterInfo info = monster.GetComponent<MonsterInfo>();
        monsterDescription = info.GetDescription();
        monsterHiddenSprite = info.GetHiddenImage();
        monsterSprite = info.GetLogImage();
        Hide();
    }
    public void Hide()
    {
        monsterNameObj.text = hiddenName;
        monsterDescriptionObj.text = "";
        monsterImage.sprite = monsterHiddenSprite;
    }
    public void Show()
    {
        monsterNameObj.text = monsterName;
        monsterDescriptionObj.text = monsterDescription;
        monsterImage.sprite = monsterSprite;
    }
    public bool IsFound()
    {
        return found;
    }
    public void Find()
    {
        found = true;
    }
}
