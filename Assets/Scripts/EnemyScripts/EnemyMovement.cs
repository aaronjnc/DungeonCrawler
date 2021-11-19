using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public float maxMoveDistance;
    public float minMoveDistance;
    protected Vector2 centerPos;
    protected EnemyAttack attack;
    protected Vector2 nextPoint;
    protected Quaternion endRot;
    protected bool attacking;
    protected float zPos;
    protected bool moving;
    protected bool waiting;
    public float waitTime;
    protected void SetNextPoint()
    {
        Vector3 point = centerPos + maxMoveDistance * Random.insideUnitCircle;
        point.z = zPos;
        Vector3 dir = (point - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Vector2.Distance(transform.position, point));
        if (hit.collider == null)
        {
            nextPoint = new Vector2(point.x, point.y);
            getEndRotation();
            moving = true;
        }
    }

    protected void getEndRotation()
    {
        Vector3 point = new Vector3(nextPoint.x, nextPoint.y, zPos);
        Vector3 dir = (point - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        endRot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle - 90);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerPos, maxMoveDistance);
        Gizmos.DrawWireSphere(centerPos, minMoveDistance);
    }
}
