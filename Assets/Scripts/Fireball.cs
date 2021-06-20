using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fireball : MonoBehaviour
{
    GameManager manager;
    public float speed;
    Vector3 dir;
    public LayerMask tiles;
    public LayerMask enemies;
    Tilemap map;
    int mapz;
    public float baseDamage;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        map = manager.map;
        mapz = manager.mapz;
        dir = transform.up;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += speed * -dir*Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1,enemies);
        foreach(Collider2D collider in colliders)
        {
            float mod = 1;
            if (collider.Equals(collision))
            {
                mod = 1.5f;
            }
            EnemyInfo info;
            if (collider.gameObject.TryGetComponent<EnemyInfo>(out info))
            {
                info.ReduceHealth(baseDamage*mod);
                info.FireDamage();
            }
        }
        Vector3Int tilePos = map.WorldToCell(transform.position);
        for (int x = tilePos.x-1; x<=tilePos.x+1;x++)
        {
            for (int y = tilePos.y-1;y<=tilePos.y+1;y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector3Int newPos = new Vector3Int(x, y, mapz);
                if (map.GetTile<Tile>(newPos).sprite == null)
                    continue;
                if (manager.breakable(newPos))
                {
                    map.SetTile(newPos, null);
                    map.RefreshTile(newPos);
                }
            }
        }
        if (collision.gameObject.name != "Player")
            Destroy(gameObject);
    }
}
