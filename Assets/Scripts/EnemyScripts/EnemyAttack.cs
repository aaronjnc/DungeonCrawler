using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAttack : MonoBehaviour
{
    public float reach;
    public float viewangle;
    public float damage;
    public LayerMask player;
    public LayerMask tile;
    Vector3 lastLocation;
    public bool spotted;
    PlayerFight fightScript;
    public float attackSpeed;
    float attackCount = 0f;
    bool attackCharged = true;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fightScript = GameObject.Find("Player").GetComponent<PlayerFight>();
    }
    private void FixedUpdate()
    {
        if (!attackCharged)
            attackCount += Time.deltaTime;
        if (attackCount >= attackSpeed)
        {
            attackCount = 0f;
            attackCharged = true;
        }
        if (spotted)
        {
            if (agent.remainingDistance <= reach && attackCharged)
            {
                fightScript.TakeDamage(damage);
                attackCharged = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == (player | (1 << collision.gameObject.layer)))
        {
            Vector2 pos = collision.gameObject.transform.position;
            Vector2 currentpos = transform.position;
            Vector2 dirToTarget = (pos - currentpos).normalized;
            float angle = Vector2.SignedAngle(transform.up, dirToTarget);
            if (angle < viewangle / 2 && angle > -viewangle / 2)
            {
                float distancetoTarget = Vector3.Distance(transform.position, pos);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distancetoTarget, tile))
                {
                    lastLocation = pos;
                    spotted = true;
                    SetLocation();
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
            float angle = Vector2.SignedAngle(transform.up, dirToTarget);
            if (angle < viewangle / 2 && angle > -viewangle / 2)
            {
                float distancetoTarget = Vector3.Distance(transform.position, pos);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distancetoTarget, tile))
                {
                    lastLocation = pos;
                    spotted = true;
                    SetLocation();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        spotted = false;
    }
    void SetLocation()
    {
        agent.SetDestination(lastLocation);
    }
}
