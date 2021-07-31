using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraTrigger : MonoBehaviour
{
    BoxCollider2D cameraCollider;
    private void Start()
    {
        cameraCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMovement enemy;
        if (collision.gameObject.TryGetComponent(out enemy))
        {
            enemy.activated = true;
        }
        if (collision.gameObject.tag.Equals("MapCollider"))
            ChunkGen.currentWorld.LoadChunk(collision.gameObject.transform.parent.transform.position);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyMovement enemy;
        if (collision.gameObject.TryGetComponent(out enemy))
        {
            enemy.activated = false;
        }
        if (collision.gameObject.tag.Equals("MapCollider"))
            ChunkGen.currentWorld.UnloadChunk(collision.gameObject.transform.parent.transform.position);
    }
}
