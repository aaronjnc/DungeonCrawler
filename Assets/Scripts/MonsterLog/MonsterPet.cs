using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class MonsterPet : MonoBehaviour
{
    [SerializeField]
    private LayerMask monsterLayer;
    PlayerControls controls;
    [SerializeField]
    private float distance;
    [SerializeField]
    private MonsterLog log;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Interact.Press.performed += ClickEvent;
        controls.Interact.Press.Enable();
    }

    private void ClickEvent(CallbackContext ctx)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - transform.position, distance, monsterLayer);
        if (hit.collider != null)
        {
            GameObject monster = hit.collider.gameObject;
            MonsterInfo monsterInfo = monster.GetComponent<MonsterInfo>();
            log.Register(monsterInfo);
        }
    }

    private void OnDestroy()
    {
        if (controls != null)
            controls.Disable();
    }
    public MonsterLog GetLog()
    {
        return log;
    }
}
