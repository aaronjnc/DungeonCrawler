using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class FreePlayerMove : MonoBehaviour
{
    [Tooltip("player body")]
    private Rigidbody2D player;
    [Tooltip("player controls")]
    private PlayerControls controls;
    //game manager
    private GameManager manager;
    //look direction
    public Vector2 dir = Vector2.zero;
    //previous look direction
    private Vector2 previousDir = Vector2.zero;
    //player speed
    public float speed = 5f;
    //player rotation speed
    public float rotSpeed = 10f;
    //block breaking script
    private BlockBreaking blockBreaking;
    //current tile position
    public Vector3Int pos = Vector3Int.zero;
    //previous tile position
    private Vector3Int prevpos = Vector3Int.zero;
    //inventory
    public GameObject inv;
    //rotation direction
    private Vector2 rotDir = Vector2.zero;
    //menu object
    public GameObject menu;
    //magic menu object
    public GameObject magicTree;
    //look position
    private Vector3Int lookPos = Vector3Int.zero;
    //previous look position
    private Vector3Int prevlookPos = Vector3Int.zero;
    //interactable layer mask
    public LayerMask interactable;
    [Tooltip("current chunk")]
    [HideInInspector] public Vector2Int currentChunk = Vector2Int.zero;
    [Tooltip("player can move")]
    public bool canMove = true;
    [Tooltip("sprint modifier")]
    private float sprintMod = 1f;
    void Start()
    {
        GameObject grid = GameObject.Find("Grid");
        player = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        blockBreaking = grid.GetComponent<BlockBreaking>();
        controls.Movement.Horizontal.performed += ctx => dir.x += ctx.ReadValue<float>();
        controls.Movement.Horizontal.canceled += ctx => dir.x = 0;
        controls.Movement.Horizontal.Enable();
        controls.Movement.Vertical.performed += ctx => dir.y += ctx.ReadValue<float>();
        controls.Movement.Vertical.canceled += ctx => dir.y = 0;
        controls.Movement.Vertical.Enable();
        controls.Movement.Sprint.performed += ctx => sprintMod = 2f;
        controls.Movement.Sprint.canceled += ctx => sprintMod = 1f;
        controls.Movement.Sprint.Enable();
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
            LoadFromFile();
        }
        SaveSystem.Save();
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 5, interactable);
            if (hit.collider != null)
            {
                SaveSystem.Save();
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
        if (inv.activeInHierarchy)
        {
            inv.SetActive(false);
            manager.ResumeGame();
        }
        else if (!manager.paused)
        {
            inv.SetActive(true);
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
            Vector3 velDir = (-dir.x * transform.up + dir.y * transform.right) * sprintMod;
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
                blockBreaking.enabled = true;
                blockBreaking.Positioning(lookPos,currentChunk);
            }
            prevlookPos = lookPos;
        }
        if (rotDir != Vector2.zero)
            previousDir = rotDir;
    }
    private void OnDestroy()
    {
        if (controls != null)
            controls.Disable();
    }
    /// <summary>
    /// loads player information from file
    /// </summary>
    private void LoadFromFile()
    {
        PlayerSave p = GameInformation.Instance.LoadPlayer();
        transform.position = p.GetPlayerPos();
        transform.eulerAngles = p.GetPlayerRot();
        Vector2Int relPos = new Vector2Int((int)Mathf.Round(transform.position.x - .5f), (int)Mathf.Round(transform.position.y - .5f));
        currentChunk = ChunkGen.currentWorld.GetChunkPos(relPos);
        Vector2Int newPos = ChunkGen.currentWorld.GetChunkTilePos(relPos);
        pos.x = newPos.x;
        pos.y = newPos.y;
        manager.pos = pos;
        manager.currentChunk = currentChunk;
        ChunkGen.currentWorld.currentChunk = currentChunk;
        ChunkGen.currentWorld.GenerateSurroundingChunks();
    }
}
