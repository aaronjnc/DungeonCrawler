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
            float dot = Vector3.Dot(dir, transform.right);
            if (dot >= Mathf.Cos(swingAngle))
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
