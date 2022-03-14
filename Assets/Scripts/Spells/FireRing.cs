using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : MonoBehaviour
{
    public float radius;
    public float speed;
    public float damage;

    // Update is called once per frame
    void FixedUpdate()
    {
        float newscale = speed * Time.deltaTime;
        transform.localScale += new Vector3(newscale, newscale, 0);
        if (transform.localScale.x >= radius)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyInfo info;
        if (collision.gameObject.TryGetComponent<EnemyInfo>(out info))
        {
            info.ReduceHealth(damage);
            info.FireDamage();
        }
    }
}
