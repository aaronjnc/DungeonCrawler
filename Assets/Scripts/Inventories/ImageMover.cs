using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
/// <summary>
/// Script used to move items around in the inventory
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ImageMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //textbox prefab representing how many items are in current stack
    public Text itemCount;
    //gameobject instantiated at runtime to hold item count
    private GameObject itemCounter;
    //image is movable
    public bool movable = true;
    //manager reference
    GameManager manager;
    //if mouse is held down on image
    bool mouseDown = false;
    //initial position of image
    Vector3 startpos = Vector3.zero;
    //array position of item
    public Vector2Int itemPos;
    //inventory reference
    Inventory inv;
    /// <summary>
    /// Sets up manager, inventory reference, and start pos
    /// </summary>
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
        if (itemPos.x < 10)
        {
            itemCounter = Instantiate(itemCount.gameObject, transform);
            UpdateCount(0);
        }
    }
    /// <summary>
    /// returns item array position
    /// </summary>
    /// <returns>Vector2Int array pos</returns>
    public Vector2Int getArrayPos()
    {
        return itemPos;
    }
    /// <summary>
    /// sets mouse down to true when clicked
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (movable && !inv.IsEmpty(itemPos))
        {
            mouseDown = true;
        }
    }
    /// <summary>
    /// sets mouse down to false when pointer lifted, calls drop item method
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (mouseDown)
        {
            mouseDown = false;
            DropItem();
        }
        transform.position = startpos;
    }
    /// <summary>
    /// moves image if mouseDown is true
    /// </summary>
    void Update()
    {
        if (mouseDown)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            transform.position = new Vector3(pos.x, pos.y, startpos.z);
        }
    }
    /// <summary>
    /// drops image, resetting object to original position then seeing if the item moves to another slot
    /// </summary>
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
            if (dist < closestDistance && dist < collisions[i].bounds.extents.x)
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
    /// <summary>
    /// updates item counter
    /// </summary>
    /// <param name="count">number of items</param>
    public void UpdateCount(int count)
    {
        if (count == 0)
        {
            itemCounter.SetActive(false);
            return;
        }
        itemCounter.GetComponent<Text>().text = count.ToString();
        itemCounter.SetActive(true);
    }
}
