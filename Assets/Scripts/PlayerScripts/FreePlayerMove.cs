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
    [Tooltip("Movement direction")]
    [HideInInspector] public Vector2 dir = Vector2.zero;
    [Tooltip("Previous look direction")]
    private Vector2 previousDir = Vector2.zero;
    [Tooltip("Base speed of player")]
    [SerializeField] private float speed = 5f;
    [Tooltip("Rotation speed of player")]
    [SerializeField] private float rotSpeed = 10f;
    [Tooltip("Block breaking script")]
    private BlockBreaking blockBreaking;
    [Tooltip("Integer position of player")]
    public Vector3Int pos = Vector3Int.zero;
    [Tooltip("Previous integer position of player")]
    private Vector3Int prevpos = Vector3Int.zero;
    [Tooltip("Inventory game object")]
    [SerializeField] private GameObject inv;
    [Tooltip("Look direction")]
    private Vector2 rotDir = Vector2.zero;
    [Tooltip("Menu game object")]
    [SerializeField] private GameObject menu;
    [Tooltip("Magic menu game object")]
    [SerializeField] private GameObject magicTree;
    [Tooltip("Position of tile being looked at")]
    private Vector3Int lookPos = Vector3Int.zero;
    [Tooltip("Position of tile previously being looked at")]
    private Vector3Int prevlookPos = Vector3Int.zero;
    [Tooltip("Layer of interactable objects")]
    public LayerMask interactable;
    [Tooltip("Current chunk player is in")]
    [HideInInspector] public Vector2Int currentChunk = Vector2Int.zero;
    [Tooltip("Player can move")]
    [HideInInspector] public bool canMove = true;
    [Tooltip("Sprint modifier")]
    private float sprintMod = 1f;
    void Start()
    {
        GameObject grid = GameObject.Find("Grid");
        player = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
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
        pos.z = GameManager.Instance.mapz;
        prevpos.z = GameManager.Instance.mapz;
        if (GameManager.Instance.loadFromFile)
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
            GameManager.Instance.ResumeGame();
        }
        else if (!GameManager.Instance.paused)
        {
            magicTree.SetActive(true);
            GameManager.Instance.PauseGame();
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
            GameManager.Instance.ResumeGame();
        }
        else if (!GameManager.Instance.paused)
        {
            menu.SetActive(true);
            GameManager.Instance.PauseGame();
        }
    }
    /// <summary>
    /// Interacts with special tile when 'Space' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void Interact(CallbackContext ctx)
    {
        if (!GameManager.Instance.paused)
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
            GameManager.Instance.ResumeGame();
        }
        else if (!GameManager.Instance.paused)
        {
            inv.SetActive(true);
            GameManager.Instance.PauseGame();
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.paused && canMove)
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
        currentChunk = ChunkGen.Instance.GetChunkPos(relPos);
        Vector2Int newPos = ChunkGen.Instance.GetChunkTilePos(relPos);
        pos.x = newPos.x;
        pos.y = newPos.y;
        GameManager.Instance.pos = pos;
        GameManager.Instance.currentChunk = currentChunk;
        Vector3Int lookDir= Vector3Int.zero;
        if (rotDir != Vector2.zero)
            lookDir = new Vector3Int((int)rotDir.x, (int)rotDir.y, 0);
        else
            lookDir = new Vector3Int((int)previousDir.x, (int)previousDir.y, 0);
        lookPos = pos + lookDir;
        if (lookPos != prevlookPos)
        {
            if (GameManager.Instance.blockBreaking)
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
        currentChunk = ChunkGen.Instance.GetChunkPos(relPos);
        Vector2Int newPos = ChunkGen.Instance.GetChunkTilePos(relPos);
        pos.x = newPos.x;
        pos.y = newPos.y;
        GameManager.Instance.pos = pos;
        GameManager.Instance.currentChunk = currentChunk;
        ChunkGen.Instance.currentChunk = currentChunk;
        ChunkGen.Instance.GenerateSurroundingChunks();
    }
}
