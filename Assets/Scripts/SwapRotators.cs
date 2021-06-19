using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class SwapRotators : MonoBehaviour
{
    PlayerControls controls;
    public GameObject[] rotators = new GameObject[4];
    public Sprite[] fullRotators = new Sprite[5];
    public Sprite smallRotator;
    [HideInInspector]
    public int current = 0;
    int previous = 0;
    public int chosen = 0;
    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        controls.Interact.SwitchItem.performed += SwitchRotator;
        controls.Interact.SwitchItem.Enable();
    }
    void SwitchRotator(CallbackContext ctx)
    {
        previous = current;
        current = Mathf.Clamp(current + (int)ctx.ReadValue<float>(), 0, 3);
        if (current != previous)
        {
            rotators[previous].SetActive(false);
            rotators[current].SetActive(true);
        }
        UpdateRotator(current);
    }
    public void UpdateRotator(int i)
    {
        rotators[i].GetComponent<ItemRotator>().UpdateItems();
    }
}
