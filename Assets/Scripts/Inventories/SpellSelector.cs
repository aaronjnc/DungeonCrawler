using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SpellSelector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameManager manager;
    bool mouseDown = false;
    Vector3 startpos = Vector3.zero;
    Vector2Int itemPos;
    public SpellManager spellManager;
    public bool activated;
    public int spellPos;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        startpos = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (activated)
            mouseDown = true;
    }
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
