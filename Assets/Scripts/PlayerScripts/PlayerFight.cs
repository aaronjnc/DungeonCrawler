using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

public class PlayerFight : MonoBehaviour
{
    [Tooltip("Pplayer controls")]
    private PlayerControls controls;
    [Tooltip("Chosen item")]
    private ItemSlot invItem;
    [Tooltip("Player health")] [HideInInspector]
    public float health;
    [Tooltip("Max player health")]
    [SerializeField] private float maxHealth = 100;
    [Tooltip("Health slider")]
    [SerializeField] private Slider healthSlider;
    [Tooltip("Enemy layer")]
    [SerializeField] private LayerMask enemy;
    [Tooltip("Player reach")]
    [SerializeField] private float reach;
    // Start is called before the first frame update
    void Start()
    {
        invItem = new ItemSlot();
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;
        healthSlider.value = health;
        controls = new PlayerControls();
        controls.Interact.Press.canceled += BaseAttack;
        controls.Interact.Press.Enable();
        controls.Fight.AdvanceAttack.canceled += AdvancedAttack;
        controls.Fight.AdvanceAttack.Enable();
        controls.Fight.Magic.performed += CastSpell;
        controls.Fight.Magic.Enable();
    }
    /// <summary>
    /// Cast spell when number is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void CastSpell(CallbackContext ctx)
    {
        int num;
        int.TryParse(ctx.control.name, out num);
        num--;
        GetComponent<Magic>().PerformSpell(num);
    }
    /// <summary>
    /// Player base attack when 'Left Mouse Button' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void BaseAttack(CallbackContext ctx)
    {
        /*if (GameManager.Instance.fighting && !GameManager.Instance.paused)
        {
            invItem.AddExisting(GameManager.Instance.currentItem);
            if (invItem.IsWeapon())
            {
                invItem.GetWeaponScript().BaseAttack(gameObject.transform);
            }
        }*/
    }
    /// <summary>
    /// Player advanced attackw when 'Right Mouse Button' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void AdvancedAttack(CallbackContext ctx)
    {
        /*if (GameManager.Instance.fighting && !GameManager.Instance.paused)
        {
            invItem.AddExisting(GameManager.Instance.currentItem);
            if (invItem.IsWeapon())
            {
                invItem.GetWeaponScript().AdvancedAttack(gameObject.transform);
            }
        }*/
    }        
    /// <summary>
    /// deals given damage to player
    /// </summary>
    /// <param name="amount">amount of damage player takes</param>
    public void TakeDamage(float amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        healthSlider.value = health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (controls != null)
            controls.Disable();
    }
}
