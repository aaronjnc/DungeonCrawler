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
    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<EnemyAttack>();
        centerpos = transform.position;
        Vector2 newPosition = centerpos + Random.insideUnitCircle * radius;
        Vector2 dir = (centerpos - newPosition).normalized;
        float magnitude = (centerpos - newPosition).magnitude;
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

    }
    /// <summary>
    /// Determine whether or not to rotate enemy
    /// </summary>
    public void Rotate()
    {

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
}
