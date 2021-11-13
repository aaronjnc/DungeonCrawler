using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

public class PlayerFight : MonoBehaviour
{
    PlayerControls controls;
    GameManager manager;
    ItemReference invItem;
    public float health;
    public float maxHealth = 100;
    public Slider healthSlider;
    public LayerMask enemy;
    public float reach;
    // Start is called before the first frame update
    void Start()
    {
        invItem = new ItemReference();
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;
        healthSlider.value = health;
        controls = new PlayerControls();
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
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
        if (manager.fighting && !manager.paused)
        {
            invItem.ChangeValues(manager.currentItem);
            if (invItem.fighting)
            {
                invItem.weaponScript.BaseAttack(gameObject.transform);
            }
        }
    }
    /// <summary>
    /// Player advanced attackw when 'Right Mouse Button' is pressed
    /// </summary>
    /// <param name="ctx"></param>
    void AdvancedAttack(CallbackContext ctx)
    {
        if (manager.fighting && !manager.paused)
        {
            invItem.ChangeValues(manager.currentItem);
            if (invItem.fighting)
            {
                invItem.weaponScript.AdvancedAttack(gameObject.transform);
            }
        }
    }        
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
        controls.Disable();
    }
}
