using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfo : MonoBehaviour
{
    [Tooltip("Spawn weight")]
    public float weight;
    [Tooltip("Spawn scale")]
    public float scale;
    [Tooltip("Monster ID")]
    public byte id;
    [SerializeField]
    private string monsterName;
    [SerializeField]
    private Sprite logImage;
    [SerializeField]
    private Sprite hiddenImage;
    [SerializeField]
    private string description;
    [Tooltip("Spawn chunk")]
    [HideInInspector] public Vector2Int chunk;
    //types of enemies
    public enum MonsterType
    {
        Troll,
        Toad,
        Spider,
    }
    /// <summary>
    /// Gives string representing monster type and position
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(new string[] { "*", gameObject.name, "\n" });
        sb.Append(new string[] { gameObject.transform.position.ToString(), "\n" });
        sb.Append(new string[] { gameObject.transform.rotation.ToString(), "\n" });
        return sb.ToString();
    }
    public Sprite GetHiddenImage()
    {
        return hiddenImage;
    }
    public string GetDescription()
    {
        return description;
    }
    public Sprite GetLogImage()
    {
        return logImage;
    }
    public string GetName()
    {
        return monsterName;
    }
}
