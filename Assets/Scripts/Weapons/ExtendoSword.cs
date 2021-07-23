﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendoSword : MonoBehaviour, WeaponInterface<Transform, PlayerFight>
{
    public float reach;
    public float extendedreach;
    public LayerMask enemy;
    public bool hasHoldEffect;
    public float swingAngle;
    public float baseDamage;
    public float advancedDamage;
    public bool HasHoldEffect()
    {
        return hasHoldEffect;
    }
    public void BaseAttack(Transform playerTransform)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerTransform.position, reach, enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dir = enemies[i].transform.position - playerTransform.position;
            float dot = Vector3.Dot(dir, transform.right);
            if (dot >= Mathf.Cos(swingAngle))
            {
                enemies[i].GetComponent<EnemyInfo>().ReduceHealth(baseDamage);
            }
        }
    }
    public void AdvancedAttack(Transform playerTransform)
    {
        RaycastHit2D enemies = Physics2D.Raycast(playerTransform.position-transform.up, -transform.up, extendedreach);
        if (enemies.collider.gameObject.layer == enemy)
        {
            enemies.collider.gameObject.GetComponent<EnemyInfo>().ReduceHealth(advancedDamage);
        }
        Debug.Log(enemies.collider.gameObject.name);
    }
    public void HoldEffect(PlayerFight playerFight)
    {

    }
}
