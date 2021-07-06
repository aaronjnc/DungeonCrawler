using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    public float radius;
    Vector2 centerpos;
    EnemyAttack attack;
    public bool activated = false;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        attack = GetComponent<EnemyAttack>();
        centerpos = transform.position;
        Vector2 newPosition = centerpos + Random.insideUnitCircle * radius;
        Vector2 dir = (centerpos - newPosition).normalized;
        float magnitude = (centerpos - newPosition).magnitude;
        agent.Warp(centerpos);
        if (!Physics2D.Raycast(centerpos, dir, magnitude))
        {
            agent.SetDestination(new Vector3(newPosition.x, newPosition.y, -1));
        }
    }
    float waittime = 10f;
    float timewaited = 0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!attack.spotted)
        {
            if (NavMeshSurface2d.activeSurfaces.Count == 0)
                return;
            if (!agent.hasPath || agent.remainingDistance < 1f)
            {
                timewaited += Time.deltaTime;
                if (timewaited > waittime)
                {
                    Debug.Log(transform.position);
                    Vector2 newPosition = centerpos + Random.insideUnitCircle * radius;
                    Vector2 dir = (centerpos - newPosition).normalized;
                    float magnitude = (centerpos - newPosition).magnitude;
                    if (!Physics2D.Raycast(centerpos, dir, magnitude))
                    {
                        agent.SetDestination(new Vector3(newPosition.x, newPosition.y, -1));
                    }
                    timewaited = 0f;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(centerpos, radius);
    }
}
