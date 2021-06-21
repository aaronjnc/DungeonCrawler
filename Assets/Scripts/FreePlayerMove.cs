using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FreePlayerMove : MonoBehaviour
{
    Rigidbody2D player;
    PlayerControls controls;
    GameManager manager;
    public Vector2 dir = Vector2.zero;
    Vector2 previousDir = Vector2.zero;
    public float speed = 5f;
    public float rotSpeed = 10f;
    RopeSystem ropesystem;
    DestroyandPlace blockplacing;
    public Vector3Int pos = Vector3Int.zero;
    Tilemap map;
    Vector3Int prevpos = Vector3Int.zero;
    GameObject canvas;
    Vector2 rotDir = Vector2.zero;
    [HideInInspector]
    public bool inventoryOpen = false;
    public GameObject marketPlace;
    public GameObject playerMarket;
    public GameObject menu;
    public GameObject magicTree;
    // Start is called before the first frame update
    void Start()
    {
        GameObject grid = GameObject.Find("Grid");
        player = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        canvas = manager.invObject;
        ropesystem = grid.GetComponent<RopeSystem>();
        blockplacing = grid.GetComponent<DestroyandPlace>();
        controls.Movement.Horizontal.performed += ctx => dir.x += ctx.ReadValue<float>();
        controls.Movement.Horizontal.canceled += ctx => dir.x = 0;
        controls.Movement.Horizontal.Enable();
        controls.Movement.Vertical.performed += ctx => dir.y += ctx.ReadValue<float>();
        controls.Movement.Vertical.canceled += ctx => dir.y = 0;
        controls.Movement.Vertical.Enable();
        controls.Interact.Inventory.performed += Inventory;
        controls.Interact.Inventory.Enable();
        controls.Movement.MousePosition.Enable();
        controls.Interact.Enter.canceled += Enter;
        controls.Interact.Enter.Enable();
        controls.Interact.Menu.performed += ActivateMenu;
        controls.Interact.Menu.Enable();
        controls.Fight.MagicMenu.performed += SpellMenu;
        controls.Fight.MagicMenu.Enable();
    }
    void SpellMenu(CallbackContext ctx)
    {
        if (magicTree.activeInHierarchy)
            magicTree.SetActive(false);
        else
            magicTree.SetActive(true);
    }
    void ActivateMenu(CallbackContext ctx)
    {
        if (menu.activeInHierarchy)
            menu.SetActive(false);
        else
            menu.SetActive(true);
    }
    void Enter(CallbackContext ctx)
    { 
        if (!marketPlace.activeInHierarchy)
        {
            Vector3Int lookDir = new Vector3Int((int)rotDir.x, (int)rotDir.y, -1);
            if (manager.markets.Contains(pos+lookDir))
            {
                int loc = manager.markets.IndexOf(pos + lookDir);
                marketPlace.GetComponent<MarketPlace>().SetVendor(manager.vendors[loc]);
                marketPlace.SetActive(true);
                playerMarket.SetActive(true);
                inventoryOpen = true;
                manager.invOpen = true;
            }
        }
        else
        {
            marketPlace.SetActive(false);
            playerMarket.SetActive(false);
            inventoryOpen = false;
            manager.invOpen = false;
        }
    }
        
    void Inventory(CallbackContext ctx)
    {
        if (!canvas.activeInHierarchy)
        {
            canvas.SetActive(true);
            inventoryOpen = true;
            manager.invOpen = true;
        }
        else
        {
            canvas.SetActive(false);
            inventoryOpen = false;
            manager.invOpen = false;
        }
    }

    void FixedUpdate()
    {
        if (!inventoryOpen)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(controls.Movement.MousePosition.ReadValue<Vector2>());
            float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            dir = dir.normalized;
            Vector3 velDir = -dir.x * transform.up + dir.y * transform.right;
            rotDir = new Vector2(Mathf.Round(transform.up.y), Mathf.Round(-transform.up.x));
            player.velocity = velDir * speed;
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            Actions();
            if (pos != prevpos)
                prevpos = pos;
        }
    }
    void Actions()
    {
        pos = map.WorldToCell(transform.position);
        manager.pos = pos;
        Vector3Int lookDir= Vector3Int.zero;
        if (rotDir != Vector2.zero)
            lookDir = new Vector3Int((int)rotDir.x, (int)rotDir.y, 0);
        else
            lookDir = new Vector3Int((int)previousDir.x, (int)previousDir.y, 0);
        if (manager.blockplacing)
        {
            blockplacing.enabled = true;
            blockplacing.Positioning(pos + lookDir);
        }
        else if (manager.ropeplacing)
        {
            if (!ropesystem.enabled)
                ropesystem.enabled = true;
            if (pos != prevpos)
                ropesystem.Roping(pos);
        }
        if (rotDir != Vector2.zero)
            previousDir = rotDir;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMovement enemy;
        if (collision.gameObject.TryGetComponent<EnemyMovement>(out enemy))
        {
            enemy.activated = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyMovement enemy;
        if (collision.gameObject.TryGetComponent<EnemyMovement>(out enemy))
        {
            enemy.activated = false;
        }
    }
}
