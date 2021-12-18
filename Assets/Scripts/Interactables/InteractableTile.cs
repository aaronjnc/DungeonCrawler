using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class InteractableTile : MonoBehaviour
{
    protected GameManager manager;
    private void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
    }
    public abstract void Interact();
}
