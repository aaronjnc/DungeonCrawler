using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyMovement : MonoBehaviour
{
    [Tooltip("Speed of enemy")]
    [SerializeField] protected float speed;
    [Tooltip("Turn speed of enemy")]
    [SerializeField] protected float turnSpeed;
    [Tooltip("Maximum move distance")]
    [SerializeField] protected float maxMoveDistance;
    [Tooltip("Minimum move distance")]
    [SerializeField] protected float minMoveDistance;
    [Tooltip("Enemy spawn point")]
    protected Vector2 centerPos;
    [Tooltip("Next patrol point")]
    protected Vector2 nextPoint;
    [Tooltip("End rotation")]
    protected Quaternion endRot;
    [Tooltip("Z position")]
    protected float zPos;
    [Tooltip("Is moving")]
    protected bool moving;
    [Tooltip("Waiting at patrol point")]
    protected bool waiting;
    [Tooltip("How long to wait at each point")]
    [SerializeField] protected float waitTime;
    /// <summary>
    /// sets next travel point
    /// </summary>
    protected void SetNextPoint()
    {
        Vector3 point = centerPos + maxMoveDistance * Random.insideUnitCircle;
        point.z = zPos;
        Vector3 dir = (point - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Vector2.Distance(transform.position, point));
        if (hit.collider == null)
        {
            nextPoint = new Vector2(point.x, point.y);
            GetEndRotation();
            moving = true;
        }
    }
    /// <summary>
    /// gets rotation to travel to next point
    /// </summary>
    protected void GetEndRotation()
    {
        Vector3 point = new Vector3(nextPoint.x, nextPoint.y, zPos);
        Vector3 dir = (point - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        endRot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle - 90);
    }
    /// <summary>
    /// draws movement spheres
    /// </summary>
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerPos, maxMoveDistance);
        Gizmos.DrawWireSphere(centerPos, minMoveDistance);
    }
}
