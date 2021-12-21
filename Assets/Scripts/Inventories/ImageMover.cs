using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
[RequireComponent(typeof(BoxCollider2D))]
public class ImageMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool movable = true;
    GameManager manager;
    bool mouseDown = false;
    Vector3 startpos = Vector3.zero;
    public Vector2Int itemPos;
    Inventory inv;
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        inv = manager.inv;
        startpos = transform.position;
    }
    /// <summary>
    /// Array position of this image
    /// </summary>
    /// <param name="arrayPos">Array position</param>
    public void SetArrayPos(Vector2Int arrayPos)
    {
        itemPos = arrayPos;
    }
    public Vector2Int getArrayPos()
    {
        return itemPos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (movable && !inv.IsEmpty(itemPos))
        {
            mouseDown = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (mouseDown)
        {
            mouseDown = false;
            DropItem();
        }
        transform.position = startpos;
    }
    void Update()
    {
        if (mouseDown)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            transform.position = new Vector3(pos.x, pos.y, startpos.z);
        }
    }
    private void DropItem()
    {
        var old = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = true;
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 30);
        Physics2D.queriesHitTriggers = old;
        float closestDistance = int.MaxValue;
        GameObject closestObj = null;
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].gameObject == gameObject)
                continue;
            float dist = Vector2.Distance(collisions[i].gameObject.transform.position, transform.position);
            if (dist < closestDistance && dist < 20)
            {
                closestObj = collisions[i].gameObject;
                closestDistance = dist;
            }
        }
        if (closestObj != null)
        {
            inv.DropItem(getArrayPos(), closestObj.GetComponent<ImageMover>().getArrayPos());
        }
        transform.position = startpos;
    }
}
