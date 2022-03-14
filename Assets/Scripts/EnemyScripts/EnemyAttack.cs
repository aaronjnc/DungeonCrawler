using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// parent class for enemy movement
/// </summary>
public abstract class EnemyAttack : MonoBehaviour
{
    //reach of basic attack
    public float reach;
    //enemy view angle
    public float viewangle;
    //damage performed by basic attack
    public float damage;
    //layer of player
    public LayerMask player;
    //layer of tile
    public LayerMask tile;
    //last location player was seen
    [HideInInspector] public Vector3 lastLocation;
    //true if enemy can see player
    [HideInInspector] public bool spotted;
    //reference to the player's fight script
    PlayerFight fightScript;
    //speed of basic attack
    public float attackSpeed;
    //if attack is charged and ready to be performed
    protected bool attackCharged = true;
    //if attack is charging
    protected bool charging;
    //movement script reference
    EnemyMovement movement;
    /// <summary>
    /// sets up enemy components
    /// </summary>
    void Start()
    {
        fightScript = GameObject.Find("Player").GetComponent<PlayerFight>();
        movement = GetComponent<EnemyMovement>();
    }
    /// <summary>
    /// perform attack if charged
    /// </summary>
    public void Attack()
    {
        if (!attackCharged && !charging)
            StartCoroutine(ChargeAttack());
        if (attackCharged)
        {
            PerformAttack();
            attackCharged = false;
        }
    }
    /// <summary>
    /// charge enemy attack
    /// </summary>
    /// <returns></returns>
    IEnumerator ChargeAttack()
    {
        charging = true;
        yield return new WaitForSeconds(attackSpeed);
        attackCharged = true;
        charging = false;
    }
    /// <summary>
    /// abstract method called within each special enemy class to perform attack
    /// </summary>
    protected abstract void PerformAttack();
    /// <summary>
    /// determines if player is visible once within range
    /// </summary>
    /// <param name="collision">player collider</param>
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
    /// <summary>
    /// determines if player is visible when staying within range
    /// </summary>
    /// <param name="collision">player collider</param>
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
    /// <summary>
    /// when player leaves view range
    /// </summary>
    /// <param name="collision">player collider</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        spotted = false;
    }
    /// <summary>
    /// Draws enemy reach
    /// </summary>
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
