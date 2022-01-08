using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyMovement : MonoBehaviour
{
    //movement speed of enemy
    public float speed;
    //turn speed of enemy
    public float turnSpeed;
    //maximum distance of patrol point
    public float maxMoveDistance;
    //minimum distance of patrol point
    public float minMoveDistance;
    //enemy spawn point
    protected Vector2 centerPos;
    //enemy attack script
    protected EnemyAttack attack;
    //next patrol point
    protected Vector2 nextPoint;
    //quaternion rotating to
    protected Quaternion endRot;
    //if enemy is attacking
    protected bool attacking;
    //z coordinate
    protected float zPos;
    //true if moving
    protected bool moving;
    //true if waiting
    protected bool waiting;
    //how long to wait between moves
    public float waitTime;
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
            getEndRotation();
            moving = true;
        }
    }
    /// <summary>
    /// gets rotation to travel to next point
    /// </summary>
    protected void getEndRotation()
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
