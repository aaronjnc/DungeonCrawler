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
        TilemapRenderer renderer;
        if (collision.gameObject.TryGetComponent(out renderer))
            ChunkGen.currentWorld.LoadChunk(collision.gameObject.transform.position);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyMovement enemy;
        if (collision.gameObject.TryGetComponent(out enemy))
        {
            enemy.activated = false;
        }
        TilemapRenderer renderer;
        if (collision.gameObject.TryGetComponent(out renderer))
            ChunkGen.currentWorld.UnloadChunk(collision.gameObject.transform.position);
    }
}
