using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : WeaponInterface
{
    [Tooltip("Reach of basic sword")]
    public float reach;
    [Tooltip("Enemy layer")]
    public LayerMask enemy;
    [Tooltip("Sword has hold effect")]
    public bool hasHoldEffect;
    [Tooltip("Swing angle")]
    public float swingAngle;
    [Tooltip("Damage of base attack")]
    public float baseDamage;
    [Tooltip("Damage of advanced attack")]
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
