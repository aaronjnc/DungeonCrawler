using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyAttack : MonoBehaviour
{
    public float reach;
    public float viewangle;
    public float damage;
    public LayerMask player;
    public LayerMask tile;
    [HideInInspector]
    public Vector3 lastLocation;
    [HideInInspector]
    public bool spotted;
    PlayerFight fightScript;
    public float attackSpeed;
    protected bool attackCharged = true;
    protected bool charging;
    EnemyMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        fightScript = GameObject.Find("Player").GetComponent<PlayerFight>();
        movement = GetComponent<EnemyMovement>();
    }
    public void attack()
    {
        if (!attackCharged && !charging)
            StartCoroutine(chargeAttack());
        if (attackCharged)
        {
            performAttack();
            attackCharged = false;
        }
    }

    IEnumerator chargeAttack()
    {
        charging = true;
        yield return new WaitForSeconds(attackSpeed);
        attackCharged = true;
        charging = false;
    }

    protected abstract void performAttack();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == (player | (1 << collision.gameObject.layer)))
        {
            Vector2 pos = collision.gameObject.transform.position;
            Vector2 currentpos = transform.position;
            Vector2 dirToTarget = (pos - currentpos).normalized;
            if (Vector3.Angle(transform.up, dirToTarget) < viewangle)
            {
                float distancetoTarget = Vector3.Distance(transform.position, pos);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distancetoTarget, tile))
                {
                    lastLocation = pos;
                    spotted = true;
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player == (player | (1 << collision.gameObject.layer)))
        {
            Vector2 pos = collision.gameObject.transform.position;
            Vector2 currentpos = transform.position;
            Vector2 dirToTarget = (pos - currentpos).normalized;
            if (Vector3.Angle(transform.up, dirToTarget) < viewangle)
            {
                float distancetoTarget = Vector3.Distance(transform.position, pos);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distancetoTarget, tile))
                {
                    lastLocation = pos;
                    spotted = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        spotted = false;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 line = transform.position + (transform.up * reach);
        Vector3 rotatedLine = Quaternion.AngleAxis(viewangle, transform.forward) * line;
        Vector3 rotatedLine2 = Quaternion.AngleAxis(viewangle - 90, transform.forward) * line;
        Gizmos.DrawWireSphere(transform.position, reach);
        Gizmos.DrawLine(transform.position, rotatedLine);
        Gizmos.DrawLine(transform.position, rotatedLine2);
    }
}
