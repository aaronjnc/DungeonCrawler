using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    [Tooltip("Enemy health")]
    private float health;
    [Tooltip("Max enemy health")]
    [SerializeField] private float maxHealth;
    [Tooltip("Enemy health slider")]
    [SerializeField] private Slider slider;
    [Tooltip("Modifier for effect of fire damage")]
    [SerializeField] private float firedamage;
    [Tooltip("Is on fire")]
    private bool onfire = false;
    [Tooltip("Fire length")]
    [SerializeField] private float firelength;
    [Tooltip("Time on fire")]
    private float firetime = 0f;
    [Tooltip("Chance of catching on fire")]
    [SerializeField] private float firechance;
    [Tooltip("Spawn weight")]
    public float weight;
    [Tooltip("Spawn scale")]
    public float scale;
    [Tooltip("Enemy ID")]
    public byte id;
    [Tooltip("Spawn chunk")]
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
        Debug.Log("Hit");
        health = Mathf.Clamp(health-amount,0,maxHealth);
        slider.value = health;
        if (health <= 0)
        {
            ChunkGen.Instance.GetChunk(chunk).DestroyEnemy(this.gameObject);
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
    /// <summary>
    /// Returns enemy health
    /// </summary>
    /// <returns></returns>
    public float GetHealth()
    {
        return health;
    }
}
