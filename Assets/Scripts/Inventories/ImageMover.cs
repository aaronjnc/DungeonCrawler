using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ImageMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameManager manager;
    bool mouseDown = false;
    Vector3 startpos = Vector3.zero;
    Vector2Int itemPos;
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
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!inv.isEmpty(itemPos))
        {
            mouseDown = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (mouseDown)
        {
            mouseDown = false;
            inv.DropItem(itemPos, transform.localPosition);
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
}
