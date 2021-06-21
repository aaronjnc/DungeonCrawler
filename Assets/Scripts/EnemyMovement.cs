using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    public float radius = 10f;
    Vector3 centerpos;
    EnemyAttack attack;
    public bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<EnemyAttack>();
        centerpos = transform.position;
    }
    float waittime = 10f;
    float timewaited = 0f;
    // Update is called once per frame
    void Update()
    {
        if (!attack.spotted)
        {
            timewaited += Time.deltaTime;
        }
    }
}
