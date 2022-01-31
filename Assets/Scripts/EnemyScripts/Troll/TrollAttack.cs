using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollAttack : EnemyAttack
{
    /// <summary>
    /// club the player if they are within range
    /// </summary>
    protected override void performAttack()
    {
        Collider2D playerColl = Physics2D.OverlapCircle(transform.position, reach, player);
        if (playerColl != null)
        {
            Vector2 dirToTarget = (playerColl.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.up, dirToTarget) < viewangle)
            {
                float distancetoTarget = Vector3.Distance(transform.position, playerColl.transform.position);
                if (Physics2D.Raycast(transform.position, dirToTarget, distancetoTarget, player))
                {
                    playerColl.gameObject.GetComponent<PlayerFight>().TakeDamage(damage);
                }
            }
        }
    }
}
