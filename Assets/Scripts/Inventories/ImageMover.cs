using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
[RequireComponent(typeof(BoxCollider2D))]
public class ImageMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameManager manager;
    bool mouseDown = false;
    Vector3 startpos = Vector3.zero;
    Vector2Int itemPos;
    Inventory inv;
    List<GameObject> otherImages = new List<GameObject>();
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
        if (!inv.IsEmpty(itemPos))
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
    void OnTriggerEnter2D(Collider2D other)
    {
        otherImages.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        otherImages.Remove(other.gameObject);
    }
    private void DropItem()
    {
        int closestObj = -1;
        float closestDistance = int.MaxValue;
        for (int i = 0; i < otherImages.Count; i++)
        {
            float dist = Vector2.Distance(otherImages[i].transform.position, transform.position);
            if (dist < closestDistance && dist < 20)
            {
                closestObj = i;
                closestDistance = dist;
            }
        }
        if (closestObj != -1)
        {
            inv.DropItem(getArrayPos(), otherImages[closestObj].GetComponent<ImageMover>().getArrayPos());
        }
        otherImages.Clear();
        transform.position = startpos;
    }
}
