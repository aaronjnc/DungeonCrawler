using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAttack : MonoBehaviour
{
    public float reach;
    public float viewangle;
    public float damage;
    public LayerMask player;
    public LayerMask tile;
    Vector3 lastLocation;
    public bool spotted;
    IAstarAI ai;
    PlayerFight fightScript;
    public float attackSpeed;
    float attackCount = 0f;
    bool attackCharged = true;
    // Start is called before the first frame update
    void Start()
    {
        fightScript = GameObject.Find("Player").GetComponent<PlayerFight>();
        ai = GetComponent<IAstarAI>();
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
            if (ai.remainingDistance <= reach && attackCharged)
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
            Vector3 pos = collision.gameObject.transform.position;
            Vector3 dirtoTarget = (pos - transform.position).normalized;
            if (Vector3.Angle(transform.up,dirtoTarget)<viewangle/2)
            {
                float distancetoTarget = Vector3.Distance(transform.position, pos);
                if (!Physics.Raycast(transform.position,dirtoTarget,distancetoTarget,tile))
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
            Vector3 pos = collision.gameObject.transform.position;
            Vector3 dirtoTarget = (pos - transform.position).normalized;
            if (Vector3.Angle(transform.up, dirtoTarget) < viewangle / 2)
            {
                float distancetoTarget = Vector3.Distance(transform.position, pos);
                if (!Physics.Raycast(transform.position, dirtoTarget, distancetoTarget, tile))
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
        ai.destination = lastLocation;
        ai.SearchPath();
    }
}
