using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SpellSelector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //mouse is down
    private bool mouseDown = false;
    //start pos of player
    Vector3 startpos = Vector3.zero;
    //item position
    Vector2Int itemPos;
    //spell manager script
    public SpellManager spellManager;
    //activated
    public bool activated;
    //spell position
    public int spellPos;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
    }
    /// <summary>
    /// On mouse press
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (activated)
            mouseDown = true;
    }
    /// <summary>
    /// On mouse lift
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (mouseDown)
        {
            mouseDown = false;
            spellManager.PlaceSpell(gameObject, spellPos);
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
