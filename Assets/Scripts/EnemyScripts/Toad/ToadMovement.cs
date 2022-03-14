using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadMovement : EnemyMovement
{
    //time needed to ready jump
    public float readyJump;
    //readying jump
    bool readying = true;
    //jumping
    bool jumping;
    /// <summary>
    /// sets up basic movement information
    /// </summary>
    void Start()
    {
        attack = GetComponent<ToadAttack>();
        centerPos = transform.position;
        zPos = transform.position.z;
        endRot = Quaternion.identity;
    }
    /// <summary>
    /// moves in patrol pattern, except when player is spotted, then pursue
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, turnSpeed * Time.deltaTime);
            }
            if (endRot == transform.rotation)
            {
                if (readying)
                {
                    readying = false;
                    Wait(readyJump);
                    jumping = true;
                } 
                if (jumping)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    Vector2 pos = new Vector2(transform.position.x, transform.position.y);
                    if (Mathf.Abs(Vector2.Distance(pos, nextPoint)) < .5f)
                    {
                        waiting = true;
                        moving = false;
                        StartCoroutine(Wait(waitTime));
                    }
                }

                /*Vector2 pos = new Vector2(transform.position.x, transform.position.y);
                if (attacking && Mathf.Abs(Vector2.Distance(pos, nextPoint)) < attack.reach)
                {
                    moving = false;
                    attack.attack();
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                }
                pos = new Vector2(transform.position.x, transform.position.y);
                if (!attacking && Mathf.Abs(Vector2.Distance(pos, nextPoint)) < .5f)
                {
                    waiting = true;
                    moving = false;
                    StartCoroutine(wait(waitTime));
                }*/
            }
        }
    }
    /// <summary>
    /// wait at end of every jump
    /// </summary>
    /// <param name="time"> time to wait</param>
    /// <returns></returns>
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        waiting = false;
    }
}
