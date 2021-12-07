using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : EnemyMovement
{
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<SpiderAttack>();
        centerPos = transform.position;
        zPos = transform.position.z;
        endRot = Quaternion.identity;
    }

    // Update is called once per frame
    void FixedUpdate()
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, turnSpeed * Time.deltaTime);
            }
            if (endRot == transform.rotation)
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y);
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
                    StartCoroutine(wait());
                }
            }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }
}
