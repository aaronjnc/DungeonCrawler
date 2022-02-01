using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.Tilemaps;

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
    DestroyandPlace blockplacing;
    public Vector3Int pos = Vector3Int.zero;
    Vector3Int prevpos = Vector3Int.zero;
    public GameObject canvas;
    Vector2 rotDir = Vector2.zero;
    public GameObject menu;
    public GameObject magicTree;
    Vector3Int lookPos = Vector3Int.zero;
    Vector3Int prevlookPos = Vector3Int.zero;
    public LayerMask interactable;
    [HideInInspector] public Vector2Int currentChunk = Vector2Int.zero;
    public bool canMove = true;
    void Start()
    {
        GameObject grid = GameObject.Find("Grid");
        player = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
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
        controls.Interact.Enter.canceled += Interact;
        controls.Interact.Enter.Enable();
        controls.Interact.Menu.performed += ActivateMenu;
        controls.Interact.Menu.Enable();
        controls.Fight.MagicMenu.performed += SpellMenu;
        controls.Fight.MagicMenu.Enable();
        pos.z = manager.mapz;
        prevpos.z = manager.mapz;
        if (manager.loadFromFile)
        {
            loadFromFile(manager.GetGameInformation());
        }
    }
    /// <summary>
    /// Activates the spell menu when 'X' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void SpellMenu(CallbackContext ctx)
    {
        if (magicTree.activeInHierarchy)
        {
            magicTree.SetActive(false);
            manager.ResumeGame();
        }
        else if (!manager.paused)
        {
            magicTree.SetActive(true);
            manager.PauseGame();
        }
    }
    /// <summary>
    /// Activates menu when 'Esc' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void ActivateMenu(CallbackContext ctx)
    {
        if (menu.activeInHierarchy)
        {
            menu.SetActive(false);
            manager.ResumeGame();
        }
        else if (!manager.paused)
        {
            menu.SetActive(true);
            manager.PauseGame();
        }
    }
    /// <summary>
    /// Interacts with special tile when 'Space' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void Interact(CallbackContext ctx)
    { 
        if (!manager.paused)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2, interactable);
            if (hit.collider != null)
            {
                hit.collider.gameObject.GetComponent<InteractableTile>().Interact();
            }
        }
    }
    /// <summary>
    /// Opens inventory when 'I' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void Inventory(CallbackContext ctx)
    {
        if (canvas.activeInHierarchy)
        {
            canvas.SetActive(false);
            manager.ResumeGame();
        }
        else if (!manager.paused)
        {
            canvas.SetActive(true);
            manager.PauseGame();
        }
    }

    void FixedUpdate()
    {
        if (!manager.paused && canMove)
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
    /// <summary>
    /// Calls actions that are affected on player movement
    /// </summary>
    void Actions()
    {
        Vector2Int relPos = new Vector2Int((int)Mathf.Round(transform.position.x - .5f), (int)Mathf.Round(transform.position.y - .5f));
        currentChunk = ChunkGen.currentWorld.GetChunkPos(relPos);
        Vector2Int newPos = ChunkGen.currentWorld.GetChunkTilePos(relPos);
        pos.x = newPos.x;
        pos.y = newPos.y;
        manager.pos = pos;
        manager.currentChunk = currentChunk;
        Vector3Int lookDir= Vector3Int.zero;
        if (rotDir != Vector2.zero)
            lookDir = new Vector3Int((int)rotDir.x, (int)rotDir.y, 0);
        else
            lookDir = new Vector3Int((int)previousDir.x, (int)previousDir.y, 0);
        lookPos = pos + lookDir;
        if (lookPos != prevlookPos)
        {
            if (manager.blockBreaking)
            {
                blockplacing.enabled = true;
                blockplacing.Positioning(lookPos,currentChunk);
            }
            prevlookPos = lookPos;
        }
        if (rotDir != Vector2.zero)
            previousDir = rotDir;
    }
    private void OnDestroy()
    {
        controls.Disable();
    }
    /// <summary>
    /// loads player information from file
    /// </summary>
    /// <param name="info"></param>
    private void loadFromFile(GameInformation info)
    {
        transform.position = new Vector3(info.playerPos[0], info.playerPos[1], info.playerPos[2]);
        transform.eulerAngles = new Vector3(info.playerRot[0], info.playerRot[1], info.playerRot[2]);
    }
}
