using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    public Image[] chosenImages;
    public Magic magicScript;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Places spell in chosen position
    /// </summary>
    /// <param name="image">Spell object</param>
    /// <param name="spell">Spell number</param>
    public void PlaceSpell(GameObject image, int spell)
    {
        Vector3 pos = image.transform.position;
        Sprite sprite = image.GetComponent<Image>().sprite;
        if (pos.y < chosenImages[0].gameObject.transform.position.y+(chosenImages[0].rectTransform.sizeDelta.y/2))
        {
            for (int i = 0; i < 5; i++)
            {
                float xCenter = chosenImages[i].gameObject.transform.position.x;
                if (pos.x < xCenter + (chosenImages[0].rectTransform.sizeDelta.x) && pos.x > xCenter - (chosenImages[0].rectTransform.sizeDelta.x))
                {
                    chosenImages[i].sprite = sprite;
                    chosenImages[i].color = new Color(255, 255, 255, 255);
                    magicScript.EnableSpell(spell, i, sprite);
                    break;
                }
            }
        }
    }
}
