﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpiderAttack : EnemyAttack
{
    [Tooltip("Damage done by stinger attack")]
    [SerializeField] private float stingerDamage;
    [Tooltip("Reach of web attack")]
    [SerializeField] private float webReach;
    [Tooltip("How long webbing lasts")]
    [SerializeField] private float webTime;
    [Tooltip("How long poison lasts")]
    [SerializeField] private float poisonTime;
    /// <summary>
    /// Determines type of attack to perform based on player location
    /// </summary>
    protected override void PerformAttack()
    {
        Collider2D playerColl = Physics2D.OverlapCircle(transform.position, webReach, player);
        if (playerColl != null)
        {
            float distToTarget = Mathf.Abs(Vector2.Distance(transform.position, playerColl.transform.position));
            if (distToTarget < reach)
            {
                Vector2 dirToTarget = (playerColl.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.up, dirToTarget) < viewangle)
                {
                    if (Physics2D.Raycast(transform.position, dirToTarget, distToTarget, player))
                    {
                        Debug.Log("Bit");
                        playerColl.gameObject.GetComponent<PlayerFight>().TakeDamage(damage);
                    }
                } else if (Vector3.Angle(transform.up, dirToTarget) < 180 - viewangle)
                {
                    if (Physics2D.Raycast(transform.position, dirToTarget, distToTarget, player))
                    {
                        Debug.Log("Stabbed");
                        playerColl.gameObject.GetComponent<PlayerFight>().TakeDamage(stingerDamage);
                    }
                }
            } 
            else
            {
                Vector2 dirToTarget = (playerColl.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.up, dirToTarget) < viewangle)
                {
                    if (Physics2D.Raycast(transform.position, dirToTarget, distToTarget, player))
                    {
                        Debug.Log("Webbed");
                        FreePlayerMove player = playerColl.gameObject.GetComponent<FreePlayerMove>();
                        player.canMove = false;
                        StartCoroutine(webbed(player));
                    }
                }
            }
        }
    }
    IEnumerator webbed(FreePlayerMove playerMove)
    {
        yield return new WaitForSeconds(webTime);
        playerMove.canMove = true;
    }
}
