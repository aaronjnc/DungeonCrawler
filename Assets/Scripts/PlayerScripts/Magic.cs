using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magic : MonoBehaviour
{
    int[] enabledSpells = new int[5];
    delegate void Actions();
    bool[] charged = new bool[5];
    public float[] chargeTime = new float[15];
    public float[] magicCost = new float[15];
    float[] currentCharge = new float[5];
    Actions[] allSpells = new Actions[15];
    Actions[] spells = new Actions[5];
    public GameObject fireball;
    public GameObject fireRing;
    bool spellDisabled = false;
    List<int> disabledSpells = new List<int>();
    public Image[] chosenImages;
    public float magic;
    public float maxMagic = 100;
    public Slider magicSlider;
    public float magicRegen;
    public GameObject[] cooldownTimers;
    private void Start()
    {
        magic = maxMagic;
        magicSlider.maxValue = maxMagic;
        magicSlider.minValue = 0;
        magicSlider.value = magic;
        allSpells[0] = Fireball;
        allSpells[1] = FireRing;
        for (int i = 0; i < 5; i++)
        {
            currentCharge[i] = 0f;
            charged[i] = false;
        }
    }
    private void FixedUpdate()
    {
        if (spellDisabled)
        {
            for (int i = 0; i < disabledSpells.Count; i++)
            {
                currentCharge[disabledSpells[i]] += Time.deltaTime;
                if (currentCharge[disabledSpells[i]] >= chargeTime[disabledSpells[i]])
                {
                    Debug.Log("reached");
                    currentCharge[disabledSpells[i]] = 0;
                    charged[disabledSpells[i]] = true;
                    Animator animator = cooldownTimers[disabledSpells[i]].GetComponent<Animator>();
                    animator.StopPlayback();
                    cooldownTimers[disabledSpells[i]].SetActive(false);
                    disabledSpells.RemoveAt(i);
                    if (disabledSpells.Count == 0)
                        spellDisabled = false;
                }
            }
        }
        magic = Mathf.Clamp(magic + magicRegen * Time.deltaTime, 0, maxMagic);
        magicSlider.value = magic;
    }
    public void EnableSpell(int pos, int i, Sprite sprite)
    {
        chosenImages[i].sprite = sprite;
        chosenImages[i].color = new Color(255, 255, 255, 255);
        if (disabledSpells.Contains(enabledSpells[i]))
        {
            disabledSpells.Remove(enabledSpells[i]);
            if (disabledSpells.Count == 0)
                spellDisabled = false;
        }
        enabledSpells[i] = pos;
        currentCharge[i] = 0f;
        charged[i] = true;
        spells[i] = allSpells[pos];
    }
    public void PerformSpell(int i)
    {
        int loc = enabledSpells[i];
        if (charged[i] && magic >= magicCost[loc])
        {
            spells[loc]();
            charged[i] = false;
            disabledSpells.Add(i);
            spellDisabled = true;
            Animator animator = cooldownTimers[i].GetComponent<Animator>();
            animator.speed = animator.runtimeAnimatorController.animationClips[0].length / chargeTime[i];
            cooldownTimers[i].SetActive(true);
            animator.Play("StateName");
            ReduceMagic(magicCost[loc]);
        }
    }
    void Fireball()
    {
        GameObject ball = Instantiate(fireball);
        ball.transform.position = new Vector3(transform.position.x, transform.position.y, ball.transform.position.z);
        ball.transform.up = -transform.right;
    }
    void FireRing()
    {
        GameObject ring = Instantiate(fireRing);
        ring.transform.position = new Vector3(transform.position.x, transform.position.y, ring.transform.position.z);
    }
    void ReduceMagic(float amount)
    {
        magic = Mathf.Clamp(magic - amount, 0, maxMagic);
        magicSlider.value = magic;
    }
}
