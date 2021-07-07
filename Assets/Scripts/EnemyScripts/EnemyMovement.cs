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
    public float rotateSpeed;
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
    float lookTime = 5f;
    float lookwait = 0f;
    float lookAngle;
    Vector2 nextPoint;
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
        if (agent.hasPath && !attack.spotted)
        {
            Rotate();
        }
        else if (!attack.spotted)
        {
            lookwait += Time.deltaTime;
            if (lookwait > lookTime)
            {
                lookAngle = Random.Range(0, 360);
                lookwait = 0f;
            }
            StartCoroutine("StandRotate");
        }
    }
    /// <summary>
    /// Determine whether or not to rotate enemy
    /// </summary>
    public void Rotate()
    {
        if (nextPoint != (Vector2)agent.path.corners[1])
        {
            StartCoroutine("Rotator");
            nextPoint = agent.path.corners[1];
        }
    }
    /// <summary>
    /// Rotates enemy to look relative to direction
    /// </summary>
    /// <returns></returns>
    IEnumerator Rotator()
    {
        Vector2 target = agent.path.corners[1] - transform.position;
        float angle = Vector2.SignedAngle(transform.up, target);
        float targetAngle = transform.localEulerAngles.z + angle;
        if (targetAngle >= 360)
            targetAngle -= 360;
        else if (targetAngle < 0)
            targetAngle += 360;
        transform.up = Vector3.Lerp(transform.up, target, rotateSpeed);
        yield return null;
        /*while (transform.localEulerAngles.z < targetAngle - .1f || transform.localEulerAngles.z > targetAngle + .1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotateSpeed * Time.deltaTime);
            yield return null;
        }*/
    }
    /// <summary>
    /// Rotate while waiting
    /// </summary>
    /// <returns></returns>
    IEnumerator StandRotate()
    {
        while (transform.localEulerAngles.z < lookAngle - .1f || transform.localEulerAngles.z > lookAngle + .1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, lookAngle), rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerpos, radius);
    }
}
