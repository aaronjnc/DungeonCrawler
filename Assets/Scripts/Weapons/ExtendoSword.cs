using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendoSword : WeaponInterface
{
    public float reach;
    public float extendedreach;
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
        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position + playerTransform.right, playerTransform.right, extendedreach, enemy);
        if (hit.collider.gameObject.CompareTag("Enemy"))
        {
            hit.collider.gameObject.GetComponent<EnemyInfo>().ReduceHealth(advancedDamage);
        }
    }
    public override void HoldEffect(PlayerFight playerFight)
    {

    }
}
