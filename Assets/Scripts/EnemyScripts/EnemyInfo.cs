using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    //enemy health
    public float health;
    //max enemy health
    public float maxHealth;
    //slider displaying enemy health
    public Slider slider;
    //fire damage modifier
    public float firedamage;
    //is enemy on fire
    bool onfire = false;
    //how long they are on fire for
    public float firelength;
    //time they've been on fire
    float firetime = 0f;
    //chance of catching on fire
    public float firechance;
    //spawn weight
    public float weight;
    //spawn scale
    public float scale;
    //identifying number of enemy
    public byte id;
    //chunk they spawn in
    [HideInInspector] public Vector2Int chunk;
    //types of enemies
    public enum EnemyType
    {
        Troll,
        Toad,
        Spider,
    }
    /// <summary>
    /// Sets up slider
    /// </summary>
    private void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.minValue = 0;
        slider.value = health;
    }
    /// <summary>
    /// Reduce health of enemy by given amount
    /// </summary>
    /// <param name="amount">Amount to reduce health by</param>
    public void ReduceHealth(float amount)
    {
        health = Mathf.Clamp(health-amount,0,maxHealth);
        slider.value = health;
        if (health <= 0)
        {
            ChunkGen.currentWorld.GetChunk(chunk).KillEnemy(this.gameObject);
        }
    }
    /// <summary>
    /// Determines if enemy is on fire
    /// </summary>
    public void FireDamage()
    {
        if (Random.value < firechance)
            onfire = true;
    }
    /// <summary>
    /// reduces health due to lasting damage
    /// </summary>
    private void FixedUpdate()
    {
        if (onfire)
        {
            ReduceHealth(firedamage * Time.deltaTime);
            firetime += Time.deltaTime;
            if (firetime >= firelength)
            {
                onfire = false;
                firetime = 0f;
            }    
        }
    }
    /// <summary>
    /// Gives string representing enemy type and position
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string enemy = "*" + gameObject.name + "\n";
        enemy += gameObject.transform.position + "\n";
        enemy += gameObject.transform.rotation + "\n";
        return enemy;
    }
}
