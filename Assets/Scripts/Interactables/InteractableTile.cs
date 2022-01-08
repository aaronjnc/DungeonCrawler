using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Parent class for all interactbales
/// </summary>
public abstract class InteractableTile : MonoBehaviour
{
    //game manager reference
    protected GameManager manager;
    /// <summary>
    /// sets up manager
    /// </summary>
    private void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
    }
    /// <summary>
    /// method called when interacting with object
    /// </summary>
    public abstract void Interact();
}
