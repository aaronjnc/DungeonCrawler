using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Slider slider;
    public float firedamage;
    bool onfire = false;
    public float firelength;
    float firetime = 0f;
    public float firechance;
    public float weight;
    public float scale;
    public enum EnemyType
    {
        Troll,
        Toad,
    }
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
            Destroy(gameObject);
    }
    /// <summary>
    /// Determines if enemy is on fire
    /// </summary>
    public void FireDamage()
    {
        if (Random.value < firechance)
            onfire = true;
    }
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
}
