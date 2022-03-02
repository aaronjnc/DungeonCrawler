using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : WeaponInterface
{
    public float reach;
    public LayerMask enemy;
    public bool hasHoldEffect;
    public float swingAngle;
    public float baseDamage;
    public float advancedDamage;
    public override bool HasHoldEffect()
    {
        return hasHoldEffect;
    }
    public override void BaseAttack(Transform playerTransform)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerTransform.position, reach, enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dir = enemies[i].transform.position - playerTransform.position;
            dir.Normalize();
            Vector3 max = playerTransform.right + new Vector3(Mathf.Cos(swingAngle), Mathf.Sin(swingAngle), 0);
            Vector3 min = playerTransform.right - new Vector3(Mathf.Cos(swingAngle), Mathf.Sin(swingAngle), 0);
            if (dir.x < max.x && dir.x > min.x && dir.y < max.y && dir.y > min.y)
            {
                enemies[i].GetComponent<EnemyInfo>().ReduceHealth(baseDamage);
            }
        }
    }
    public override void AdvancedAttack(Transform playerTransform)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerTransform.position, reach, enemy);
        for (int i = 0; i < enemies.Length;i++)
        {
            enemies[i].GetComponent<EnemyInfo>().ReduceHealth(advancedDamage);
        }
    }
    public override void HoldEffect(PlayerFight playerFight)
    {}
}
