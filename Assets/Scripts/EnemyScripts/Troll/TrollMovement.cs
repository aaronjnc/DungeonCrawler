using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollMovement : EnemyMovement
{
    /// <summary>
    /// sets up basic movement information
    /// </summary>
    void Start()
    {
        attack = GetComponent<TrollAttack>();
        centerPos = transform.position;
        zPos = transform.position.z;
        endRot = Quaternion.identity;
    }

    /// <summary>
    /// patrols until player is spotted then pursue
    /// </summary>
    void FixedUpdate()
    {
        attacking = attack.spotted;
        if (attacking)
        {
            nextPoint = new Vector2(attack.lastLocation.x, attack.lastLocation.y);
            GetEndRotation();
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
                    attack.Attack();
                } else
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                }
                pos = new Vector2(transform.position.x, transform.position.y);
                if (!attacking && Mathf.Abs(Vector2.Distance(pos, nextPoint)) < .5f)
                {
                    waiting = true;
                    moving = false;
                    StartCoroutine(Wait());
                }
            }
        }
    }
    /// <summary>
    /// wait at end of every patrol point
    /// </summary>
    /// <returns></returns>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }
}
