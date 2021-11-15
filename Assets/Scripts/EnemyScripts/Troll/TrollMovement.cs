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
    public float waitTime;
    private float timeWaited;
    public float moveRadius;
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<EnemyAttack>();
        centerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            nextPoint = centerPos + moveRadius * Random.insideUnitCircle;
            moving = true;
        } else
        {
            if (nextPoint != transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
            }
        }
    }
}
