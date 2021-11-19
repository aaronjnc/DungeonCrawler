using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollMovement : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    Vector2 centerPos;
    EnemyAttack attack;
    Vector2 nextPoint;
    bool moving;
    bool waiting;
    public float waitTime;
    public float moveRadius;
    float zPos;
    Quaternion endRot;
    bool attacking;
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<TrollAttack>();
        centerPos = transform.position;
        zPos = transform.position.z;
        endRot = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        attacking = attack.spotted;
        if (attacking)
        {
            nextPoint = new Vector2(attack.lastLocation.x, attack.lastLocation.y);
            getEndRotation();
            moving = true;
        }
        if (!moving && !waiting)
        {
            SetNextPoint();
        }
        else if (moving)
        {
            Vector3 target = new Vector3(nextPoint.x, nextPoint.y, zPos);
            if (endRot != transform.rotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, turnSpeed*Time.deltaTime);
            }
            if (endRot == transform.rotation)
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y);
                if (attacking && Mathf.Abs(Vector2.Distance(pos, nextPoint)) < attack.reach)
                {
                    moving = false;
                    attack.attack();
                } else
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                }
                pos = new Vector2(transform.position.x, transform.position.y);
                if (!attacking && Mathf.Abs(Vector2.Distance(pos, nextPoint)) < .5f)
                {
                    waiting = true;
                    moving = false;
                    StartCoroutine(wait());
                }
            }
        }
    }
    void SetNextPoint()
    {
        Vector3 point = centerPos + moveRadius * Random.insideUnitCircle;
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

    private void getEndRotation()
    {
        Vector3 point = new Vector3(nextPoint.x, nextPoint.y, zPos);
        Vector3 dir = (point - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        endRot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle - 90);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerPos, moveRadius);
    }
}
