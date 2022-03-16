using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fireball : MonoBehaviour
{
    public float speed;
    Vector3 dir;
    public LayerMask tiles;
    public LayerMask enemies;
    public LayerMask ignoreLayers;
    int mapz;
    public float baseDamage;
    // Start is called before the first frame update
    void Start()
    {
        mapz = GameManager.Instance.mapz;
        dir = transform.up;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += speed * -dir*Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1, enemies);
        foreach (Collider2D collider in colliders)
        {
            float mod = 1;
            if (collider.Equals(collision))
            {
                mod = 1.5f;
            }
            EnemyInfo info;
            if (collider.gameObject.TryGetComponent<EnemyInfo>(out info))
            {
                info.ReduceHealth(baseDamage * mod);
                info.FireDamage();
            }
        }
        Vector2Int hitLocation = new Vector2Int((int)transform.position.x,(int)transform.position.y);
        for (int x = hitLocation.x-1; x<=hitLocation.x+1;x++)
        {
            for (int y = hitLocation.y-1;y<=hitLocation.y+1;y++)
            {
                //Set tile on fire
            }
        }
        Destroy(gameObject);
    }
}
