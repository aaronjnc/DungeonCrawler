using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraTrigger : MonoBehaviour
{
    /// <summary>
    /// Loads chunks and enemies upon contact
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterMovement enemy;
        if (collision.gameObject.TryGetComponent(out enemy))
        {
            enemy.enabled = true;
        }
        if (collision.gameObject.tag.Equals("MapCollider"))
            ChunkGen.Instance.LoadChunk(collision.gameObject.transform.parent.transform.position);
    }
    /// <summary>
    /// Unloads enemies and chunks when leaving
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        MonsterMovement enemy;
        if (collision.gameObject.TryGetComponent(out enemy))
        {
            enemy.enabled = false;
        }
        if (collision.gameObject.tag.Equals("MapCollider"))
            ChunkGen.Instance.UnloadChunk(collision.gameObject.transform.parent.transform.position);
    }
}
