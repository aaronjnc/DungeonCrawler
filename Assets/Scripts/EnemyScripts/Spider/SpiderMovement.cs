using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : EnemyMovement
{
    /// <summary>
    /// sets up basic movement information
    /// </summary>
    void Start()
    {
        centerPos = transform.position;
        zPos = transform.position.z;
        endRot = Quaternion.identity;
    }

    /// <summary>
    /// Moves spider on patrol path, once player is spotted begin following
    /// </summary>
    void FixedUpdate()
    {
        if (!moving && !waiting)
        {
            SetNextPoint();
        }
        else if (moving)
        {
            Vector3 target = new Vector3(nextPoint.x, nextPoint.y, zPos);
            if (endRot != transform.rotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, turnSpeed * Time.deltaTime);
            }
            if (endRot == transform.rotation)
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                pos = new Vector2(transform.position.x, transform.position.y);
                if (Mathf.Abs(Vector2.Distance(pos, nextPoint)) < .5f)
                {
                    waiting = true;
                    moving = false;
                    StartCoroutine(Wait());
                }
            }
        }
    }
    /// <summary>
    /// wait at end of patrol point
    /// </summary>
    /// <returns></returns>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }
}
