using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// parent class for enemy movement
/// </summary>
public abstract class EnemyAttack : MonoBehaviour
{
    [Tooltip("Base attack reach")]
    public float reach;
    [Tooltip("Enemy view angle")]
    public float viewangle;
    [Tooltip("Base attack damage")]
    [SerializeField] protected float damage;
    [Tooltip("Player layer")]
    [SerializeField] protected LayerMask player;
    [Tooltip("Tile layer")]
    [SerializeField] protected LayerMask tile;
    [Tooltip("Player last location")]
    [HideInInspector] public Vector3 lastLocation;
    [Tooltip("Player spotted")]
    [HideInInspector] public bool spotted;
    [Tooltip("Player fight script")]
    protected PlayerFight fightScript;
    [Tooltip("Base attack speed")]
    public float attackSpeed;
    [Tooltip("Base attack is charged")]
    protected bool attackCharged = true;
    [Tooltip("Attack is charging")]
    protected bool charging;
    [Tooltip("Enemy movement script")]
    protected EnemyMovement movement;
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
